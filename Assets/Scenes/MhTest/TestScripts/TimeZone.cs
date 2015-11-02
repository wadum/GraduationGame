using UnityEngine;
using System.Collections;

public class TimeZone : MonoBehaviour {

    public float TimeMultiplier = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

/*    void OnTriggerExit(Collider collider)
    {
        TimeTrackable timescript = collider.GetComponent<TimeTrackable>();
        if (timescript)
        {
            timescript.TimeMultiplier = 1f;
        }

    }

    void OnTriggerStay(Collider collider)
    {
        TimeTrackable timescript  = collider.GetComponent<TimeTrackable>();
        if (timescript)
        {
            timescript.TimeMultiplier = TimeMultiplier;
        }
    }*/
}
