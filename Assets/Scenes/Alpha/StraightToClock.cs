using UnityEngine;
using System.Collections;

public class StraightToClock : MonoBehaviour {
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            Cockpart CK = GetComponent<Cockpart>();
            CK.goToCenterClock(CK.FloatWaypoints[0].transform.position);
        }
    }
}
