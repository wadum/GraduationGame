using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public WorldViewController WorldView;
    public GameObject cam;

	private Camera _mainCamera;

	// Use this for initialization
	void Start ()
	{
        //_mainCamera = Camera.main;
        cam.transform.position = WorldView.GetCurrentTransform().position;
        cam.transform.rotation = WorldView.GetCurrentTransform().rotation;
	}
	
	void Update ()
	{
		//if(_mainCamera != null)
			WorldView.Run(cam);
		//else {
			//_mainCamera = Camera.main;
        //    cam.transform.position = WorldView.GetCurrentTransform().position;
         //   cam.transform.rotation = WorldView.GetCurrentTransform().rotation;
		//}
	}
}
