using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

    Animator _animationController;
    NavMeshAgent _playerAI;
    Transform _playerPos;



    // Use this for initialization
    void Start () {
        _animationController = this.gameObject.GetComponent<Animator>();
        _playerAI = GetComponentInParent<NavMeshAgent>();
        _playerPos = GetComponentInParent<Transform>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckForWalkingOrStanding();

    }

    public void Jumping()
    {
            _animationController.Play("JumpUpTransition");
            _animationController.SetBool("StandingOnTheGound", false);
    }

    public void Landing()
    {
        _animationController.SetBool("StandingOnTheGound", true);
    }

    private void CheckForWalkingOrStanding()
    {
        if (Vector3.Distance(_playerAI.velocity, Vector3.zero) > 0)
        {
            _animationController.SetBool("VelocityGreaterThanZero", true);
            _animationController.Play("Walking");
        }
        else
        {
            _animationController.SetBool("VelocityGreaterThanZero", false);
      //      _animationController.Play("Idle");
        }
    }
}
