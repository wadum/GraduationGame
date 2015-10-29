using UnityEngine;
using System.Collections;

public class OneSecondScript : MonoBehaviour {

    private float TTL = 1f;
    private float elapsedTime = 0;
    private bool StartTimed = false;
	
	// Update is called once per frame
	void Update () {
        if (!StartTimed)
            return;
        if (elapsedTime > TTL)
            GetComponent<CapsuleCollider>().enabled = false;
        elapsedTime += Time.deltaTime;
	}

    public void StartTime()
    {
        elapsedTime = 0;
        StartTimed = true;
    }
}
