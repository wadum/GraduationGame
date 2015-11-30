using UnityEngine;
using System.Collections;

public class Level99Input : MonoBehaviour {

    public Camera Cam;

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
            DoRaycast(Input.mousePosition);
    }

    void DoRaycast(Vector2 pos)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos, Cam.transform.forward, out hit, 1000))
        {
            if(hit.transform.root.tag == "Wind")
            {
                Destroy(hit.transform.root.gameObject);
            }
        }
    }
}
