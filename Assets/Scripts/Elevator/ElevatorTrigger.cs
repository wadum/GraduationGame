using UnityEngine;
using System.Collections;

public class ElevatorTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        other.GetComponent<NavMeshAgent>().enabled = false;
        other.transform.parent = transform;
        GetComponentInParent<ElevatorController>().changePosition();
    }
}
