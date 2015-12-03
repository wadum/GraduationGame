using UnityEngine;
using System.Collections;
using System.Linq;

public class Level99CameraRotator : MonoBehaviour {

    public float Speed = 1;

    public GameObject[] Menus;
	
	// Update is called once per frame
	void Update () {
        if(!(Menus.Any(m => m.activeSelf)))
            transform.Rotate(new Vector3(0, Speed * Time.deltaTime, 0));
	}
}
