using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundBank : MonoBehaviour
{
    public AudioMixer GameMixer;
    public AudioSource Ambience,
        AmbienceReversed;
    public Slider GameSlider;

    private bool _reversed;
    private float _ambienceVol,
        _ambiencedReverseVol,
        _masterPitch,
        _oldPitch;
    private string _ambienceVariable = "ambVol",
        _ambienceReversedVariable = "ambRVol",
        _masterPitchVariable = "masterPit";

    void Start()
    {
        if (GameMixer == null)
            Debug.LogWarning("AudioMixer missing!");
        if (Ambience == null || AmbienceReversed == null)
            Debug.LogWarning("Ambience missing!");
        if (GameSlider == null)
            Debug.LogWarning("GameSlider missing! (should be found in GameOverlay)");

        _reversed = false;
        GameMixer.GetFloat(_ambienceVariable, out _ambienceVol);
        GameMixer.GetFloat(_masterPitchVariable, out _masterPitch);
        //GameMixer.GetFloat(_ambienceReversedVariable, out _ambiencedReverseVol);
        GameMixer.SetFloat(_ambienceReversedVariable, -80.0f);

        //Adds a listener to the main slider and invokes a method when the value changes.
        GameSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
    
    void Update()
    {
        #region buttons test
        //if (Input.GetKeyDown(KeyCode.R))
        //    ToggleBGM();

        //if (Input.GetKeyUp(KeyCode.K))
        //{
        //    _masterPitch += 1;
        //    //Debug.Log(_masterPitch);
        //    GameMixer.SetFloat(_masterPitchVariable, _masterPitch);
        //}
        //if (Input.GetKeyUp(KeyCode.L))
        //{
        //    _masterPitch -= 1;
        //    //Debug.Log(_masterPitch);
        //    GameMixer.SetFloat(_masterPitchVariable, _masterPitch);
        //}
        #endregion buttons test

        #region slider test

        if (GameSlider.value < 0)
            if (!_reversed)
                SetReverseAmbience();

        if (GameSlider.value > 0)
            if (_reversed)
                SetAmbience();

        CheckSliderToResetPitch();

        #endregion slider test
    }

    private void ValueChangeCheck()
    {
        GameMixer.GetFloat(_masterPitchVariable, out _oldPitch);
        float tmp = 0.0f;

        tmp = _oldPitch + GameSlider.value;
        if (tmp < 0.5f) tmp = 0.5f;
        if (tmp > 2.0f) tmp = 2.0f;
        GameMixer.SetFloat(_masterPitchVariable, tmp);
    }

    private void CheckSliderToResetPitch()
    {
        if (!GameSlider.IsActive() || GameSlider.value == 0)
        {
            SetAmbience();
            GameMixer.SetFloat(_masterPitchVariable, _masterPitch);
        }
    }

    private void SetAmbience()
    {
        _reversed = false;
        GameMixer.SetFloat(_ambienceVariable, _ambienceVol);
        GameMixer.SetFloat(_ambienceReversedVariable, -80.0f);
    }

    private void SetReverseAmbience()
    {
        _reversed = true;
        GameMixer.SetFloat(_ambienceVariable, -80.0f);
        GameMixer.SetFloat(_ambienceReversedVariable, _ambienceVol);
    }

    private void ToggleBGM()
    {
        if (_reversed)
            SetReverseAmbience();
        else
            SetAmbience();
    }
}