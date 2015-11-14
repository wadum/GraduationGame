using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialController : MonoBehaviour
{

    public bool ForceTutorial = false;
	public bool DisableTutorial = true;

	public List<TutorialStep> Steps;
	public List<GameObject> ObjectsToDestroy;

	private static MultiTouch _multiTouch;

	public const string PlayerPrefAlreadySeen = "TutorialAlreadySeen";
	
	void Awake ()
	{
		_multiTouch = GameObject.FindObjectOfType<MultiTouch>();
		if((PlayerPrefs.GetInt(PlayerPrefAlreadySeen) > 0 || DisableTutorial || Application.loadedLevelName != "lvl1") && !ForceTutorial)
			Destroy(gameObject);
	}

	void Start()
	{
		StartCoroutine(RunAllSteps());
	}

	IEnumerator RunAllSteps()
	{
		foreach(var step in Steps)
		{
			yield return StartCoroutine(step.Run());
		}

        PlayerPrefs.SetInt(PlayerPrefAlreadySeen, 1);

        Destroy(gameObject);
	}

	void OnDestroy()
	{
		EnableTouch();
		ObjectsToDestroy.ForEach(obj => {if(obj) Destroy(obj);});
		var cam = GameObject.FindObjectOfType<TopDownCamController>();
		if(cam)
			cam.RegisterHandlers();
	}

	public static IEnumerator MoveCamera(Vector3 desiredPos, float cameraRotationTime)
	{
		float time = 0;
		Vector3 origCamPos = Camera.main.transform.position;
		
		while(time < cameraRotationTime)
		{
			Camera.main.transform.position = Vector3.Slerp(origCamPos, desiredPos, time/cameraRotationTime);
			time += Time.deltaTime;
			yield return null;
		}
	}

	public static void EnableTouch() 
	{
		if(_multiTouch)
			_multiTouch.enabled = true;
	}

	public static void DisableTouch() 
	{
		if(_multiTouch)
			_multiTouch.enabled = false;
	}
}
