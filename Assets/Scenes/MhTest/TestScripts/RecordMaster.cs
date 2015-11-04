using UnityEngine;
using System.Collections;

public class RecordMaster : MonoBehaviour {


    private TimeTrackable[] trackers;
    public GameObject cylinder;

    // Use this for initialization
    void Start () {
        trackers = GameObject.FindObjectsOfType<TimeTrackable>();
	}
	
    public void Track()
    {
        cylinder.GetComponent<CapsuleCollider>().enabled = true;
        cylinder.GetComponent<OneSecondScript>().StartTime();

        foreach (TimeTrackable track in trackers)
        {
            track.Record();
        }
    }

    public void Detrack()
    {
        foreach(TimeTrackable track in trackers)
        {
            track.Reverse();
        }
    }

    public void Reset()
    {
        Application.LoadLevel(0);
    }
}
