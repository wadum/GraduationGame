using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level99DragableButton : MonoBehaviour {

	public GameObject PowerUpChoosin;

	Vector3 _initialPosition;

	RectTransform _t;
	Canvas _c;
	Level99UIController _ui;

	public bool chosen {get;private set;}

	void Start()
	{
		_c = GetComponentInParent<Canvas>();
		_t = GetComponent<RectTransform>();
		_initialPosition = _t.anchoredPosition;
		_ui = FindObjectOfType<Level99UIController>();
	}

	public void DisableNonChosen()
	{
		gameObject.SetActive(chosen);
	}

	public void EnableNonChosen()
	{
		gameObject.SetActive(true);
	}

	public void StartDrag()
	{
		if(PowerUpChoosin.activeInHierarchy)
			StartCoroutine("Dragging");
	}

	public void StopDrag()
	{

		if(!PowerUpChoosin.activeInHierarchy) return;
		if(_ui.PowerUp1 == this)
			_ui.PowerUp1 = null;
		if(_ui.PowerUp2 == this)
			_ui.PowerUp2 = null;
		if(_ui.PowerUp3 == this)
			_ui.PowerUp3 = null;
		if(InRange(_ui.PowerUp1Pos) && !_ui.PowerUp1)
		{
			_t.anchoredPosition = _ui.PowerUp1Pos.anchoredPosition - new Vector2(0,55);
			_ui.PowerUp1 = this;
			chosen = true;
		} else if(InRange(_ui.PowerUp2Pos) && !_ui.PowerUp2)
		{
			_t.anchoredPosition = _ui.PowerUp2Pos.anchoredPosition - new Vector2(0,55);
			_ui.PowerUp2 = this;
			chosen = true;
		} else if(InRange(_ui.PowerUp3Pos) && !_ui.PowerUp3)
		{
			_t.anchoredPosition = _ui.PowerUp3Pos.anchoredPosition - new Vector2(0,55);
			_ui.PowerUp3 = this;
			chosen = true;
		}
		else {
			_t.anchoredPosition = _initialPosition;
			chosen = false;
		}
		StopCoroutine("Dragging");
	}

	IEnumerator Dragging()
	{
		while(true){
			Vector2 pos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_c.transform as RectTransform, Input.mousePosition, _c.worldCamera, out pos);
			transform.position = _c.transform.TransformPoint(pos);
			//transform.position = Input.mousePosition;
			yield return null;
		}
	}

	private bool InRange(RectTransform other)
	{
		return Vector2.Distance(other.anchoredPosition, _t.anchoredPosition) < 100;
	}
}
