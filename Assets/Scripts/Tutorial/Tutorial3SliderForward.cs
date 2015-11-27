using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Tutorial3SliderForward : TutorialStep {

	public GameObject Forward;
    public GameObject Backward;

    public Slider TimeSlider;

	protected override bool Continue ()
	{
		return TimeSlider.value > 99;
	}

	override public IEnumerator Run()
	{
		/*TouchRequired = false;

		Forward.GetComponent<EventTrigger>().enabled = true;
		Forward.GetComponent<Button>().interactable = true;

		yield return StartCoroutine(base.Run());

		Forward.GetComponent<EventTrigger>().triggers.ForEach(t => {
			if(t.eventID == EventTriggerType.PointerUp) 
				t.callback.Invoke(new BaseEventData(GameObject.FindObjectOfType<EventSystem>()));
			});
		Forward.GetComponent<EventTrigger>().enabled = false;
		Forward.GetComponent<Button>().interactable = false;*/


        TouchRequired = false;

        Forward.GetComponent<EventTrigger>().enabled = true;
        Forward.GetComponent<Button>().interactable = true;

        yield return StartCoroutine(base.Run());

        Forward.GetComponent<EventTrigger>().triggers.ForEach(t => {
            if (t.eventID == EventTriggerType.PointerUp)
                t.callback.Invoke(new BaseEventData(GameObject.FindObjectOfType<EventSystem>()));
        });
        Backward.GetComponent<EventTrigger>().enabled = true;
        Backward.GetComponent<Button>().interactable = true;
        Forward.GetComponent<EventTrigger>().enabled = true;
        Forward.GetComponent<Button>().interactable = true;
        GameOverlayController.gameOverlayController.DeactivateSlider();
    }
}
