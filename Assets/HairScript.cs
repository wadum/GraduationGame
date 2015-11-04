using UnityEngine;
using System.Collections;

public class HairScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody>().AddForce(-GetComponentInParent<NavMeshAgent>().velocity*10 + Vector3.up*3);
	}
}
