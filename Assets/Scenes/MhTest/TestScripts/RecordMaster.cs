using UnityEngine;
using System.IO;

public class RecordMaster : MonoBehaviour {
    private TimeTrackable[] trackers;
    public GameObject cylinder;
    public float State=0;
    private float PastState = 0;
    public static float time;

    // Use this for initialization
    void Awake()
    {
        trackers = GameObject.FindObjectsOfType<TimeTrackable>();

    }
    
    void Start () {
        foreach (TimeTrackable track in trackers)
        {
            track.tracking = true;
            track.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void SetFloat(float value)
    {
        State = value;
    }    

    void Update()
    {
        time += Time.deltaTime;
        if (State > 0 && PastState <= 0)
            foreach (TimeTrackable track in trackers)
            {
                time = 0;
                track.Move();
                track.forward = true;
                track.frozen = false;
            }
        if (State < 0 && PastState >= 0)
            foreach (TimeTrackable track in trackers)
            {
                time = 0;
                track.Move();
                track.forward = false;
                track.frozen = false;
            }
        if (State == 0)
            foreach(TimeTrackable track in trackers)
            {
                track.frozen = true;
            }

        PastState = State;
        if (Input.anyKey)
        {
            if(Input.GetKeyDown(KeyCode.A)){ Track(); }
            if (Input.GetKeyDown(KeyCode.D)) { Reset(); }
        }
    }
	
    public void Track()
    {
        if (cylinder)
        {
            cylinder.GetComponent<CapsuleCollider>().enabled = true;
            cylinder.GetComponent<OneSecondScript>().StartTime();

        }
        time = 0;

        foreach (TimeTrackable track in trackers)
        {
            track.GetComponent<Rigidbody>().isKinematic = false;
            track.Record();
        }
    }

    public void Reset()
    {
        Application.LoadLevel(0);
    }
}
