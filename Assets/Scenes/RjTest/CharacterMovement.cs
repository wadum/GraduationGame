using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

    public GameObject Terrain;

    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonUp(0) == true)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
<<<<<<< HEAD
                NavMeshHit navHit;
=======
                if (hit.collider.tag != "Terrain")
                {
                    return;
                }

                /*NavMeshHit navHit;
>>>>>>> Sound
                if (agent.Raycast(hit.point, out navHit)){
                    agent.SetDestination(navHit.position);
                }
                else
<<<<<<< HEAD
                {
                    agent.SetDestination(hit.point);
                }
=======
                {*/
                agent.SetDestination(hit.point);
                //}
>>>>>>> Sound
            }
        }
	}
}
