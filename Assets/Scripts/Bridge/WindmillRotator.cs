using UnityEngine;
using System.Linq;
using System.Collections;

public class WindmillRotator : MonoBehaviour {

    public float RotationSpeed = 2;
    public GameObject ImActive;
    ElectrifiedBridge[] On;
	
	void Start () {
        On = GetComponentsInChildren<ElectrifiedBridge>();
	}
	
	void Update () {
        if (On.Any(b => b.Active))
        {
            transform.Rotate(new Vector3(0, 0, RotationSpeed));
            ImActive.gameObject.SetActive(true);
        }
        else
        {
            ImActive.gameObject.SetActive(false);
        }
    }
}
