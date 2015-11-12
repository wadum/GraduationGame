using UnityEngine;
using UnityEngine.UI;

public class GameOverlayController : MonoBehaviour
{

    public GameObject StoryText;
    public GameObject PauseMenu;
    public GameObject StoreScreen;
    public GameObject TimeSlider;
    public TimeSliderController SliderController;
    public AudioSource ActivateSliderSound;

    public static GameOverlayController gameOverlayController;

    private Canvas _canvas;
    private TimeControllable _currentObj;
    private AnimationController _animController;
    private CharacterMovement _player;

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

        _animController = FindObjectOfType<AnimationController>();
    }

    void Update()
    {
        if (_animController == null)
            Debug.LogWarning("Character's animation controller missing from scene!");

        if (Input.GetKeyDown(KeyCode.Escape))
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
        if (PauseMenu.activeSelf) HidePauseMenu();
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
        if (ActivateSliderSound && (!TimeSlider.activeSelf || _currentObj != obj))
            ActivateSliderSound.Play();
        _currentObj = obj;
        MasterHighlight master = _currentObj.GetComponent<MasterHighlight>();
        if (master)
            master.Activate();
        TimeSlider.SetActive(true);
        TimeSlider.GetComponentInChildren<Slider>().value = _currentObj.GetFloat();
        SliderController.SetTimeControllable(obj);

        if (_player == null)
            _player = FindObjectOfType<CharacterMovement>();

        _player.SetPlayerLookAts(true, obj.gameObject);
        _animController.StartMagic();
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

        if (_animController == null)
            _animController = FindObjectOfType<AnimationController>();

        if (_player == null)
            _player = FindObjectOfType<CharacterMovement>();

        GameObject tmpGO = null;
        if (_currentObj != null)
            tmpGO = _currentObj.gameObject;

        _player.SetPlayerLookAts(false, tmpGO);
        _animController.StopMagic();
    }

    public void SetFloat(float var)
    {
        _currentObj.SetFloat(var);
    }
}
