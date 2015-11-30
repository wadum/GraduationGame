using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class WindmillRotator : MonoBehaviour {

    public float RotationSpeed = 2;
    public GameObject RotateObject;
    Electrified[] On;
    public List<GameObject> ObjectsToRotate;
    public GameObject CubeIcon;
	
	void Awake () {
        On = transform.root.GetComponentsInChildren<Electrified>();
	}
	Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }
	void Update () {
        if (On.Any(b => b.Active))
        {

            if (ObjectsToRotate.Count > 0)
        {
                foreach (GameObject obj in ObjectsToRotate)
                {
                    if(obj != CubeIcon)
                        obj.transform.position = RotatePointAroundPivot(obj.transform.position, RotateObject.transform.position, new Vector3(0, 0, RotationSpeed));
                    obj.transform.Rotate(new Vector3(0, 0, RotationSpeed));
                }
//                    obj.transform.Rotate()RotateAround(RotateObject.transform.position, RotationSpeed);
            return;
        }

//        if (On.Any(b => b.Active))
  //      {
            RotateObject.transform.Rotate(new Vector3(0, 0, RotationSpeed));
        }
    }
}
