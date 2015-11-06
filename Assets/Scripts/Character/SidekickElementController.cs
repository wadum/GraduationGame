using UnityEngine;
using System.Collections;

public class SidekickElementController : MonoBehaviour {


    public bool SwirlingAroundPlayer;
    public float OriginalRotationSpeed;
    public float EndRotationSpeed;
    public float SwirlAroundPlayerDistance;
    public GameObject parent;

    float _rotationSpeed;
    float _time;
    float _animationTimer = 2;
    float _swirlDistance;
    GameObject _player;
    Vector3 _endPos;
    Vector3 _originalPos;
    Vector3 _rotationPos;
    Vector3 _startpos;
    Vector3 _SAOPos;
    Quaternion _originalRot;

    public enum Status { SwirlingAroundPlayer, SwirlingAroundObject, Forming, InBody, FlyOut, FlyBack };
    public Status _myStatus;
    // Use this for initialization

    void Start () {

        _myStatus = Status.SwirlingAroundPlayer;

        _player = GameObject.FindGameObjectWithTag("Player");

        _originalPos = transform.localPosition;
        _rotationPos = transform.localPosition;
        _originalRot = transform.localRotation;
        _rotationSpeed = OriginalRotationSpeed / 4;

        SwirlingAroundPlayer = true;

        MultiTouch.RegisterTapHandlerByTag("TimeManipulationObject", hit =>
        {
            var parentTransform = hit.transform.root;
            if (parentTransform.GetComponent<ObjectTimeController>())
            {
                FlyToObject(parentTransform.GetComponent<HelperSwirlAroundLocation>().SwirlAroundLocation.transform.position, parentTransform.GetComponent<HelperSwirlAroundLocation>().SwirlDistance);
            }
        });
        MultiTouch.RegisterTapHandlerByTag("Terrain", hit =>
        {
            if (_myStatus != Status.SwirlingAroundPlayer && _myStatus != Status.FlyBack)
            {
                _myStatus = Status.FlyBack;
                _time = 0;
                _startpos = transform.position;
                _animationTimer = 1;
            }

            if (_myStatus == Status.SwirlingAroundObject)
            {
                _myStatus = Status.FlyBack;
                _time = 0;
                _startpos = transform.position;
                _animationTimer = 2;
            }
        });
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(_myStatus);
        // if we are to formUp and ou current status is swirling around the player then start forming up.
        if (!SwirlingAroundPlayer && _myStatus == Status.SwirlingAroundPlayer)
        {
            _myStatus = Status.Forming;
        }

        // Switch case that determines the state of the Helper.
        switch (_myStatus)
        {
            case Status.SwirlingAroundPlayer:
                GetComponent<RandomRotation>().enabled = true;

                RotateAroundPos(_player.transform.position);

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
                _time += Time.deltaTime;
                transform.position = Vector3.Lerp(_startpos, _endPos, _time / _animationTimer);
                if(Vector3.Distance(transform.position, _endPos) < _swirlDistance)
                {
                    _myStatus = Status.SwirlingAroundObject;
                }

                break;

            case Status.SwirlingAroundObject:
                GetComponent<RandomRotation>().enabled = true;
                _time = 0;
                _startpos = transform.position;
                RotateAroundPos(_endPos);
                break;

            case Status.FlyBack:
                _time += Time.deltaTime;
                transform.position = Vector3.Lerp(_startpos, _originalPos + parent.transform.localPosition + _player.transform.position, _time / _animationTimer);
                if (Vector3.Distance(transform.position, _originalPos + parent.transform.localPosition + _player.transform.position) < 0.1)
                {
                    SwirlingAroundPlayer = true;
                    _myStatus = Status.SwirlingAroundPlayer;
                    this.transform.parent = parent.transform;
                    _rotationSpeed = OriginalRotationSpeed;
                }
                break;

            default:
           
                break;
        }
    }

    private void RotateAroundPos(Vector3 Pos)
    {
        transform.RotateAround(Pos, Vector3.up, _rotationSpeed * Time.deltaTime);

        if (_rotationSpeed > EndRotationSpeed)
        {
            _rotationSpeed = _rotationSpeed - Time.deltaTime;
        }

        if (Vector3.Distance(transform.position, new Vector3(transform.position.x, Pos.y, transform.position.z)) > 0.2)
        {
            if (transform.position.y < Pos.y)
            {
                transform.position = transform.position + Vector3.up * Time.deltaTime;
            }
            else
            {
                transform.position = transform.position + Vector3.down * Time.deltaTime;
            }
        }
    }

    void FlyToObject(Vector3 EndPos, float SwirlDistance)
    {
        this.transform.parent = null;
        SwirlingAroundPlayer = false;
        _startpos = transform.position;
        _endPos = EndPos;
        _swirlDistance = SwirlDistance;
        _myStatus = Status.FlyOut;
        _time = 0;
    }
}
