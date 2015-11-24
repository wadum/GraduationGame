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

    private BoxCollider _boxCollider;
    private BoxCollider BoxCol { get { return _boxCollider ?? (_boxCollider = GetComponent<BoxCollider>()); } }

    private Bounds? _trueBounds;
    private Bounds TrueBounds { get { return _trueBounds ?? (_trueBounds = GetTrueBounds()).Value; } }

    private Vector3 Center { get { return TrueBounds.center; } }
    private Vector3 Forward { get { return transform.forward*TrueBounds.extents.z; } }
    private Vector3 Backward { get { return -Forward; } }
    private Vector3 Right { get { return transform.right*TrueBounds.extents.x; } }
    private Vector3 Left { get { return -Right; } }

    private Bounds GetTrueBounds() {
        // Counteract unity stupidity.
	    var currentRotation = transform.rotation;
	    transform.rotation = Quaternion.identity;

	    var bounds = BoxCol.bounds;

        transform.rotation = currentRotation;

        return bounds;
    }

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

	    _boxCollider = GetComponent<BoxCollider>();
	    _trueBounds = GetTrueBounds();
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag != "Player" || !_enter)
            return;

        _enter.AssumeDirectControl();
    }

    void OnTriggerExit(Collider other) {
        if (other.tag != "Player")
            return;

        var relativePosition = other.transform.position - TrueBounds.center;
        var alignForward = Quaternion.FromToRotation(transform.forward, Vector3.forward);
        var alignedDirection = alignForward*relativePosition;
        
        if (alignedDirection.z >= TrueBounds.extents.z) {
            if (_forwardExit)
                _forwardExit.AssumeDirectControl();
            return;
        }

        if (alignedDirection.z <= -TrueBounds.extents.z) {
            if (_backwardExit)
                _backwardExit.AssumeDirectControl();
            return;
        }

        if (alignedDirection.x >= TrueBounds.extents.x) {
            if (_rightExit)
                _rightExit.AssumeDirectControl();
            return;
        }

        if (alignedDirection.x <= TrueBounds.extents.x) {
            if (_leftExit)
                _leftExit.AssumeDirectControl();
            return;
        }
    }

    void OnDrawGizmosSelected() {
        _trueBounds = GetTrueBounds();

        if (Enter) {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(Center, Enter.transform.position);
        }

        if (ForwardExit) {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Forward + Center, ForwardExit.transform.position);
        }

        if (BackwardExit) {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(Backward + Center, BackwardExit.transform.position);
        }

        if (RightExit) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Right + Center, RightExit.transform.position);
        }

        if (LeftExit) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Left + Center, LeftExit.transform.position);
        }
    }
}