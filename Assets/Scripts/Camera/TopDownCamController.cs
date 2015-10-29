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
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>().gameObject;
    }

    void Update()
    {
        // Stay with the player
        _cam.transform.position += _player.transform.position - _oldPlayerPos;
        _oldPlayerPos = _player.transform.position;

        //ROTATION LEFT RIGHT
        if (Input.GetKey(KeyCode.D))
        {
            _cam.transform.RotateAround(_player.transform.position, _cam.transform.up, RotateSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            _cam.transform.RotateAround(_player.transform.position, _cam.transform.up, -RotateSpeed);
        }

        //ZOOMING
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        {
            if (Vector3.Distance(_cam.transform.position, _player.transform.position) > MaxZoomDistance) return;
            _cam.transform.position += -_cam.transform.forward * ZoomSpeed;
            _oldPlayerPos = _player.transform.position;
            return;
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            if (Vector3.Distance(_cam.transform.position, _player.transform.position) < MinZoomDistance) return;
            _cam.transform.position += _cam.transform.forward * ZoomSpeed;
            _oldPlayerPos = _player.transform.position;
            return;
        }

        //ROTATION UP DOWN
        if (Input.GetKey(KeyCode.W))
        {
            if (_cam.transform.localRotation.eulerAngles.x + RotateSpeed < UpDownRotationLimit || _cam.transform.localRotation.eulerAngles.x > 360 - UpDownRotationLimit - 10)
                _cam.transform.RotateAround(_player.transform.position, _cam.transform.right, RotateSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (_cam.transform.localRotation.eulerAngles.x < UpDownRotationLimit + 10 || _cam.transform.localRotation.eulerAngles.x - RotateSpeed > 360 - UpDownRotationLimit)
                _cam.transform.RotateAround(_player.transform.position, _cam.transform.right, -RotateSpeed);
        }
        
    }

    public void Run(GameObject cam)
    {
        _cam = cam;
        _oldPlayerPos = _player.transform.position;
        StartCoroutine(AlwaysLookAt(_player));
    }

    private IEnumerator AlwaysLookAt(GameObject go)
    {
        while(true)
        {
            _cam.transform.LookAt(go.transform);
            yield return null;
        }
    }
}
