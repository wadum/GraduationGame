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

    private bool JumpForJoy(RaycastHit hit)
    {
        if (_jumping)
            return false;

        var v1 = hit.point;
        // If the angle is too high for our initial click, we look if we might be able to stand elsewhere on the model.
        if (Vector3.Angle(hit.normal, Vector3.up) > MaximumAcceptableSlant)
        {
            // we get the boundies of the box, these are not the same as the actual rendering, but it'll give us the center of the object
            Bounds bounds = hit.collider.GetComponent<Renderer>().bounds;
            RaycastHit hitInfo;
            // First we try a raycast, to hit the middle of the surface
            if (Physics.Raycast((bounds.center + Vector3.up * (1 + 2 * bounds.extents.y)), Vector3.down, out hitInfo))
            {
                float v = Vector3.Angle(hitInfo.normal, Vector3.up);
                if (v <= MaximumAcceptableSlant || Mathf.Abs(v - 180) <= MaximumAcceptableSlant)
                {
                    v1 = hitInfo.point;
                } else 
                // if the raycast missed, there might be holes in the model, so we're casting a spherecast instead
                if (Physics.SphereCast(bounds.center + Vector3.up * (1 + 2 * bounds.extents.y), Mathf.Max(bounds.extents.x, bounds.extents.z)/2, Vector3.down * (1 + 2 * bounds.extents.y), out hitInfo))
                {
// for debugging reasons I saw drawing a line, might want to draw again 
//  Debug.DrawRay(bounds.center + Vector3.up * (1 + 2 * Mathf.Max(bounds.extents.y)), Vector3.down * (1 + 2 * Mathf.Max(bounds.extents.y)), Color.green, 2);
                    v = Vector3.Angle(hitInfo.normal, Vector3.up);
                    if (v <= MaximumAcceptableSlant || Mathf.Abs(v - 180) <= MaximumAcceptableSlant)
                        v1 = hitInfo.point;
                    else return false;
                    // we didnt hit anything, so we dont know where to jump, hence we return
                }

            }
            else
            {
                Debug.DrawRay(bounds.center + Vector3.up * (1 + 2 * Mathf.Max(bounds.extents.y)), Vector3.down * (1 + 2 * Mathf.Max(bounds.extents.y)), Color.red, 2);
                return false;
            }
        }

        // Store current parent;
        var currentParent = transform.parent;

        // Detach the player and put him back into world coordinates
        transform.parent = null;


        // Get location at bottom of player
        var v2 = PlayerFeet;

        if (!CanReach(v2, v1))
        {
            // Put the player back :|
            transform.parent = currentParent;
            return false;
        }

        // Do the actual jump
        StartCoroutine(Jumping(v1, hit.collider.gameObject.transform, false));
		return true;
    }

    private bool ReturnToTerraFirma(RaycastHit hit)
    {
        if (_jumping)
            return false;
        if (!transform.parent)
            return true;
        // Check if we are standing on a valid jumpable object, otherwise return
        if (!TagsToJumpOnto.Contains(transform.parent.tag))//!TagsToJumpOnto.Contains(transform.root.tag))
            return false;

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
            return false;
        }

        // Do the actual jump
        StartCoroutine(Jumping(v1, null, true));
		return true;
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
        if (lookDirection != Vector3.zero)
        {
            var lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            while (Vector3.Angle(lookDirection, transform.forward) > 0.1f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 5f);
                yield return null;
            }
        }

        if (_animator) {
            _animator.Jumping();
            yield return new WaitForSeconds(0.2f);
        }

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

    // The sphere on the player has collided with something, if it's something jumpable, we enable the highlight script for that particular item.
    void OnTriggerEnter(Collider collider)
    {
        // if the object is of type jumpable
        if(TagsToJumpOnto.Contains(collider.tag))
        {
            // Since we have no good indication of the "rock"'s surface,  it's hard to tell if we can actually reach it
//            if (!CanReach(feet, collider.bounds.center)) // would be nice, but it doesn't work as easily as this might seem
                HighlightScript script = collider.gameObject.GetComponent<HighlightScript>();
                if (script)
                    script.Activate();
        }
    }

    // Something left the sphere around the player, if it's something jumpable, we deactivate the highlight script, so it no longer shines.
    void OnTriggerExit(Collider collider)
    {
        // if the object is of type jumpable
        if (TagsToJumpOnto.Contains(collider.tag))
        {
            HighlightScript script = collider.gameObject.GetComponent<HighlightScript>();
            if (script)
                script.Deactivate();
        }
    }
}