using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverlayController : MonoBehaviour {

	public GameObject StoryText;
	public GameObject PauseMenu;
    public GameObject StoreScreen;
    public GameObject TimeSlider;

    Canvas _canvas;
    TimeControllable _currentObj;

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

    public void ShowStore()
    {
        StoreScreen.SetActive(true);
    }

    public void HideStore()
    {
        StoreScreen.SetActive(false);
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

    public void Activate(TimeControllable obj)
    {
        _currentObj = obj;
        TimeSlider.SetActive(true);
        TimeSlider.GetComponentInChildren<Slider>().value = _currentObj.GetFloat();
    }

    public void Deactivate()
    {
        TimeSlider.SetActive(false);
    }

    public void SetFloat(float var)
    {
        _currentObj.SetFloat(var);
    }
}
