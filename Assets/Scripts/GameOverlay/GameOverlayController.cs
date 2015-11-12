using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class GameOverlayController : MonoBehaviour {

	public GameObject StoryText;
	public GameObject PauseMenu;
    public GameObject StoreScreen;
    public GameObject TimeSlider;
	public TimeSliderController SliderController;
	public AudioSource[] ActivateSliderSounds;

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

    public void GoToMainMenu()
    {
        if (!SaveLoad.saveLoad)
            Debug.Log("No SaveLoad class in game");
        else
        {
            SaveLoad.saveLoad.Save();
            SaveLoad.saveLoad.SaveInterval = 0;
        }
        Application.LoadLevel("Main Menu");
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
        if (obj == _currentObj)
            return;
        if (TimeSlider.activeSelf)
            DeactivateSlider();
        if (ActivateSliderSounds.Length > 0 && (!TimeSlider.activeSelf || _currentObj != obj))
			ActivateSliderSounds.ToList().ForEach(a => a.Play());
        _currentObj = obj;
        MasterHighlight master = _currentObj.GetComponent<MasterHighlight>();
        if(master)
            master.Activate();
        TimeSlider.SetActive(true);
        TimeSlider.GetComponentInChildren<Slider>().value = _currentObj.GetFloat();
		SliderController.SetTimeControllable(obj);
    }

    public void DeactivateSlider()
    {
        if (_currentObj)
        {
            MasterHighlight master = _currentObj.GetComponent<MasterHighlight>();
            if (master)
                master.Deactivate();
        }

        TimeSlider.SetActive(false);
        _currentObj = null;
    }

    public void SetFloat(float var)
    {
        _currentObj.SetFloat(var);
    }
}
