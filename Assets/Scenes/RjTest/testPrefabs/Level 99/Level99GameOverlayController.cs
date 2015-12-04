using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Level99GameOverlayController : MonoBehaviour
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

    public static Level99GameOverlayController gameOverlayController;

    private Canvas _canvas;
    private TimeControllable _currentObj;
    private AnimationController _animController;
    private CharacterMovement _player;

    private readonly List<CockRotator> _buttonsToRotate = new List<CockRotator>();

	List<SidekickElementController> _sideKickElementControllers = null;

    void Awake()
    {
        gameOverlayController = this;
    }

    void Start()
    {
        _canvas = GetComponentInChildren<Canvas>();
        _canvas.worldCamera = Camera.main;

        _animController = FindObjectOfType<AnimationController>();
        _buttonsToRotate.AddRange(GetComponentsInChildren<CockRotator>());
		_sideKickElementControllers = FindObjectsOfType<SidekickElementController>().ToList();
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
        var master = _currentObj.GetComponent<MasterHighlight>();
        if (master)
            master.Activate();      

        TimeSlider.GetComponentInChildren<Slider>().value = _currentObj.GetFloat();
        SliderController.SetTimeControllable(obj);

		_sideKickElementControllers.ForEach(s => s.MoveOut(obj));

        //move buttons in
        _buttonsToRotate.ForEach(cock => cock.moveIn = true);
        _buttonsToRotate.ForEach(cock => cock.MoveInside());

        if (_player == null)
            _player = FindObjectOfType<CharacterMovement>();

        _player.SetPlayerLookAtWhenMagic(true, obj.gameObject);
        _animController.StartMagic();

        // lol stop
        var agent = _player.GetComponent<NavMeshAgent>();
        if (agent && agent.isActiveAndEnabled)
            agent.destination = _player.transform.position;
    }

    public void DeactivateSlider()
    {
        if (_currentObj)
        {
            var master = _currentObj.GetComponent<MasterHighlight>();
            if (master)
                master.Deactivate();
        }
		if (_sideKickElementControllers != null)
			_sideKickElementControllers.ForEach(s => s.MoveBack());

        //move buttons out
        _buttonsToRotate.ForEach(cock => cock.moveIn = false);
        _buttonsToRotate.ForEach(cock => cock.MoveOutside());

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
        if (!_currentObj || !obj)
            return false;

        var tc = FindFirstTimeControllableIn(obj);
        return tc && tc == _currentObj;
    }

    private static TimeControllable FindFirstTimeControllableIn(GameObject target) {
        var parent = target.transform;
        while (parent) {
            var tc = parent.GetComponent<TimeControllable>();
            if (tc)
                return tc;

            parent = parent.parent;
        }

        return null;
    }
}