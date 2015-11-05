using UnityEngine;
using System.Collections;

public class WaitForCog : MonoBehaviour {
    public GameObject Gear;
    LightningGenerator Generator;

    void Start()
    {
        Generator = GetComponent<LightningGenerator>();
    }
	// Update is called once per frame
	void Update () {
        if (Gear == null && !Generator.enabled )
            GetComponent<LightningGenerator>().enabled = true;
	}
}
