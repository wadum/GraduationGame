using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

    public GameObject Terrain;
    public GameObject DeliverClockPartArea;
    public GameObject[] FloatWaypoints;

    NavMeshAgent agent;


    public float waypointFloatSpeed = 1.0f;
    float startTime;
    float journeyLength;
    float distCovered;
    float fracJourney;
    int CurrentWaypoint;
    bool flyToCenterClock = false;

    Vector3 lerpStartingPos;
    Vector3 lerpEndPos;

    // Use this for initialization
    void Start () {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        CurrentWaypoint = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (flyToCenterClock)
        {
            agent.enabled = false;

            distCovered = (Time.time - startTime) * waypointFloatSpeed;
            fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(lerpStartingPos, lerpEndPos, fracJourney);

            if (fracJourney >= 1)
            {
                startTime = Time.time;
                CurrentWaypoint++;
                lerpStartingPos = FloatWaypoints[CurrentWaypoint-1].transform.position;

                if (lerpStartingPos == DeliverClockPartArea.transform.position)
                {
                    agent.enabled = true;
                    flyToCenterClock = false;
                    return;
                }

                lerpEndPos = FloatWaypoints[CurrentWaypoint].transform.position;
                journeyLength = Vector3.Distance(lerpStartingPos, lerpEndPos);
            }

        }

        if (Input.GetMouseButtonUp(0) == true)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //NavMeshHit navHit;

                /*if (hit.collider.tag != "Terrain")
                {
                    return;
                }*/

                agent.SetDestination(hit.point);

                /*if (agent.Raycast(hit.point, out navHit)){
                    agent.SetDestination(navHit.position);
                }
                else
                {
                agent.SetDestination(hit.point);
                }*/
            }
        }
    }

    public void goToCenterClock(GameObject[] Waypoints)
    {
        FloatWaypoints = Waypoints;
        startTime = Time.time;
        lerpEndPos = FloatWaypoints[CurrentWaypoint].transform.position;
        lerpStartingPos = transform.position;
        journeyLength = Vector3.Distance(lerpStartingPos, FloatWaypoints[0].transform.position);
        flyToCenterClock = true;

    }
}
