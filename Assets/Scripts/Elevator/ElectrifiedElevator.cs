using UnityEngine;
using System.Collections;

public class ElectrifiedElevator : Electrified {

    public Collider _elevatorTrigger;
    public Animator Ani;

	// Use this for initialization
	void Start () {
        base.Deactivate();
	}

    // Update is called once per frame
    override public bool Activate()
    {
        //_elevatorTrigger.enabled = true;
        return base.Activate();
    }

    override public bool Deactivate()
    {
        //_elevatorTrigger.enabled = false;
        return base.Deactivate();
    }
}
