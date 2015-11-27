using UnityEngine;
using System.Collections;

public class Tutorial0SetUp : TutorialStep {

	public Transform MoveToPoint;
    public BaseDynamicCameraAI dynamicCameraAi;


    private GameObject _player;

	override public IEnumerator Run()
	{

		TutorialController.DisableTouch();

		yield return null;

		_player = GameObject.FindGameObjectWithTag("Player");
		_player.GetComponent<NavMeshAgent>().ResetPath();
		_player.GetComponent<NavMeshAgent>().SetDestination(MoveToPoint.position);
        dynamicCameraAi.AssumeDirectControl();

        while (_player.GetComponent<NavMeshAgent>().remainingDistance > 0.001f){
			yield return null;
		}


		/*yield return StartCoroutine(TutorialController.MoveCamera(CameraPoint.position, CameraRotationTime));*/

	}

}
