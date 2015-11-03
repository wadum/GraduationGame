using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class RecordMaster : MonoBehaviour {
    private TimeTrackable[] trackers;
    public GameObject cylinder;
    public float State=0;
    public static float time;
    public float _time;
    public float tmp;
    public Slider slider;
    private float epsilon = 0.1f;
    bool tracking = false;
    // Use this for initialization
    void Awake()
    {
        trackers = GameObject.FindObjectsOfType<TimeTrackable>();

    }
    
    void Start () {
        tmp = 0 ;
        foreach (TimeTrackable track in trackers)
        {
            if (track.queue.Count == 0)
            {
                track.tracking = true;
                track.GetComponent<Rigidbody>().isKinematic = true;

            }
            else
            {
                if (track.queue[track.queue.Count - 1].time > tmp)
                {
                    tmp = track.queue[track.queue.Count - 1].time;
                }

            }
        }
        slider.maxValue = tmp;
        time = tmp;
        State = tmp;
        slider.value = tmp;
    }

    public void SetFloat(float value)
    {
        State = value;
    }    

    void Update()
    {
        if (!tracking)
        {
                time = State;
        }

        if (Input.anyKey)
        {
            if(Input.GetKeyDown(KeyCode.A)){ Track(); }
            if (Input.GetKeyDown(KeyCode.D)) { Reset(); }
        }
    }
	
    public void Track()
    {
        tracking = true;
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
