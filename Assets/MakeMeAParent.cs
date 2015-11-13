using UnityEngine;
using System.Collections;

public class MakeMeAParent : MonoBehaviour {

    public GameObject objh;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        objh.transform.parent = transform;
        Destroy(GetComponent<MakeMeAParent>());
    }
}
