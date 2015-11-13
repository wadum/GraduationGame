﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeSliderController : MonoBehaviour {

    public float TimeSpeed;

    private Slider _slider;
	private SoundMaster _soundMaster;
	private TimeControllable _obj;

    private Coroutine _increaseCorout;
    private Coroutine _decreaseCorout;

    void Start()
    {
		_soundMaster = FindObjectOfType<SoundMaster>();
        _slider = GetComponentInChildren<Slider>();
    }

	public void SetTimeControllable (TimeControllable obj)
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
		if(_increaseCorout == null) return;
        StopCoroutine(_increaseCorout);
		_obj.StopMusic();
    }

    public void DecreaseTimePressed()
    {
		if(_soundMaster)
			_soundMaster.PlayReversed();
        _decreaseCorout = StartCoroutine(changeValue(-TimeSpeed));
		_obj.MusicBackward();
    }

    public void DecreaseTimeReleased()
    {
		if(_decreaseCorout == null) return;
		if(_soundMaster)
			_soundMaster.PlayNormal();
        StopCoroutine(_decreaseCorout);
		_obj.StopMusic();
    }

    IEnumerator changeValue(float var)
    {
        while (true)
        {
            _slider.value += 100f * Time.deltaTime/var;
            yield return null;
        }
    }
}
