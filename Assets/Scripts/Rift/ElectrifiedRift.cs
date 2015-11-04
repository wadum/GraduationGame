using UnityEngine;
using System.Collections;

public class ElectrifiedRift : Electrified {

    // Use this for initialization
    void Start()
    {
        base.Deactivate();
    }

    // Update is called once per frame
    override public bool Activate()
    {
        return base.Activate();
    }

    override public bool Deactivate()
    {
        return base.Deactivate();
    }
}
