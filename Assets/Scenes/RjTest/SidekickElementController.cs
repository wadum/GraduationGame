using UnityEngine;
using System.Collections;

public class SidekickElementController : MonoBehaviour {


    public bool Detach;
    public float OriginalRotationSpeed;
    public float EndRotationSpeed;
    //[Range(0.1f, 0.5f)]
    //public float CircleWidth = 0.3f;

    float _rotationSpeed;
    GameObject _player;
    Vector3 _originalPos;
    Vector3 _rotationPos;
    Quaternion _originaRot;

    // Use this for initialization
    void Start () {

        _player = GameObject.FindGameObjectWithTag("Player");

        _originalPos = transform.position;
        _rotationPos = transform.position;
        _originaRot = transform.localRotation;
        _rotationSpeed = OriginalRotationSpeed;

      

        Detach = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (!Detach)
        {
            transform.position = _originalPos + _player.transform.position;
            transform.rotation = _originaRot;
            _rotationSpeed = OriginalRotationSpeed;
        }

        if (Detach)
        {
            transform.RotateAround(_player.transform.position, Vector3.up, _rotationSpeed * Time.deltaTime);

            if (_rotationSpeed > EndRotationSpeed)
            {
                _rotationSpeed = _rotationSpeed - Time.deltaTime;
            }

            if (Vector3.Distance(transform.position, new Vector3(transform.position.x,_player.transform.position.y,transform.position.z)) > 0.35) 
            {
                if (transform.position.y < _player.transform.position.y)
                {
                    transform.position = transform.position + Vector3.up * Time.deltaTime;
                } else
                {
                    transform.position = transform.position + Vector3.down * Time.deltaTime;
                }
            }
        }
        // set to true to get them to rotate change when we need to animate helper
        Detach = true;
    }
}
