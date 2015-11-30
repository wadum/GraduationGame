using UnityEngine;
using System.Collections;

public class Level99CameraRotator : MonoBehaviour {

    public float Speed = 1;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, Speed * Time.deltaTime, 0));
	}
}
