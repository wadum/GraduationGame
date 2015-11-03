using UnityEngine;
using System.Collections;

public class ElectrifiedElevator : Electrified {
    public ElevatorDoor _door;

	// Use this for initialization
	void Start () {
        _door.close();
        base.Deactivate();
	}

    // Update is called once per frame
    override public bool Activate()
    {
        _door.open();
        return base.Activate();
    }

    override public bool Deactivate()
    {
        _door.close();
        return base.Deactivate();
    }
}
