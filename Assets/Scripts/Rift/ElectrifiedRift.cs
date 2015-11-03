using UnityEngine;
using System.Collections;

public class ElectrifiedRift : Electrified {

    LightningGenerator _partnerGenerator;

    // Use this for initialization
    void Start()
    {
        _partnerGenerator = GetComponentInParent<RiftScript>().Partner.GetComponentInChildren<LightningGenerator>();
        _partnerGenerator.enabled = false;
        base.Deactivate();
    }

    // Update is called once per frame
    override public bool Activate()
    {
        _partnerGenerator.enabled = true;
        return base.Activate();
    }

    override public bool Deactivate()
    {
        _partnerGenerator.enabled = false;
        return base.Deactivate();
    }
}
