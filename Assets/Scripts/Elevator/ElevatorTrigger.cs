using UnityEngine;
using System.Collections;

public class ElevatorTrigger : MonoBehaviour {

    public Animator Ani;
    public float WaitTime = 5;

    public GameObject UpperWall;
    public GameObject LowerWall;

    public GameObject Player
    {
        get; set;
    }
    public bool Triggered
    {
        get; set;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" || other.gameObject.transform.root != other.gameObject.transform) return;

        Player = other.gameObject;
        Triggered = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player" || other.gameObject.transform.root != other.gameObject.transform) return;

        Player = other.gameObject;
        Triggered = false;
    }


    void Update()
    {
        UpperWall.SetActive(!Ani.GetCurrentAnimatorStateInfo(0).IsName("UpPose"));
        LowerWall.SetActive(!Ani.GetCurrentAnimatorStateInfo(0).IsName("DownPose"));

        if (Ani.GetCurrentAnimatorStateInfo(0).IsName("MovingUp") || Ani.GetCurrentAnimatorStateInfo(0).IsName("MovingDown"))
        {
            if (Triggered)
            {
                Player.GetComponent<NavMeshAgent>().enabled = false;
                Player.transform.parent = transform;
            }
        }
    }


    public IEnumerator moveElevator()
    {
        while (true)
        {
            while (Ani.GetCurrentAnimatorStateInfo(0).IsName("MovingUp") || Ani.GetCurrentAnimatorStateInfo(0).IsName("MovingDown") || Ani.GetCurrentAnimatorStateInfo(0).IsName("New State"))
            {
                yield return null;
            }

            if (Triggered)
            {
                Player.GetComponent<NavMeshAgent>().enabled = true;
                Player.transform.parent = null;
            }

            yield return new WaitForSeconds(WaitTime);

            Ani.SetBool("Up", !Ani.GetBool("Up"));

            yield return new WaitForSeconds(1f);

        }
    }
}

