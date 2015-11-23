using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class RadialSlider : MonoBehaviour {

	public Color DefaultColor;
	public Color Wrong;
	public Color Right;
	public float TransitionSeconds;

	RectTransform _t;
	Canvas _c;
	Image _i;

	Coroutine _transitionRoutine;

	void Start()
	{
		_c = GameObject.FindGameObjectWithTag("Gui").GetComponentInChildren<Canvas>();
		_t = GetComponent<RectTransform>();
		_i = GetComponent<Image>();
	}
	
	public void SetValue(float value)
	{
		if(value < 1f)
		{
			_i.color = DefaultColor;
			if(_transitionRoutine != null)
				StopCoroutine(_transitionRoutine);
		}
		GetComponent<Image>().fillAmount = value;
	}
	
	public void SetPosition(Vector2 vec)
	{
		_t.anchoredPosition = vec/_c.scaleFactor;
	}

	public void TransitionWrong()
	{
		if(_transitionRoutine != null)
			StopCoroutine(_transitionRoutine);
		_transitionRoutine = StartCoroutine(TransitionColor(Wrong));
	}

	public void TransitionRight()
	{
		if(_transitionRoutine != null)
			StopCoroutine(_transitionRoutine);
		_transitionRoutine = StartCoroutine(TransitionColor(Right));
	}

	private IEnumerator TransitionColor(Color color)
	{
		float endTime = Time.time + TransitionSeconds;
		while( Time.time < endTime )
		{
			_i.color = Color.Lerp(DefaultColor, color, 1f-(endTime-Time.time)/TransitionSeconds);
			yield return null;
		}
	}
}
