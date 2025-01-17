﻿using UnityEngine;
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
    public bool tracking = false;

    // Use this for initialization
    void Awake()
    {
        trackers = transform.GetComponentsInChildren<TimeTrackable>();
        foreach(TimeTrackable track in trackers)
        {
            track.tracking = tracking;

        }
    }
    
    void Start () {
        InitializeSlider();
    }

    public void InitializeSlider()
    {
        tmp = 0;
        foreach (TimeTrackable track in trackers)
        {
            if (track.queue.Count == 0)
            {
                // Nothing
            }
            else
            {
                if (track.queue[track.queue.Count - 1].time > tmp)
                {
                    tmp = track.queue[track.queue.Count - 1].time;
                }

            }
        }
        time = _time;
        State = _time;
        slider.maxValue = tmp;
        slider.value = _time;
    }

    public void SetFloat(float value)
    {
        State = value;
   }

    void Update()
    {
        if(tracking)
        {
            time += Time.deltaTime;
            _time = time;
        }
        else
        {
            time = State;
            _time = time;
        }

        if (Input.anyKey)
        {
            if(Input.GetKeyDown(KeyCode.A)){ Track(); }
            if (Input.GetKeyDown(KeyCode.D)) { Reset(); }
        }
    }
	
    public void Track()
    {
        if (!tracking)
        {
            tracking = true;
            if (cylinder)
            {
                cylinder.GetComponent<CapsuleCollider>().enabled = true;
                cylinder.GetComponent<OneSecondScript>().StartTime();

            }
            time = 0;
            _time = time;

            foreach (TimeTrackable track in trackers)
            {
                track.GetComponent<Rigidbody>().isKinematic = false;
                track.Record();
            }
        }
        else
        {
            tracking = false;
            foreach (TimeTrackable track in trackers)
            {
                track.Move();
            }
            InitializeSlider();
        }
    }

    public void Reset()
    {
        Application.LoadLevel(0);
    }

    public void SaveState()
    {
    }
}
