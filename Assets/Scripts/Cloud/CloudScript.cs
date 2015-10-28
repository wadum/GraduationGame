using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour {
    public GameObject wind;
    public Vector3 WindDirection;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnWind", 0.5f, 0.5f);
    }

  
    void SpawnWind()
    {
        GameObject la = (GameObject)Instantiate(wind, transform.position, Quaternion.identity);
        la.GetComponent<ObjectMoveScript>().SetDirection(WindDirection);
    }
}
