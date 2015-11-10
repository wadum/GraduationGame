using UnityEngine;
using System.Collections;

public class ElevatorTrigger : MonoBehaviour {

    public Animator Ani;
    public float Cooldown = 10;

    private float lastUsed = -10;

	void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.gameObject.transform.root != other.gameObject.transform) return;
        if (lastUsed + Cooldown > Time.time) return;
        lastUsed = Time.time;

        other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        other.gameObject.transform.root.parent = this.transform;

        StartCoroutine(moveElevator(other.gameObject));
    }

    IEnumerator moveElevator(GameObject player)
    {

        Ani.SetBool("Up", !Ani.GetBool("Up"));

        yield return null;

        while (Ani.GetCurrentAnimatorStateInfo(0).IsName("MovingUp") || Ani.GetCurrentAnimatorStateInfo(0).IsName("MovingDown"))
        {
            yield return null;
        }

        player.GetComponent<NavMeshAgent>().enabled = true;
        player.transform.root.parent = null;

        lastUsed = Time.time;

    }
}

