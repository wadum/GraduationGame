using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RadialSlider : MonoBehaviour {
	
	Vector2 _canvasFactor;
	RectTransform _t;
	Canvas _c;

	void Start()
	{
		_c = GameObject.FindGameObjectWithTag("Gui").GetComponentInChildren<Canvas>();
		_t = GetComponent<RectTransform>();
		_canvasFactor = new Vector2(1f/Screen.width*2048f, 1f/Screen.height*1536f);
	}

	public float GetValue()
	{
		return GetComponent<Image>().fillAmount;
	}
	
	public void SetValue(float value)
	{
		GetComponent<Image>().fillAmount = value;
	}
	
	public void SetPosition(Vector2 vec)
	{
		//vec.Scale(_canvasFactor);
		_t.anchoredPosition = vec/_c.scaleFactor;
	}
}
