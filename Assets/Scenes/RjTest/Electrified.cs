using UnityEngine;
using System.Collections;

public class Electrified : MonoBehaviour {

    public bool Active;

	// Use this for initialization
	void Start () {
        Active = false;
	}

    // Update is called once per frame
    public bool Activate()
    {
        return Active = true;
    }

    public bool Deactivate()
    {
        return Active = false;
    }
}
