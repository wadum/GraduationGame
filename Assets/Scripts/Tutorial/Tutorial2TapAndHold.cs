using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class Tutorial2TapAndHold : TutorialStep {

	public GameObject Forward;
	public GameObject Backward;

	override public IEnumerator Run()
	{
		MultiTouch.RegisterTapAndHoldHandlerByTag("TimeManipulationObject", hit => Completed = true);

		Backward.GetComponent<EventTrigger>().enabled = false;
		Backward.GetComponent<Button>().interactable = false;
		Forward.GetComponent<EventTrigger>().enabled = false;
		Forward.GetComponent<Button>().interactable = false;
		yield return StartCoroutine(base.Run());
	}
}
