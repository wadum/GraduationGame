using UnityEngine;
using System.Collections;
using System;

public class TopDownCamController : MonoBehaviour {

    public Vector3 StartingPos;
    public float ZoomSpeed = 100f;
    public float RotateSpeed = 1f;
    public float UpDownRotationLimit = 80;

    private GameObject _player;
    private GameObject _cam;

    public float MaxZoomDistance = 20f;
    public float MinZoomDistance = 3f;

    private Vector3 _oldPlayerPos;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
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

	public void Run(GameObject cam)
	{
		_cam = cam;
		_oldPlayerPos = _player.transform.position;
		StartCoroutine(AlwaysLookAt(_player));
	}

	private void StayWithPlayer ()
	{
		_cam.transform.position += _player.transform.position - _oldPlayerPos;
		_oldPlayerPos = _player.transform.position;
	}

	private void ZoomOut ()
	{
		if (Vector3.Distance(_cam.transform.position, _player.transform.position) > MaxZoomDistance) return;
		_cam.transform.position += -_cam.transform.forward * ZoomSpeed;
	}

	private void ZoomIn ()
	{
		if (Vector3.Distance(_cam.transform.position, _player.transform.position) < MinZoomDistance) return;
		_cam.transform.position += _cam.transform.forward * ZoomSpeed;
	}

	private void RotateDown ()
	{
		if (_cam.transform.localRotation.eulerAngles.x < UpDownRotationLimit + 10 
		    || _cam.transform.localRotation.eulerAngles.x - RotateSpeed > 360 - UpDownRotationLimit)
			_cam.transform.RotateAround(_player.transform.position, _cam.transform.right, -RotateSpeed);
	}

	private void RotateUp ()
	{
		if (_cam.transform.localRotation.eulerAngles.x + RotateSpeed < UpDownRotationLimit 
		    || _cam.transform.localRotation.eulerAngles.x > 360 - UpDownRotationLimit - 10)
			_cam.transform.RotateAround(_player.transform.position, _cam.transform.right, RotateSpeed);
	}

	private void RotateRight ()
	{
		_cam.transform.RotateAround(_player.transform.position, _cam.transform.up, RotateSpeed);
	}

	private void RotateLeft ()
	{
		_cam.transform.RotateAround(_player.transform.position, _cam.transform.up, -RotateSpeed);
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
