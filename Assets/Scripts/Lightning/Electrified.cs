using UnityEngine;
using System.Collections;

public class Electrified : MonoBehaviour {

    public bool Active;

    // Use this for initialization
    void Start()
    {
        Active = false;
    }

    // Update is called once per frame
    public virtual bool Activate()
    {
        return Active = true;
    }

    public virtual bool Deactivate()
    {
        return Active = false;
    }
}
