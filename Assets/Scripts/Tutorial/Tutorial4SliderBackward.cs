using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Tutorial4SliderBackward : TutorialStep {

	public GameObject Forward;
	public GameObject Backward;

	public Slider TimeSlider;

	protected override bool Continue ()
	{
		return TimeSlider.value < 30;
	}

	override public IEnumerator Run()
	{
		/*TouchRequired = false;

		Backward.GetComponent<EventTrigger>().enabled = true;
		Backward.GetComponent<Button>().interactable = true;

		yield return StartCoroutine(base.Run());

		Backward.GetComponent<EventTrigger>().triggers.ForEach(t => {
			if(t.eventID == EventTriggerType.PointerUp) 
				t.callback.Invoke(new BaseEventData(GameObject.FindObjectOfType<EventSystem>()));
			});
		Backward.GetComponent<EventTrigger>().enabled = true;
		Backward.GetComponent<Button>().interactable = true;
		Forward.GetComponent<EventTrigger>().enabled = true;
		Forward.GetComponent<Button>().interactable = true;
		GameOverlayController.gameOverlayController.DeactivateSlider();*/

        TouchRequired = false;

        Backward.GetComponent<EventTrigger>().enabled = true;
        Backward.GetComponent<Button>().interactable = true;

        yield return StartCoroutine(base.Run());

        Backward.GetComponent<EventTrigger>().triggers.ForEach(t => {
            if (t.eventID == EventTriggerType.PointerUp)
                t.callback.Invoke(new BaseEventData(GameObject.FindObjectOfType<EventSystem>()));
        });
        Backward.GetComponent<EventTrigger>().enabled = false;
        Backward.GetComponent<Button>().interactable = false;


    }
}
