using UnityEngine;
using System.Collections;

public class ElectrifiedElevator : Electrified {

    public Collider _elevatorTrigger;
    public Animator Ani;

	// Use this for initialization
	void Start () {
        StartCoroutine(_elevatorTrigger.gameObject.GetComponent<ElevatorTrigger>().moveElevator());
        Ani.speed = 0;
        base.Deactivate();
	}

    // Update is called once per frame
    override public bool Activate()
    {
        Ani.speed = 1;
        //_elevatorTrigger.enabled = true;
        return base.Activate();
    }

    override public bool Deactivate()
    {
        Ani.speed = 0;
        //_elevatorTrigger.enabled = false;
        return base.Deactivate();
    }
}
