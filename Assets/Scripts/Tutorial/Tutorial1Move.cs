using UnityEngine;
using System.Collections;

public class Tutorial1Move : TutorialStep {

	override public IEnumerator Run()
	{
		MultiTouch.RegisterTapHandlerByTag("Terrain", hit => Completed = true);

		yield return StartCoroutine(base.Run());
	}
}