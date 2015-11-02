﻿using UnityEngine;
using System.IO;

public class RecordMaster : MonoBehaviour {
    private TimeTrackable[] trackers;
    public GameObject cylinder;


    // Use this for initialization
    void Awake()
    {
        trackers = GameObject.FindObjectsOfType<TimeTrackable>();

    }
    void Start () {
        foreach (TimeTrackable track in trackers)
        {
            if (File.Exists(track.path + track.name + ".dat"))
            {
                return;
            }
            track.tracking = true;
            track.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Update()
    {
        if (Input.anyKey)
        {
            if(Input.GetKeyDown(KeyCode.A)){ Track(); }
            if (Input.GetKeyDown(KeyCode.S)) { Detrack(); }
            if (Input.GetKeyDown(KeyCode.D)) { Reset(); }
            if (Input.GetKeyDown(KeyCode.Q)) {
                foreach(TimeTrackable track in trackers)
                {
                    track.Save();
                }
            }
            if (Input.GetKeyDown(KeyCode.W)) {
                foreach (TimeTrackable track in trackers)
                {
                    track.Load();
                }
            }
        }
    }
	
    public void Track()
    {
        cylinder.GetComponent<CapsuleCollider>().enabled = true;
        cylinder.GetComponent<OneSecondScript>().StartTime();

        foreach (TimeTrackable track in trackers)
        {
            track.GetComponent<Rigidbody>().isKinematic = false;
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
