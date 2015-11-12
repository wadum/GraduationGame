using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject DeliverClockPartArea;
    public GameObject[] FloatWaypoints;
    public float WaypointFloatSpeed = 1.0f;


    private bool LootAtMovingObject = false;
    private GameObject LookAtTarget = null;

    private NavMeshAgent _agent;
    private float _startTime,
        _journeyLength,
        _distCovered,
        _fracJourney;
    private int _currentWaypoint;
    private bool _flyToCenterClock;

    private Vector3 _lerpStartingPos,
        _lerpEndPos;

    // Use this for initialization
    void Start()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _currentWaypoint = 0;

        MultiTouch.RegisterTapHandlerByTag("Terrain", hit => { GoTo(hit.point); GameOverlayController.gameOverlayController.DeactivateSlider(); });
        MultiTouch.RegisterTapHandlerByTag("Clockpart", hit => GoTo(hit.collider.transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        if (LootAtMovingObject)
        {
            if (LookAtTarget != null)
            {
                LookHERE tmpLTarget = LookAtTarget.GetComponentInChildren<LookHERE>();
                if (tmpLTarget != null)
                {
                    Vector3 tmpActualTarget = new Vector3(tmpLTarget.transform.position.x, this.gameObject.transform.position.y, tmpLTarget.transform.position.z);
                    gameObject.transform.LookAt(tmpActualTarget);
                }
                else
                    Debug.LogWarning("LookHERE script has to be assigned to one of the children of the target gameObject: " + LookAtTarget.name);
            }
        }

        if (!DeliverClockPartArea)
        {
            Debug.Log("Add DeliverClockPartArea");
            return;
        }

        if (!_flyToCenterClock)
            return;

        _agent.enabled = false;

        _distCovered = (Time.time - _startTime) * WaypointFloatSpeed;
        _fracJourney = _distCovered / _journeyLength;
        transform.position = Vector3.Lerp(_lerpStartingPos, _lerpEndPos, _fracJourney);

        if (!(_fracJourney >= 1))
            return;

        _startTime = Time.time;
        _currentWaypoint++;
        _lerpStartingPos = FloatWaypoints[_currentWaypoint - 1].transform.position;

        if (_lerpStartingPos == DeliverClockPartArea.transform.position || _currentWaypoint == FloatWaypoints.Length)
        {
            _agent.enabled = true;
            _flyToCenterClock = false;
            return;
        }
        _lerpEndPos = FloatWaypoints[_currentWaypoint].transform.position;
        _journeyLength = Vector3.Distance(_lerpStartingPos, _lerpEndPos);
    }

    public void SetPlayerLookAts(bool value, GameObject go)
    {
        LootAtMovingObject = value;
        LookAtTarget = go;
    }

    public void GoTo(Vector3 position)
    {
        if (_agent.enabled)
        {
            GameOverlayController.gameOverlayController.DeactivateSlider();
            _agent.SetDestination(position);
        }
    }

    public void GoToCenterClock(GameObject[] waypoints)
    {
        FloatWaypoints = waypoints;
        _startTime = Time.time;
        _lerpEndPos = FloatWaypoints[_currentWaypoint].transform.position;
        _lerpStartingPos = transform.position;
        _journeyLength = Vector3.Distance(_lerpStartingPos, FloatWaypoints[0].transform.position);
        _flyToCenterClock = true;
    }
}
