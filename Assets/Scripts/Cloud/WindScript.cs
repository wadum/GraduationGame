using UnityEngine;
using System.Collections;

public class WindScript : MonoBehaviour {
    public float LifeSpan;
    public float TTL = 20f;
	
	// Update is called once per frame
	void Update () {
        LifeSpan += Time.deltaTime;
        if (LifeSpan >= TTL)
            Destroy(this.gameObject);
    }
}
