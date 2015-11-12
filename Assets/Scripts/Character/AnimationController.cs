using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator _animationController;
    private NavMeshAgent _playerAI;
    // private Transform _playerPos;

    // Use this for initialization
    void Start()
    {
        _animationController = this.gameObject.GetComponent<Animator>();
        _playerAI = GetComponentInParent<NavMeshAgent>();
        // _playerPos = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWalkingOrStanding();
    }

    public void Jumping()
    {
        //Debug.Log("Jumping");
        _animationController.SetBool("StandingOnTheGound", false);
        //_animationController.Play("JumpUpTransition");
    }

    public void Landing()
    {
        //Debug.Log("Landing");
        _animationController.SetBool("StandingOnTheGound", true);
    }

    public void StartMagic()
    {
        //Debug.Log("StartMagic");
        _animationController.SetBool("Magic", true);
    }

    public void StopMagic()
    {
        //Debug.Log("StopMagic");
        _animationController.SetBool("Magic", false);
    }

    private void CheckForWalkingOrStanding()
    {
        if (Vector3.Distance(_playerAI.velocity, Vector3.zero) > 0)
        {
            _animationController.SetBool("VelocityGreaterThanZero", true);
            //_animationController.Play("Walking");
        }
        else
        {
            _animationController.SetBool("VelocityGreaterThanZero", false);
            //_animationController.Play("Idle");
        }
    }
}
