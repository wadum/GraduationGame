using UnityEngine;
using System.Collections;

public class RecordMaster : MonoBehaviour {


    private TimeTrackable[] trackers;

    // Use this for initialization
    void Start () {
        trackers = GameObject.FindObjectsOfType<TimeTrackable>();
	}
	
    public void Track()
    {
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
