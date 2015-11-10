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

    public string[] TagsToJumpOnto;

    private float _jumpHeight;
    private float _jumpWidth;
    private float _dropHeight;
    private bool _jumping = false;
    private NavMeshAgent _nav;
    private AnimationController _animator;

    void Start ()
	{
	    if (!Sanity())
	    {
	        enabled = false;
            return;
	    }

	    foreach (var  jmpTag in TagsToJumpOnto)
            MultiTouch.RegisterTapHandlerByTag(jmpTag, JumpForJoy);

        MultiTouch.RegisterTapHandlerByTag("Terrain", ReturnToTerraFirma);
	}

    private bool Sanity()
    {
	    if (transform.root != transform)
	    {
	        Debug.Log("Jumping disabled: Character should be a root gameobject at the beginning of the game.");
	        return false;
	    }

        // Get the scale of the character. If no collider information is available, we fall back to lossyscale.
        var col = GetComponent<Collider>();
        var scale = col ? col.bounds.extents.y : transform.lossyScale.y;

        _jumpHeight = MaximumVerticalJump * scale;
        _jumpWidth = MaximumHorizonalJump * scale;
        _dropHeight = MaximumDrop * scale;

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

        _animator = GetComponentInChildren<AnimationController>();
        if (!_animator)
            Debug.Log("Jumping animation disabled: no animation conroller found in children of attached player");

        return true;
    }

    private bool CanReach(Vector3 from, Vector3 to)
    {
        var heightDiff = to.y - from.y;
        var verticalDist = Mathf.Abs(from.y - to.y);

        if (heightDiff > 0 && verticalDist > _jumpHeight)
            return false;

        if (heightDiff <= 0 && verticalDist > _dropHeight)
            return false;

        var horizontalDist = Vector2.Distance(new Vector2(from.x, from.z), new Vector2(to.x, to.z));
        if (horizontalDist > _jumpWidth)
            return false;

        return true;
    }

    private void JumpForJoy(RaycastHit hit)
    {
        if (_jumping)
            return;

        // Check if we are already on the object
        if (transform.root == hit.collider.gameObject.transform)
            return;

        // Store current parent;
        var currentParent = transform.parent;

        // Detach the player and put him back into world coordinates
        transform.parent = null;

        var target = hit.collider.gameObject.transform;

        // Get location on top of target
        var v1 = target.position + new Vector3(0, hit.collider.bounds.extents.y, 0);

        // Get distance from feet of character
        var v2 = transform.position - new Vector3(0, hit.collider.bounds.extents.y, 0);

        if (!CanReach(v2, v1))
        {
            // Put the player back :|
            transform.parent = currentParent;
            return;
        }

        // Do the actual jump
        StartCoroutine(Jumping(v1, target, false));
    }

    private void ReturnToTerraFirma(RaycastHit hit)
    {
        if (_jumping)
            return;

        // Check if we are standing on a valid jumpable object, otherwise return
        if (!TagsToJumpOnto.Contains(transform.root.tag))
            return;

        // Store current parent;
        var currentParent = transform.parent;

        // Detach the player and put him back into world coordinates
        transform.parent = null;

        // Get location of hit, for exact jumping
        var v1 = hit.point;

        // Get distance from feet of character
        var v2 = transform.position - new Vector3(0, transform.lossyScale.y / 2, 0);

        if (!CanReach(v2, v1))
        {
            // Put the player back :|
            transform.parent = currentParent;
            return;
        }
        
        // Do the actual jump
        StartCoroutine(Jumping(v1, null, true));
    }

    private Func<float, Vector3> MakeBezierJump(Vector3 from, Vector3 to) {
        var xzStart = new Vector2(from.x, from.z);
        var xzEnd = new Vector2(to.x, to.z);
        var midway = xzStart + (xzEnd - xzStart)/2;

        var ctrl = new Vector3(midway.x, from.y + 2*_jumpHeight, midway.y);

        return t => Mathf.Pow(1 - t, 2)*from + 2*(1 - t)*t*ctrl + Mathf.Pow(t, 2)*to;
    }

    private IEnumerator Jumping(Vector3 target, Transform targetParent, bool restoreNagivation)
    {
        _jumping = true;
        _nav.enabled = false;

        if (_animator)
            _animator.Jumping();

        // Correct target by height of the character so we land on our feet
        target += new Vector3(0, transform.lossyScale.y, 0);
        var jumpCurve = MakeBezierJump(transform.position, target);

        var t = 0f;
        while (t <= 1) {
            transform.position = jumpCurve(t);
            t += Time.deltaTime / JumpingSpeed;
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