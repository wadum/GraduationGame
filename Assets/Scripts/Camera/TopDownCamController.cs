using UnityEngine;
using System.Collections;
using System;

public class TopDownCamController : MonoBehaviour {

    public Vector3 StartingPos;
    public const float ZoomSpeed = 100f;
	public const float RotateSpeed = 1f;
    public float UpDownRotationLimit = 80;

    private GameObject _player;
    private GameObject _cam;

    public float MaxZoomDistance = 20f;
    public float MinZoomDistance = 3f;

    private Vector3 _oldPlayerPos;
	private Vector3 _oldMousePos;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
		HandleKeyboardInput();
		HandleMouseInput();
    }

	public void Run(GameObject cam)
	{
		_cam = cam;
		_oldPlayerPos = _player.transform.position;
		StartCoroutine(AlwaysLookAt(_player));
	}

	private void HandleMouseInput()
	{
		if(Input.GetMouseButtonDown(1))
			_oldMousePos = Input.mousePosition;

		if(Input.GetMouseButton(1)){
			RotateUp(-1 * (Input.mousePosition.y - _oldMousePos.y));
			RotateRight(Input.mousePosition.x - _oldMousePos.x);
			_oldMousePos = Input.mousePosition;
		}
		ZoomIn(Input.mouseScrollDelta.y);
	}

	private void HandleKeyboardInput()
	{
		if (Input.GetKey(KeyCode.A))
			RotateRight();
		
		if (Input.GetKey(KeyCode.D))
			RotateLeft();
		
		if (Input.GetKey(KeyCode.W))
		{
			if(Input.GetKey(KeyCode.LeftShift))
				ZoomIn();
			else
				RotateUp();
		}
		
		if (Input.GetKey(KeyCode.S))
		{
			if(Input.GetKey(KeyCode.LeftShift))
				ZoomOut();
			else
				RotateDown();
		}
	}

	private void StayWithPlayer ()
	{
		_cam.transform.position += _player.transform.position - _oldPlayerPos;
		_oldPlayerPos = _player.transform.position;
	}

	private void ZoomOut (float distance = ZoomSpeed)
	{
		if (Vector3.Distance(_cam.transform.position, _player.transform.position) + distance > MaxZoomDistance) return;
		_cam.transform.position += -_cam.transform.forward * distance;
	}

	private void ZoomIn (float distance = ZoomSpeed)
	{
		if (Vector3.Distance(_cam.transform.position, _player.transform.position) - distance < MinZoomDistance) return;
		_cam.transform.position += _cam.transform.forward * distance;
	}

	private void RotateDown (float distance = RotateSpeed)
	{
		RotateUp(-distance);
	}

	private void RotateUp(float distance = RotateSpeed)
	{
		if (_cam.transform.localRotation.eulerAngles.x + distance < UpDownRotationLimit 
		    || _cam.transform.localRotation.eulerAngles.x > 360 - UpDownRotationLimit - 10)
			_cam.transform.RotateAround(_player.transform.position, _cam.transform.right, distance);
	}

	private void RotateLeft (float distance = RotateSpeed)
	{
		RotateRight(-distance);
	}

	private void RotateRight (float distance = RotateSpeed)
	{
		_cam.transform.RotateAround(_player.transform.position, _cam.transform.up, distance);
	}

    private IEnumerator AlwaysLookAt(GameObject go)
    {
        while(true)
        {
			StayWithPlayer();
            _cam.transform.LookAt(go.transform);
            yield return null;
        }
    }
}
