using UnityEngine;
using System.Collections;

public class LeaveElevator : MonoBehaviour {

    public ElevatorTrigger _trigger;

    void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        _trigger.Triggered = false;

    }


}
