using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeSliderController : MonoBehaviour {

    public float TimeSpeed;
    Slider _slider;
	SoundMaster _soundMaster;
	TimeControllable _obj;

    Coroutine _increaseCorout;
    Coroutine _decreaseCorout;

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
