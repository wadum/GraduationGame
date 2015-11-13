using UnityEngine;
using System.Collections;

public class Electrified : MonoBehaviour {

    public bool Active;

    // Update is called once per frame
    public virtual bool Activate()
    {
        return Active = true;
    }

    public virtual bool Deactivate()
    {
        return Active = false;
    }

    void OnDisable()
    {
        Deactivate();
    }
}
