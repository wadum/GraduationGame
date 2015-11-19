using UnityEngine;
using System.Collections;

public class ElevatorTrigger : MonoBehaviour {

    public Animator Ani;
    public float WaitTime = 2;

    public GameObject ImActive;
    public GameObject UpperWall;
    public GameObject LowerWall;

    public AudioSource Audio_moving;

    float _aniTime;
    bool running = false;
    float timer = 0;

    public GameObject Player
    {
        get; set;
    }
    public bool Triggered
    {
        get; set;
    }

    void OnTriggerStay(Collider other)
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
        if (ImActive.gameObject.activeSelf == true)
        {
            _aniTime = 0;
            Ani.speed = 1;
            if (Audio_moving)
            {
                Audio_moving.UnPause();
            }

            if (running && Audio_moving && !Audio_moving.isPlaying)
            {
                Audio_moving.Play();
            }

            if (Ani.GetCurrentAnimatorStateInfo(0).IsName("New State"))
            {
                return;
            }

            UpperWall.SetActive(!Ani.GetCurrentAnimatorStateInfo(0).IsName("UpPose"));
            LowerWall.SetActive(!Ani.GetCurrentAnimatorStateInfo(0).IsName("DownPose"));

            if (Ani.GetCurrentAnimatorStateInfo(0).IsName("MovingUp") || Ani.GetCurrentAnimatorStateInfo(0).IsName("MovingDown"))
            {
                if (Triggered)
                {
                    Player.GetComponent<NavMeshAgent>().enabled = false;
                    Player.transform.parent = transform;
                }
                running = false;
                return;
            }

            timer += Time.deltaTime;

            if (!running && timer > WaitTime)
            {
                timer = 0;
                Ani.SetBool("Up", !Ani.GetBool("Up"));

                running = true;
            }
            if (Triggered)
            {
                Player.GetComponent<NavMeshAgent>().enabled = true;
                Player.transform.parent = null;
            }
        } else
        {
            _aniTime += Time.deltaTime;
            if (_aniTime > 0.3)
                if (Audio_moving)
                {
                    Audio_moving.Pause();
                }
            Ani.speed = 0;
        }
    }
}

