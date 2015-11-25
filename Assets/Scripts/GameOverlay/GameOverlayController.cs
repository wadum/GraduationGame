using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameOverlayController : MonoBehaviour
{

    public GameObject StoryText;
    public GameObject PauseMenu;
    public GameObject PauseButton;
    public GameObject StoreScreen;
    public GameObject TimeSlider;
    public GameObject SoundPopup;
    public GameObject ButtonsToHide;
    public GameObject ControlsPicture;
    public TimeSliderController SliderController;
    public List<AudioSource> ActivateSliderSounds;
    public AudioSource MenuClickSound;

    public static GameOverlayController gameOverlayController;

    private Canvas _canvas;
    private TimeControllable _currentObj;
    private AnimationController _animController;
    private CharacterMovement _player;

    private List<CockRotator> _bottunsToRotate = new List<CockRotator>();

    void Awake()
    {
        gameOverlayController = this;
    }

    void Start()
    {
        _canvas = GetComponentInChildren<Canvas>();
        _canvas.worldCamera = Camera.main;

        _animController = FindObjectOfType<AnimationController>();
        _bottunsToRotate.AddRange(GetComponentsInChildren<CockRotator>());
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
        if (MenuClickSound)
            MenuClickSound.Play();
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
        if (MenuClickSound)
            MenuClickSound.Play();
        PauseButton.SetActive(!PauseButton.activeSelf);
        PauseMenu.SetActive(!PauseMenu.activeSelf);
    }

    public void ToggleTutorialPicture()
    {
        if (MenuClickSound)
            MenuClickSound.Play();
        ControlsPicture.SetActive(!ControlsPicture.activeSelf);

    }

    public void ActivateSlider(TimeControllable obj)
    {
        if (obj == _currentObj)
            return;
        if (TimeSlider.activeSelf)
            DeactivateSlider();
        if (ActivateSliderSounds != null && (!TimeSlider.activeSelf || _currentObj != obj))
            ActivateSliderSounds.ForEach(a => a.Play());
        _currentObj = obj;
        MasterHighlight master = _currentObj.GetComponent<MasterHighlight>();
        if (master)
            master.Activate();      

        TimeSlider.GetComponentInChildren<Slider>().value = _currentObj.GetFloat();
        SliderController.SetTimeControllable(obj);

        //move buttons in
        _bottunsToRotate.ForEach(cock => cock.moveIn = true);
        _bottunsToRotate.ForEach(cock => cock.MoveInside());

        if (_player == null)
            _player = FindObjectOfType<CharacterMovement>();

        _player.SetPlayerLookAtWhenMagic(true, obj.gameObject);
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
        //move buttons out
        _bottunsToRotate.ForEach(cock => cock.moveIn = false);
        _bottunsToRotate.ForEach(cock => cock.MoveOutside());

        _currentObj = null;

        if (_animController == null)
            _animController = FindObjectOfType<AnimationController>();

        if (_player == null)
            _player = FindObjectOfType<CharacterMovement>();

        GameObject tmpGO = null;
        if (_currentObj != null)
            tmpGO = _currentObj.gameObject;

        _player.SetPlayerLookAtWhenMagic(false, tmpGO);
        _animController.StopMagic();
    }

    public void SetFloat(float var)
    {
        _currentObj.SetFloat(var);
    }

    public void ToggleVolume()
    {
        if (MenuClickSound)
            MenuClickSound.Play();

        SoundPopup.SetActive(!SoundPopup.activeSelf);
        ButtonsToHide.SetActive(!ButtonsToHide.activeSelf);
    }

    public void ClickSound()
    {
        if (MenuClickSound)
            MenuClickSound.Play();
    }

    public bool IsCurrentlySelected(GameObject obj) {
        // time controllable objects should always be at root, so we compare those.
        return _currentObj && _currentObj.transform.root == obj.transform.root;
    }
}