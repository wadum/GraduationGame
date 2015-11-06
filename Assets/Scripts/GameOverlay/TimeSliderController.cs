using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeSliderController : MonoBehaviour {

    public float TimeSpeed;
    Slider _slider;

    Coroutine _increaseCorout;
    Coroutine _decreaseCorout;

    void Start()
    {
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
        _decreaseCorout = StartCoroutine(changeValue(-TimeSpeed));
    }

    public void DecreaseTimeReleased()
    {
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
