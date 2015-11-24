using UnityEngine;
using System.Collections;

public class SidekickElementController : MonoBehaviour
{


    public bool SwirlingAroundPlayer;
    public float OriginalRotationSpeed;
    public float EndRotationSpeed;
    public float SwirlAroundPlayerDistance = 0.6f;
    public GameObject parent;

    float _rotationSpeed;
    float _time;
    float _animationTimer = 2;
    float _swirlDistance;
    GameObject _player;
    Transform _endPos;
    Vector3 _originalPos;
    Vector3 _startpos;
    Vector3 _SAOPos;
    Quaternion _originalRot;

    public enum Status { SwirlingAroundPlayer, SwirlingAroundObject, Forming, InBody, FlyOut, FlyBack };
    public Status _myStatus;
    // Use this for initialization

    void Start()
    {
        _myStatus = Status.SwirlingAroundPlayer;

        _player = GameObject.FindGameObjectWithTag("Player");

        _originalPos = transform.localPosition;
        _originalRot = transform.localRotation;
        _rotationSpeed = OriginalRotationSpeed / 4;

        SwirlingAroundPlayer = true;
        MultiTouch.RegisterTapAndHoldHandlerByTag("TimeManipulationObject", hit =>
        {
            var parentTransform = hit.transform;
            ObjectTimeController timecontroller = parentTransform.GetComponentInParent<ObjectTimeController>();
            if (timecontroller)
            {
                if (timecontroller.InRange)
                    FlyToObject(parentTransform.GetComponentInParent<HelperSwirlAroundLocation>().SwirlAroundLocation.transform, parentTransform.GetComponentInParent<HelperSwirlAroundLocation>().SwirlDistance);
            }
			return true;
        });
        MultiTouch.RegisterTapAndHoldHandlerByTag("Rock", hit =>
        {
            var parentTransform = hit.transform;
            ObjectTimeController timecontroller = parentTransform.GetComponentInParent<ObjectTimeController>();
            if (timecontroller)
            {
                if (timecontroller.InRange)
                    FlyToObject(parentTransform.GetComponentInParent<HelperSwirlAroundLocation>().SwirlAroundLocation.transform, parentTransform.GetComponentInParent<HelperSwirlAroundLocation>().SwirlDistance);
            }
			return true;
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
			return true;
        });
    }

    // Update is called once per frame
    void Update()
    {
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
                _swirlDistance = SwirlAroundPlayerDistance;
                RotateAroundPos(_player.transform.position + Vector3.up);

                break;

            case Status.Forming:
                GetComponent<RandomRotation>().enabled = false;
                transform.localPosition = _originalPos;
                _myStatus = Status.InBody;
                break;

            case Status.InBody:
                transform.localPosition = _originalPos;
                transform.rotation = _originalRot;
                _rotationSpeed = OriginalRotationSpeed;
                break;

            case Status.FlyOut:
                _time += Time.deltaTime;
                transform.position = Vector3.Lerp(_startpos, _endPos.position, _time / _animationTimer);
                if (Vector3.Distance(transform.position, _endPos.position) < _swirlDistance)
                {
                    _myStatus = Status.SwirlingAroundObject;
                }

                break;

            case Status.SwirlingAroundObject:
                GetComponent<RandomRotation>().enabled = true;
                _time = 0;
                _startpos = transform.position;
                RotateAroundPos(_endPos.position);
                break;

            case Status.FlyBack:
                _time += Time.deltaTime;
                transform.position = Vector3.Lerp(_startpos, _originalPos + parent.transform.localPosition + _player.transform.position, _time / _animationTimer);
                if (Vector3.Distance(transform.position, _originalPos + parent.transform.localPosition + _player.transform.position) < SwirlAroundPlayerDistance)
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

    private void RotateAroundPos(Vector3 targetPos)
    {
        transform.RotateAround(targetPos, Vector3.up, _rotationSpeed * Time.deltaTime);

        //Lowering he rotational speed as time goes by
        if (_rotationSpeed > EndRotationSpeed)
        {
            _rotationSpeed = _rotationSpeed - Time.deltaTime;
        }

        CorrecRotationDistance(targetPos);

        //Close in towards the y.pos of the rotation position
        if (Vector3.Distance(transform.position, new Vector3(transform.position.x, targetPos.y, transform.position.z)) > 0.2f)
        {
            if (transform.position.y < targetPos.y)
            {
                transform.position = transform.position + Vector3.up * Time.deltaTime;
            }
            else
            {
                transform.position = transform.position + Vector3.down * Time.deltaTime;
            }
        }
    }

    private void CorrecRotationDistance(Vector3 targetPos)
    {
        //If the distance to the object is too long or too short move closer or futher away
        if (Vector3.Distance(transform.position, targetPos) < _swirlDistance - 0.1f)
        {
            transform.position -= (targetPos + transform.position).normalized * Time.deltaTime;
        }
        if (Vector3.Distance(transform.position, targetPos) > _swirlDistance + 0.1f)
        {
            transform.position += (targetPos - transform.position).normalized * Time.deltaTime;
        }
    }

    void FlyToObject(Transform EndPos, float SwirlDistance)
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
