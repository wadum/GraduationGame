﻿using UnityEngine;
using System.Collections;

public class CameraLock : MonoBehaviour {

    GameObject Player;

    public Vector3 RelataveStartPos;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = RelataveStartPos + Player.transform.position;
    }
}
