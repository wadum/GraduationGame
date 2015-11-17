using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterJumping : MonoBehaviour
{
    public float MaximumVerticalJump = 2f;
    public float MaximumHorizonalJump = 5f;
    public float MaximumDrop = 10f;
    public float JumpingSpeed = 2f;
    public float JumpingHeightFactor = 0.6f;
    public float MaximumAcceptableSlant = 35f;

    public bool ControlNavJumps = true;

    public string[] TagsToJumpOnto;

    private float _height;
    private float JumpHeight { get { return _height * MaximumVerticalJump; } }
    private float JumpWidth { get { return _height * MaximumHorizonalJump; } }
    private Vector3 PlayerFeet { get { return _renderer.bounds.center - new Vector3(0, _height, 0); } }
    private float DropHeight { get { return _height * MaximumDrop; } }
    private bool _jumping = false;
    private NavMeshAgent _nav;
    private AnimationController _animator;
    private Renderer _renderer;
    private Vector3 _jumpingTarget;

    void Start()
    {
        if (!Sanity())
        {
            enabled = false;
            return;
        }

        if (ControlNavJumps)
        {
            _nav.autoTraverseOffMeshLink = false;
            StartCoroutine(HandleNavJumps());
        }

        foreach (var jmpTag in TagsToJumpOnto)
            MultiTouch.RegisterTapHandlerByTag(jmpTag, JumpForJoy);

        MultiTouch.RegisterTapHandlerByTag("Terrain", ReturnToTerraFirma);
    }

    private IEnumerator HandleNavJumps()
    {
        while (true) {
            if (_jumping) {
                yield return null;
                continue;
            }

            if (!_nav.isOnOffMeshLink)
            {
                yield return null;
                continue;
            }

            if (!_nav.currentOffMeshLinkData.activated)
            {
                yield return null;
                continue;
            }

            var dest = _nav.destination;
            // If we are close enough to the destination, we jump there instead of the off mesh link end pos
            var target = CanReach(PlayerFeet, dest) ?
                dest :
                _nav.currentOffMeshLinkData.endPos;

            yield return StartCoroutine(Jumping(target, null, true));

            _nav.CompleteOffMeshLink();
            _nav.destination = dest;
            _nav.Resume();
        }
    }

    private bool Sanity()
    {
        if (transform.root != transform)
        {
            Debug.Log("Jumping disabled: Character should be a root gameobject at the beginning of the game.");
            return false;
        }

        // Get the scale of the character. If no collider information is available, we fall back to lossyscale.
        //var col = GetComponent<Collider>();

        if (TagsToJumpOnto.Length == 0)
        {
            Debug.Log("Jumping disabled: No tags to jump onto.");
            return false;
        }

        var inputs = FindObjectsOfType<MultiTouch>();
        if (!inputs.Any())
        {
            Debug.Log("Jumping disabled: No input system in scene.");
            return false;
        }

        if (inputs.Length > 1)
        {
            Debug.Log("Jumping disabled: Too many input systems in scene, only one expected, change this script?");
            return false;
        }

        _nav = GetComponent<NavMeshAgent>();
        if (!_nav)
        {
            Debug.Log("Jumping disabled: how did this even happen?");
            return false;
        }

        _renderer = GetComponent<Renderer>();
        if (!_renderer)
        {
            Debug.Log("Jumping disabled: how did this even happen?");
            return false;
        }

        _height = _renderer.bounds.size.y;

        _animator = GetComponentInChildren<AnimationController>();
        if (!_animator)
            Debug.Log("Jumping animation disabled: no animation conroller found in children of attached player");

        return true;
    }

    private bool CanReach(Vector3 from, Vector3 to)
    {
        var heightDiff = to.y - from.y;
        var verticalDist = Mathf.Abs(from.y - to.y);

        if (heightDiff > 0 && verticalDist > JumpHeight)
            return false;

        if (heightDiff <= 0 && verticalDist > DropHeight)
            return false;

        var horizontalDist = Vector2.Distance(new Vector2(from.x, from.z), new Vector2(to.x, to.z));
        if (horizontalDist > JumpWidth)
            return false;

        return true;
    }

    private void JumpForJoy(RaycastHit hit)
    {
        if (_jumping)
            return;

        // Check if we are already on the object
        if (transform.parent == hit.collider.gameObject.transform)
            return;

        if (Vector3.Angle(hit.normal, Vector3.up) > MaximumAcceptableSlant)
            return;
        // Store current parent;
        var currentParent = transform.parent;

        // Detach the player and put him back into world coordinates
        transform.parent = null;

        // Get location on top of target
        var v1 = hit.point;

        // Get location at bottom of player
        var v2 = PlayerFeet;

        if (!CanReach(v2, v1))
        {
            // Put the player back :|
            transform.parent = currentParent;
            return;
        }

        // Do the actual jump
        StartCoroutine(Jumping(v1, hit.collider.gameObject.transform, false));
    }

    private void ReturnToTerraFirma(RaycastHit hit)
    {
        if (_jumping)
            return;
        if (!transform.parent)
            return;
        // Check if we are standing on a valid jumpable object, otherwise return
        if (!TagsToJumpOnto.Contains(transform.parent.tag))//!TagsToJumpOnto.Contains(transform.root.tag))
            return;

        // Store current parent;
        var currentParent = transform.parent;

        // Detach the player and put him back into world coordinates
        transform.parent = null;

        // Get location of hit, for exact jumping
        var v1 = hit.point;

        var v2 = PlayerFeet;

        if (!CanReach(v2, v1))
        {
            // Put the player back :|
            transform.parent = currentParent;
            return;
        }

        // Do the actual jump
        StartCoroutine(Jumping(v1, null, true));
    }

    private Func<float, Vector3> MakeBezierJump(Vector3 from, Vector3 to)
    {
        var dist = Vector3.Distance(from, to);
        var xzStart = new Vector2(from.x, from.z);
        var xzEnd = new Vector2(to.x, to.z);
        var midway = xzStart + (xzEnd - xzStart) / 2;

        var ctrl = new Vector3(midway.x, from.y + JumpingHeightFactor * dist, midway.y);

        return t => Mathf.Pow(1 - t, 2) * from + 2 * (1 - t) * t * ctrl + Mathf.Pow(t, 2) * to;
    }

    private IEnumerator Jumping(Vector3 target, Transform targetParent, bool restoreNagivation)
    {
        _jumping = true;
        _nav.enabled = false;

        GameOverlayController.gameOverlayController.DeactivateSlider();

        var lookDirection = Vector3.ProjectOnPlane(target - transform.position, Vector3.up);
        var lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        while (Vector3.Angle(lookDirection, transform.forward) > 0.1f) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 5f);
            yield return null;
        }

        if (_animator) {
            _animator.Jumping();
            yield return new WaitForSeconds(0.2f);
        }

        _jumpingTarget = target;

        var jumpingSpeed = Vector3.Distance(transform.position, target) / JumpWidth * JumpingSpeed;
        var jumpCurve = MakeBezierJump(transform.position, target);

        var t = 0f;
        while (t <= 1)
        {
            transform.position = jumpCurve(t);
            t += Time.deltaTime / jumpingSpeed;
            yield return null;
        }

        transform.position = target;
        if (restoreNagivation)
            _nav.enabled = true;

        if (_animator)
            _animator.Landing();

        transform.parent = targetParent;
        _jumping = false;
    }
}