using UnityEngine;
using System.Collections;

public abstract class TutorialStep : MonoBehaviour {

	public GameObject Overlay;
	public GameObject Text;
	//public Transform CameraPoint;
	
	public float CameraRotationTime = 1.5f;
	
	protected bool Completed;
	protected bool TouchRequired = true;
	private MultiTouch _multiTouch;

	protected virtual bool Continue()
	{
		return Completed;
	}

	public virtual IEnumerator Run()
	{
		Completed = false;

        if (Overlay) Overlay.SetActive(true);
        if (TouchRequired) TutorialController.EnableTouch();
		if(Text) Text.SetActive(true);
		
		while(!Continue()) yield return null;
        
		if(Overlay) Overlay.SetActive(false);
        if (TouchRequired) TutorialController.DisableTouch();
        if (Text) Text.SetActive(false);

		/*if(CameraPoint)
			yield return StartCoroutine(TutorialController.MoveCamera(CameraPoint.position, CameraRotationTime));*/

	}
}
