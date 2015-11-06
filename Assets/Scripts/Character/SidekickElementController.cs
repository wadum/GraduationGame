using UnityEngine;
using System.Collections;

public class SidekickElementController : MonoBehaviour {


    public bool FormUp;
    public float OriginalRotationSpeed;
    public float EndRotationSpeed;

    float _rotationSpeed;
    GameObject _player;
    Vector3 _originalPos;
    Vector3 _rotationPos;
    Quaternion _originalRot;

    public enum Status { Swirling, Forming, InBody, FlyOut, FlyBack };
    public Status _myStatus;
    // Use this for initialization

    void Start () {

        _myStatus = Status.Swirling;

        _player = GameObject.FindGameObjectWithTag("Player");

        _originalPos = transform.localPosition;
        _rotationPos = transform.localPosition;
        _originalRot = transform.localRotation;
        _rotationSpeed = OriginalRotationSpeed / 4;

        FormUp = false;
    }
	
	// Update is called once per frame
	void Update () {

        // if we are to formUp and ou current status is swirling around the player then start forming up.
        if (FormUp && _myStatus == Status.Swirling)
        {
            _myStatus = Status.Forming;

        }

        // If we are formed up and dont wan to be any more then start swirling.
        if (!FormUp)
        {
            _myStatus = Status.Swirling;

        }

        // Switch case that determines the state of the Helper.
        switch (_myStatus)
        {
            case Status.Swirling:
                GetComponent<RandomRotation>().enabled = true;
                transform.RotateAround(_player.transform.position, Vector3.up, _rotationSpeed * Time.deltaTime);

                if (_rotationSpeed > EndRotationSpeed)
                {
                    _rotationSpeed = _rotationSpeed - Time.deltaTime;
                }

                if (Vector3.Distance(transform.position, new Vector3(transform.position.x, _player.transform.position.y, transform.position.z)) > 0.2)
                {
                    if (transform.position.y < _player.transform.position.y)
                    {
                        transform.position = transform.position + Vector3.up * Time.deltaTime;
                    }
                    else
                    {
                        transform.position = transform.position + Vector3.down * Time.deltaTime;
                    }
                }
                break;

            case Status.Forming:
                GetComponent<RandomRotation>().enabled = false;
                transform.localPosition  = _originalPos;
                _myStatus = Status.InBody;
                break;

            case Status.InBody:
                transform.localPosition = _originalPos;
                transform.rotation = _originalRot;
                _rotationSpeed = OriginalRotationSpeed;
                break;

            case Status.FlyOut:
        
                break;

            case Status.FlyBack:
           
                break;

            default:
           
                break;
        }
    }
}
