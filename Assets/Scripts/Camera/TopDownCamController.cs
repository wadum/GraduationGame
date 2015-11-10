using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System;

public class TopDownCamController : MonoBehaviour {

    public Vector3 StartingPos;
    public const float ZoomSpeed = 0.5f;
	public const float RotateSpeed = 1f;
    public float UpDownRotationLimit = 80;
    public bool ForceTouch = false;

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
        if (!Application.isMobilePlatform && !ForceTouch)
        {
    		HandleKeyboardInput();
    		HandleMouseInput();
        }
    }

	public void Run(GameObject cam)
	{
		_cam = cam;
		_oldPlayerPos = _player ? _player.transform.position : (_player = GameObject.FindGameObjectWithTag("Player")).transform.position;

        MultiTouch.RegisterPinchHandler(HandlePinch);

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
            if (Input.GetKey(KeyCode.LeftShift))
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

    private void HandlePinch(List<Touch> touches)
    {
        Func<List<Vector2>, Vector2> averagePoint = vectors => vectors.Aggregate((v1, v2) => v1 + v2) / vectors.Count;
        Func<List<Vector2>, Vector2, float> averageDistanceToPoint = (vectors, point) => vectors.Select(p => Vector2.Distance(p, point)).Average();
        Func<Vector2, Vector2> screenNormalize = v => new Vector2(v.x / Screen.width, v.y / Screen.height);

        var positions = touches.Select(t => t.position).ToList();
        var center = averagePoint(positions);
        var averageDistanceToCenter = averageDistanceToPoint(positions, center);

        var previousPositions = touches.Select(t => t.position - t.deltaPosition).ToList();
        var previousCenter = averagePoint(previousPositions);
        var previousAverageDistanceToCenter = averageDistanceToPoint(previousPositions, previousCenter);

        var centerMovementDirection = screenNormalize(center - previousCenter);
        var centerDistanceRatio = averageDistanceToCenter/previousAverageDistanceToCenter;

        // TODO: REMOVE THIS LATER, preferably this whole script. This is a quickfix.
        const float eyeballedZoomFactor = 20;
        const float eyeballedRotateFactor = 900;

        if (centerDistanceRatio < 1)
            ZoomOut((1 - centerDistanceRatio) * eyeballedZoomFactor);
        else
            ZoomIn((centerDistanceRatio - 1) * eyeballedZoomFactor);

        var rotate = centerMovementDirection*eyeballedRotateFactor;

        RotateUp(-rotate.y);
        RotateRight(rotate.x);
    }
}