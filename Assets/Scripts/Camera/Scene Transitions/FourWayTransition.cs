using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FourWayTransition : MonoBehaviour {

    public GameObject Enter;
    public GameObject ForwardExit;
    public GameObject BackwardExit;
    public GameObject RightExit;
    public GameObject LeftExit;

    private BaseDynamicCameraAI _enter;
    private BaseDynamicCameraAI _forwardExit;
    private BaseDynamicCameraAI _backwardExit;
    private BaseDynamicCameraAI _rightExit;
    private BaseDynamicCameraAI _leftExit;

	void Start () {
	    if (Enter) {
    	    _enter = Enter.GetComponent<BaseDynamicCameraAI>();
            if (!_enter)
                Debug.LogError("Enter gameobject set, but target gameobject does not have a dynamic camera ai.");
	    }
	    if (ForwardExit) {
    	    _forwardExit = ForwardExit.GetComponent<BaseDynamicCameraAI>();
            if (!_forwardExit)
                Debug.LogError("Forward exit gameobject set, but target gameobject does not have a dynamic camera ai.");
	    }
	    if (BackwardExit) {
    	    _backwardExit = BackwardExit.GetComponent<BaseDynamicCameraAI>();
            if (!_backwardExit)
                Debug.LogError("Backward exit gameobject set, but target gameobject does not have a dynamic camera ai.");
	    }
	    if (RightExit) {
    	    _rightExit = RightExit.GetComponent<BaseDynamicCameraAI>();
            if (!_rightExit)
                Debug.LogError("Right exit gameobject set, but target gameobject does not have a dynamic camera ai.");
	    }
	    if (LeftExit) {
    	    _leftExit = LeftExit.GetComponent<BaseDynamicCameraAI>();
            if (!_leftExit)
                Debug.LogError("Left exit gameobject set, but target gameobject does not have a dynamic camera ai.");
	    }
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag != "Player" || !_enter)
            return;

        _enter.AssumeDirectControl();
    }

    void OnTriggerExit(Collider other) {
        if (other.tag != "Player")
            return;

        var relativePosition = other.transform.position - transform.position;
        var projection = Vector3.ProjectOnPlane(relativePosition, transform.up);
        var angle = Vector3.Angle(transform.forward, projection);
        angle = Vector3.Dot(Vector3.Cross(transform.forward, projection), transform.up) >= 0? angle: -angle;
        angle += 180;

        if (angle >= 135 && angle < 225 && _forwardExit) {
            _forwardExit.AssumeDirectControl();
            return;
        }

        if (angle >= 225 && angle < 315 && _rightExit) {
            _rightExit.AssumeDirectControl();
            return;
        }

        if (angle >= 315 || angle < 45 && _backwardExit) {
            _backwardExit.AssumeDirectControl();
            return;
        }

        if (angle >= 45 && angle < 135 && _leftExit) {
            _leftExit.AssumeDirectControl();
            return;
        }

        Debug.Log("Should not happen: " + angle);
    }
}