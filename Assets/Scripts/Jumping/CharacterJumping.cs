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
    private TouchHandling _inputSystem;
    private bool _jumping = false;
    private NavMeshAgent _nav;

    void Start ()
	{
	    if (!Sanity())
	    {
	        enabled = false;
            return;
	    }

	    foreach (var  jmpTag in TagsToJumpOnto)
            _inputSystem.RegisterTapHandlerByTag(jmpTag, JumpForJoy);

        _inputSystem.RegisterTapHandlerByTag("Terrain", ReturnToTerraFirma);
	}

    private bool Sanity()
    {
	    if (transform.root != transform)
	    {
	        Debug.Log("Jumping disabled: Character should be a root gameobject at the beginning of the game.");
	        return false;
	    }

        var scale = transform.lossyScale.y;
        _jumpHeight = MaximumVerticalJump * scale;
        _jumpWidth = MaximumHorizonalJump * scale;
        _dropHeight = MaximumDrop * scale;

	    if (TagsToJumpOnto.Length == 0)
	    {
	        Debug.Log("Jumping disabled: No tags to jump onto.");
	        return false;
	    }

	    var inputs = FindObjectsOfType<TouchHandling>();
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

        _inputSystem = inputs[0];

        _nav = GetComponent<NavMeshAgent>();
        if (!_nav)
        {
            Debug.Log("Jumping disabled: how did this even happen?");
            return false;
        }

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
        var v1 = target.position + new Vector3(0, target.lossyScale.y / 2, 0);

        // Get distance from feet of character
        var v2 = transform.position - new Vector3(0, transform.lossyScale.y / 2, 0);

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

        // Check if we are already on Terra Firma -- in which case we assume the player should not jump
        if (transform.root == transform)
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

    private IEnumerator Jumping(Vector3 target, Transform targetParent, bool restoreNagivation)
    {
        _jumping = true;
        _nav.enabled = false;

        // Correct target by height of the character so we land on our feet
        target += new Vector3(0, transform.lossyScale.y, 0);

        while (Vector3.Distance(transform.position, target) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, JumpingSpeed * Time.deltaTime);
            yield return null;
        }

        if (restoreNagivation)
            _nav.enabled = true;

        transform.parent = targetParent;
        _jumping = false;
    }
}