using UnityEngine;

public class ObjectTimeControllerColliderOverride : MonoBehaviour {

    public GameObject TargetTimeController;

    private MasterHighlight _masterHighlight;
    private Collider _timeControllerCollider;

	// Use this for initialization
	void Start () {
	    _masterHighlight = TargetTimeController.GetComponent<MasterHighlight>();
	    if (!_masterHighlight) {
	        Debug.Log("No MasterHighlight on target of collider override.");
	        enabled = false;
	        return;
	    }

	    _timeControllerCollider = TargetTimeController.GetComponent<Collider>();
	}

    void OnTriggerEnter(Collider col) {
        if (col.tag != "Player")
            return;

        _timeControllerCollider.enabled = false;
        _masterHighlight.InRange = true;
    }

    void OnTriggerExit(Collider col) {
        if (col.tag != "Player")
            return;

        _timeControllerCollider.enabled = true;
        _masterHighlight.InRange = false;
    }
}
