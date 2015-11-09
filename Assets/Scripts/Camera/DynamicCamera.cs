using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCamera : MonoBehaviour {


    [Header("Position and Rotation")]
    public float NeutralDistanceFromPlayer = 5f;
    [Range(-180, 180)] public float NeutralPitch = 20f;
    [Range(-180, 180)] public float NeutralYaw = 0f;

    [Header("Field of View")]
    [Range(10, 170)] public float NeutralFieldOfView = 60;
    [Range(10, 170)] public float MinimumFieldOfView = 20;
    [Range(10, 170)] public float MaximumFieldOfView = 100;
    public float ZoomFovDegreesPerPixelPinch = 100;

    private Camera _camera;
    private Transform _player;
    private Collider _playerCollider;

    // Used to override the camera agent
    private bool _playerZoomIntent;
    private bool _playerRotateIntent;

    // Used for smoothing
    private float _desiredDistance;
    private float _desiredZoom;
    private float _desiredPitch;
    private float _desiredYaw;

    // Helper properties
    private Quaternion RelativeRotation { get { return Quaternion.Inverse(transform.rotation) * _player.rotation; } }
    private float CurrentYaw {
        get { return RelativeRotation.eulerAngles.y; }
        set {
            var distance = CurrentDistance;

            transform.position = _player.position;
            transform.rotation = GetRelativeRotation(value, CurrentPitch);

            CurrentDistance = distance;
        }
    }

    private float CurrentPitch {
        get { return RelativeRotation.eulerAngles.x; }
        set {
            var distance = CurrentDistance;

            transform.position = _player.position;
            transform.rotation = GetRelativeRotation(CurrentYaw, value);

            CurrentDistance = distance;
        }
    }

    private Quaternion GetRelativeRotation(float yaw, float pitch) {
        var inverse = Quaternion.Inverse(_player.rotation);

        var yawAxis = inverse * Vector3.up;
        var yawRot = Quaternion.AngleAxis(yaw, yawAxis) * _player.rotation;

        inverse = Quaternion.Inverse(yawRot);
        var pitchAxis = inverse * Vector3.right;
        var pitchRot = Quaternion.AngleAxis(pitch, pitchAxis) * yawRot;

        return pitchRot;
    }

    private float CurrentZoom {
        get { return _camera.fieldOfView; }
        set { _camera.fieldOfView = Mathf.Clamp(value, MinimumFieldOfView, MaximumFieldOfView); }
    }

    private float CurrentDistance {
        get { return Vector3.Distance(transform.position, PlayerTop); }
        set { transform.position = (transform.position - PlayerTop).normalized * value + PlayerTop; }
    }

    private Vector3 PlayerTop {
        get {
            return _playerCollider?
                _player.position + new Vector3(0, _playerCollider.bounds.extents.y, 0):
                _player.position;
        }
    }

    private bool Sanity() {
        _camera = GetComponent<Camera>();

        var players = GameObject.FindGameObjectsWithTag("Player");
        if (!players.Any()) {
            Debug.Log("No GameObject tagged Player found.");
            return false;
        }

        if (players.Length >= 2) {
            Debug.Log("More than one GameObject tagged player: " + string.Join(", ", players.Select(p => p.name).ToArray()));
            return false;
        }

        _player = players.First().transform;

        _playerCollider = _player.GetComponent<Collider>();
        if (!_playerCollider) {
            Debug.Log("Warn: player missing collider, camera offset might be wrong");
        }

        return true;
    }

    void Start () {
	    if (!Sanity()) {
            Debug.Log("DynamicCamera disabled due to sanity check failing.");
	        enabled = false;
	        return;
	    }

        MultiTouch.RegisterSwipeHandler(HandleSwipe);
	    MultiTouch.RegisterPinchHandler(HandlePinch);
        MultiTouch.RegisterTapHandlerByTag("Terrain", _ => _playerRotateIntent = _playerZoomIntent = false);
	}

    private void ResetToNeutral() {
        CurrentDistance = NeutralDistanceFromPlayer;
        CurrentYaw = NeutralYaw;
        CurrentPitch = NeutralPitch;

        transform.LookAt(PlayerTop);
    }

    void LateUpdate() {
        ResetToNeutral();

        if (Input.GetKeyDown(KeyCode.D)) {
            Debug.Log("Current Yaw: " + CurrentYaw);
            Debug.Log("Current Pitch: " + CurrentPitch);
        }
    }

    private void HandleSwipe(Touch swipe) {
        _playerRotateIntent = true;

        var movementDirection = ScreenNormalize(swipe.deltaPosition);
    }

    private void HandlePinch(List<Touch> pinch) {
        _playerZoomIntent = true;

        Func<List<Vector2>, Vector2> averagePoint = vectors => vectors.Aggregate((v1, v2) => v1 + v2) / vectors.Count;
        Func<List<Vector2>, Vector2, float> averageDistanceToPoint = (vectors, point) => vectors.Select(p => Vector2.Distance(p, point)).Average();

        var positions = pinch.Select(t => ScreenNormalize(t.position)).ToList();
        var center = averagePoint(positions);
        var averageDistanceToCenter = averageDistanceToPoint(positions, center);

        var previousPositions = pinch.Select(t => ScreenNormalize(t.position - t.deltaPosition)).ToList();
        var previousCenter = averagePoint(previousPositions);
        var previousAverageDistanceToCenter = averageDistanceToPoint(previousPositions, previousCenter);

        CurrentZoom += 2 * ZoomFovDegreesPerPixelPinch * (previousAverageDistanceToCenter - averageDistanceToCenter);
    }

    // Transforms v -> ([0, 1], [0, 1]) given v is in screen coordinates
    private static Vector2 ScreenNormalize(Vector2 v) {
        return new Vector2(v.x / Screen.width, v.y / Screen.height);
    }
}