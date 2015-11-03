using UnityEngine;
using System.Collections;

public class TimeZone : MonoBehaviour {

    public float TimeMultiplier = 1f;

    void OnTriggerExit(Collider collider)
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
    }
}
