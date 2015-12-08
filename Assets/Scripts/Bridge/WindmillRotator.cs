using UnityEngine;
using System.Linq;
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

    private static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    private static Vector3 RotateObjectAroundObject(GameObject rotatee, GameObject rotator, Vector3 rotation) {
        return RotatePointAroundPivot(rotatee.transform.position, rotator.transform.position, rotation);
    }

	void Update () {
	    if (!On.Any(b => b.Active))
            return;

        // Making the rotation framerate independant by adding Time.deltaTime means all calibrations in the levels are off.
        // TODO: This constant should be removed, but it requires someone to go through and correct all the rotators in all the levels.
	    const float legacyAdjustment = 40f;
        var rotation = new Vector3(0, 0, RotationSpeed * Time.deltaTime * legacyAdjustment);

	    if (ObjectsToRotate.Count > 0) {
	        foreach (var obj in ObjectsToRotate) {
	            if(obj != CubeIcon)
	                obj.transform.position = RotateObjectAroundObject(obj, RotateObject, rotation);

	            obj.transform.Rotate(rotation);
	        }

	        return;
	    }

	    RotateObject.transform.Rotate(rotation);
	}
}