using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeSliderController : MonoBehaviour {

    public float TimeSpeed;
    Slider _slider;
	SoundMaster _soundMaster;

    Coroutine _increaseCorout;
    Coroutine _decreaseCorout;

    void Start()
    {
		_soundMaster = FindObjectOfType<SoundMaster>();
        _slider = GetComponentInChildren<Slider>();
    }

    public void IncreaseTimePressed()
    {
        _increaseCorout = StartCoroutine(changeValue(TimeSpeed));
    }

    public void IncreaseTimeReleased()
    {
        StopCoroutine(_increaseCorout);
    }

    public void DecreaseTimePressed()
    {
		if(_soundMaster)
			_soundMaster.PlayAmbienceReversed();
        _decreaseCorout = StartCoroutine(changeValue(-TimeSpeed));
    }

    public void DecreaseTimeReleased()
    {
		if(_soundMaster)
			_soundMaster.PlayAmbience();
        StopCoroutine(_decreaseCorout);
    }

    IEnumerator changeValue(float var)
    {
        while (true)
        {
            _slider.value += var;
            yield return null;
        }
    }
}
