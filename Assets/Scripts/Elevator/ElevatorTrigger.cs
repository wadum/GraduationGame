using UnityEngine;
using System.Collections;

public class ElevatorTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        GetComponentInParent<ElevatorController>().changePosition();
    }
}
