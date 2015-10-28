using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public WorldViewController WorldView;

	private Camera _mainCamera;

	// Use this for initialization
	void Start ()
	{
		_mainCamera = Camera.main;
		_mainCamera.transform.position = WorldView.GetCurrentTransform().position;
		_mainCamera.transform.rotation = WorldView.GetCurrentTransform().rotation;
	}
	
	void Update ()
	{
		if(_mainCamera != null)
			WorldView.Run(_mainCamera);
		else {
			_mainCamera = Camera.main;
			_mainCamera.transform.position = WorldView.GetCurrentTransform().position;
			_mainCamera.transform.rotation = WorldView.GetCurrentTransform().rotation;
		}
	}
}
