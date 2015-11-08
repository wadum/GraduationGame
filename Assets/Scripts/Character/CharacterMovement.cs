using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject DeliverClockPartArea;
    public GameObject[] FloatWaypoints;

    NavMeshAgent _agent;

    public float WaypointFloatSpeed = 1.0f;
    float _startTime;
    float _journeyLength;
    float _distCovered;
    float _fracJourney;
    int _currentWaypoint;
    bool _flyToCenterClock;

    Vector3 _lerpStartingPos;
    Vector3 _lerpEndPos;

    // Use this for initialization
    void Start () {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _currentWaypoint = 0;

        MultiTouch.RegisterTapHandlerByTag("Terrain", hit => GoTo(hit.point));
        MultiTouch.RegisterTapHandlerByTag("Clockpart", hit => GoTo(hit.collider.transform.position));
    }
	
	// Update is called once per frame
	void Update () {
        if (!DeliverClockPartArea)
        {
//            Debug.Log("Add DeliverClockPartArea");
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
	    _lerpStartingPos = FloatWaypoints[_currentWaypoint-1].transform.position;

	    if (_lerpStartingPos == DeliverClockPartArea.transform.position)
	    {
	        _agent.enabled = true;
	        _flyToCenterClock = false;
	        return;
	    }

	    _lerpEndPos = FloatWaypoints[_currentWaypoint].transform.position;
	    _journeyLength = Vector3.Distance(_lerpStartingPos, _lerpEndPos);
	}

    public void GoTo(Vector3 position)
    {
        FindObjectOfType<GameOverlayController>().TimeSlider.SetActive(false);

        if (_agent.enabled)
            _agent.SetDestination(position);
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
