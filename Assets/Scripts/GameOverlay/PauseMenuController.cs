﻿using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour {
	
	void OnDisable () {
		Time.timeScale = 1;
		Debug.Log("Unpaused.");
	}

	void OnEnable () {
		Time.timeScale = 0;
		Debug.Log("Paused.");
	}

	public void Resume()
	{
		gameObject.SetActive(false);
	}

	public void Restart()
	{
        if(SaveLoad.saveLoad)
            SaveLoad.saveLoad.Reset();
		Application.LoadLevel(Application.loadedLevel);
	}

}
