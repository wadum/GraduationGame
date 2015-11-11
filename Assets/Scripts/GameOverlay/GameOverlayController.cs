using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverlayController : MonoBehaviour {

	public GameObject StoryText;
	public GameObject PauseMenu;
    public GameObject StoreScreen;
    public GameObject TimeSlider;
	public TimeSliderController SliderController;
	public AudioSource ActivateSliderSound;

    public static GameOverlayController gameOverlayController;

    Canvas _canvas;
    TimeControllable _currentObj;

    void Awake()
    {
        /*        if (gameOverlayController == null)
                {
                    DontDestroyOnLoad(gameObject);
                    gameOverlayController = this;
                }
                else if (gameOverlayController != this)
                {
                    Destroy(this.gameObject);
                }*/
        gameOverlayController = this;

    }

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

    public void ActivateSlider(TimeControllable obj)
    {
		if(ActivateSliderSound && (!TimeSlider.activeSelf || _currentObj != obj))
			ActivateSliderSound.Play();
        _currentObj = obj;
        TimeSlider.SetActive(true);
        TimeSlider.GetComponentInChildren<Slider>().value = _currentObj.GetFloat();
		SliderController.SetTimeControllable(obj);
    }

    public void DeactivateSlider()
    {
        TimeSlider.SetActive(false);
    }

    public void SetFloat(float var)
    {
        _currentObj.SetFloat(var);
    }
}
