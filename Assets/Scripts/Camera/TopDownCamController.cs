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
    public GameObject InputSystem;
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

	    TouchHandling touchHandling = null;
        if (InputSystem)
            touchHandling = InputSystem.GetComponent<TouchHandling>();

	    if (touchHandling)
            touchHandling.RegisterPinchHandler(HandlePinch);

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
        // takes the average vector of a list of vectors
        Func<List<Vector2>, Vector2> avg = vectors => vectors.Aggregate((v1, v2) => v1 + v2) / vectors.Count;
        // takes the average distance between a list of vectors and a point
        Func<List<Vector2>, Vector2, float> avgDist = (vectors, point) => vectors.Select(p => Vector2.Distance(p, point)).Average();

        // transforms the list of touches to a list of positions
        var pos = touches.Select(t => t.position).ToList();
        
        // transforms the list of touches to a list of the previous positions
        var prevPos = touches.Select(t => t.position - t.deltaPosition).ToList();

        // calculate current center by finding the average point
        var center = avg(pos);

        // calculate previous center by finding the average point
        var prevCenter = avg(prevPos);

        // calculate the difference between the average distance to the centers for the current and previous positions
        var distDiff = avgDist(pos, center) - avgDist(prevPos, prevCenter);

        // do the zoom in or out depending on whether we "pinch out" or "pinch in" ie. whether or not the distance to the center has changed
        if (distDiff <= 0)
            // if the difference is negative, the distance from the center became larger, indicating a pinch out
            ZoomOut(Mathf.Abs(distDiff) * 0.1f);
        else
            // else the distance became smaller, indicating a pinch in
            ZoomIn(Mathf.Abs(distDiff) * 0.1f);

        // calculate the distance between the centers, applying a smoothing factor
        var distCenter = (center - prevCenter) * 0.5f;

        // rotate based on the delta vector between centers
        RotateUp(-distCenter.y);
        RotateRight(distCenter.x);
    }
}