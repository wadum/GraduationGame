using UnityEngine;
using System.Collections;

public class GameOverlayController : MonoBehaviour {

	public GameObject StoryText;

	private Canvas _canvas;

	void Start()
	{
		_canvas = GetComponentInChildren<Canvas>();
		_canvas.worldCamera = Camera.main;
	}

	public void ShowStoryText(string textFile)
	{
		StoryText.SetActive(true);
		StoryText.GetComponent<StoryTextController>().Show(textFile);
	}
}
