using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class StoreScreenController : MonoBehaviour {

    public Text Timer;

	// Use this for initialization
	void Start () { 
	}
	
	// Update is called once per frame
	void Update () {
        var secondsRemaining = 24*60*60 - DateTime.Now.TimeOfDay.TotalSeconds;
        Timer.text = new DateTime(TimeSpan.FromSeconds(secondsRemaining).Ticks).ToString("hh:mm:ss");
    }
}
