using UnityEngine;
using System.Linq;
using System.Collections;

public class WindmillRotator : MonoBehaviour {

    public float RotationSpeed = 2;
    public GameObject RotateObject;
    ElectrifiedBridge[] On;
	
	void Awake () {
        On = transform.root.GetComponentsInChildren<ElectrifiedBridge>();
	}
	
	void Update () {
        if (On.Any(b => b.Active))
        {
            RotateObject.transform.Rotate(new Vector3(0, 0, RotationSpeed));
        }
    }
}
