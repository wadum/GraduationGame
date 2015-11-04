using UnityEngine;
using System.Collections;

public class ElectrifiedBridge : Electrified
{
    public BridgeController Bridge;

    // Use this for initialization
    void Start()
    {
        Bridge.close();
        base.Deactivate();
    }

    // Update is called once per frame
    override public bool Activate()
    {
        Bridge.open();
        return base.Activate();
    }

    override public bool Deactivate()
    {
        Bridge.close();
        return base.Deactivate();
    }
}