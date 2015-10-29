﻿using UnityEngine;
using System.Collections;

public class GameOverlayController : MonoBehaviour {

	public GameObject StoryText;
	public GameObject PauseMenu;

	private Canvas _canvas;

	void Start()
	{
		_canvas = GetComponentInChildren<Canvas>();
		_canvas.worldCamera = Camera.main;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			TogglePauseMenu();
	}

	public void ShowStoryText(string textFile)
	{
		StoryText.SetActive(true);
		StoryText.GetComponent<StoryTextController>().Show(textFile);
	}

	public void TogglePauseMenu()
	{
		if(PauseMenu.activeSelf) HidePauseMenu();
		else ShowPauseMenu();
	}

	public void ShowPauseMenu()
	{
		PauseMenu.SetActive(true);
	}

	public void HidePauseMenu()
	{
		PauseMenu.SetActive(false);
	}
}
