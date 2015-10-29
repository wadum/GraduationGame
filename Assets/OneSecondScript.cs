using UnityEngine;
using System.Collections;

public class OneSecondScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        collider.enabled = false;
//        Destroy(this.gameObject);
    }
}
