using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TimeSliderController : MonoBehaviour
{

    public float TimeSpeed;
    public float JaggingTime = 0.5f;
    public float JaggingCD = 0.5f;
    public bool HalfJaggedSpeed = false;

    //Audio sources
    public AudioSource
        BackwardsButton,
        ForwardsButton;

    private AudioSource _jaggeringSound;

    private Slider _slider;
    private SoundMaster _soundMaster;
    private TimeControllable _obj;

    private Coroutine _increaseCorout;
    private Coroutine _decreaseCorout;
    private float bar { get { return JaggingTime; } set { JaggingTime = value; } }
    private List<CockRotator> _bottunsToRotate = new List<CockRotator>();

    void Start()
    {
        _soundMaster = FindObjectOfType<SoundMaster>();
        _slider = GetComponentInChildren<Slider>();
        _jaggeringSound = GetComponent<AudioSource>();
        _bottunsToRotate.AddRange(GetComponentsInChildren<CockRotator>());
    }

    public void SetTimeControllable(TimeControllable obj)
    {
        _obj = obj;
    }

    public void IncreaseTimePressed()
    {
        _increaseCorout = StartCoroutine(changeValue(TimeSpeed));
        _obj.MusicForward();
    }

    public void IncreaseTimeReleased()
    {
        if (_increaseCorout == null) return;
        StopCoroutine(_increaseCorout);
        _obj.StopMusic();
    }

    public void DecreaseTimePressed()
    {
        if (_soundMaster)
            _soundMaster.PlayReversed();
        _decreaseCorout = StartCoroutine(changeValue(-TimeSpeed));
        _obj.MusicBackward();
    }

    public void DecreaseTimeReleased()
    {
        if (_decreaseCorout == null) return;
        if (_soundMaster)
            _soundMaster.PlayNormal();
        StopCoroutine(_decreaseCorout);
        _obj.StopMusic();
    }

    IEnumerator changeValue(float var)
    {
        bool jagging = false;
        float foo = 0;
        float jaggingCDTime = 10;
        while (true)
        {
            while (jagging)
            {

                jaggingCDTime += Time.deltaTime;
                if (jaggingCDTime > JaggingCD)
                {
                    foo += Time.deltaTime;
                    _bottunsToRotate.ForEach(cock => cock.RotateWheel(Mathf.Sign(var), HalfJaggedSpeed));

                    BackwardsButton.mute = true;
                    ForwardsButton.mute = true;
                    if (_jaggeringSound && !_jaggeringSound.isPlaying)
                        _jaggeringSound.Play();

                    if (foo > bar)
                    {
                        

                        foo = 0;
                        _bottunsToRotate.ForEach(cock => cock.ResetRot());
                        jaggingCDTime = 0;
                    }
                }
                yield return null;
                continue;
            }
            BackwardsButton.mute = false;
            ForwardsButton.mute = false;
            _slider.value += 100f * Time.deltaTime / var;
            if (_slider.value < 100 && _slider.value > 0)
            {

                _bottunsToRotate.ForEach(cock => cock.RotateWheel(Mathf.Sign(var)));
            }
            else
            {
                jagging = true;
                _bottunsToRotate.ForEach(cock => cock.SaveRot());
            }
            yield return null;
        }
    }
}
