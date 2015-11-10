using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCamera : MonoBehaviour {


    [Header("Position and Rotation")]
    public float NeutralDistanceFromPlayer = 5f;
    [Range(-89, 89)] public float NeutralPitch = 20f;
    [Range(-180, 180)] public float NeutralYaw;

    [Header("Field of View")]
    [Range(10, 170)] public float NeutralFieldOfView = 60;
    [Range(10, 170)] public float MinimumFieldOfView = 20;
    [Range(10, 170)] public float MaximumFieldOfView = 100;
    public float ZoomFovDegreesPerPixelPinch = 100;

    private Camera _camera;
    private Transform _player;
    private Collider _playerCollider;

    // Used to override the camera agent
    private bool _playerIntent;

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
        MultiTouch.RegisterTapHandlerByTag("Terrain", _ => _playerIntent = false);
	}

    void LateUpdate() {
        if (_playerIntent)
            ConstrainedPlayerControl();
        else
            FullyAutomaticCameraControl();
    }

    private void ConstrainedPlayerControl() {
        //transform.position = -transform.forward*NeutralDistanceFromPlayer + PlayerTop;
    }

    private void FullyAutomaticCameraControl() {
        var rot = Quaternion.RotateTowards(CurrentRelativeRotation, NeutralRotation, 1f);

        Debug.Log(rot.eulerAngles);

        SetRelativePosition(rot, NeutralDistanceFromPlayer);

        transform.LookAt(_player);
    }

    private void HandleSwipe(Touch swipe) {
        _playerIntent = true;

        var movementDirection = ScreenNormalize(swipe.deltaPosition);
    }

    private void HandlePinch(List<Touch> pinch) {
        _playerIntent = true;

        Func<List<Vector2>, Vector2> averagePoint = vectors => vectors.Aggregate((v1, v2) => v1 + v2) / vectors.Count;
        Func<List<Vector2>, Vector2, float> averageDistanceToPoint = (vectors, point) => vectors.Select(p => Vector2.Distance(p, point)).Average();

        var positions = pinch.Select(t => ScreenNormalize(t.position)).ToList();
        var center = averagePoint(positions);
        var averageDistanceToCenter = averageDistanceToPoint(positions, center);

        var previousPositions = pinch.Select(t => ScreenNormalize(t.position - t.deltaPosition)).ToList();
        var previousCenter = averagePoint(previousPositions);
        var previousAverageDistanceToCenter = averageDistanceToPoint(previousPositions, previousCenter);
    }

    // Transforms v -> ([0, 1], [0, 1]) given v is in screen coordinates
    private static Vector2 ScreenNormalize(Vector2 v) {
        return new Vector2(v.x / Screen.width, v.y / Screen.height);
    }

    #region Rotational Helpers
    // Helper properties
    private Vector3 RelativeYawAxis { get { return -_player.up; } }

    private Vector3 RelativePitchAxis { get { return _player.right; } }

    private Vector3 RelativePosition { get { return transform.position - PlayerTop; } }

    private Quaternion NeutralRotation { get { return RelativeRotation(NeutralYaw, NeutralPitch); } }

    private float CurrentDistance { get { return RelativePosition.magnitude; } }

    private Quaternion CurrentRelativeRotation { get { return Quaternion.FromToRotation(-_player.forward, RelativePosition.normalized); } }

    private float CurrentAbsoluteYaw {
        get {
            var yawComponent = Vector3.ProjectOnPlane(RelativePosition.normalized, Vector3.up);
            var angle = Vector3.Angle(yawComponent, Vector3.forward);

            return yawComponent.x > 0 ? angle : -angle;
        }
    }

    private Vector3 PlayerTop {
        get {
            return _playerCollider?
                _player.position + new Vector3(0, _playerCollider.bounds.extents.y, 0):
                _player.position;
        }
    }

    // Helper methods
    private Quaternion RelativeRotation(float yaw, float pitch) {
        var yawRot = Quaternion.AngleAxis(yaw, RelativeYawAxis);
        var pitchRot = Quaternion.AngleAxis(pitch, yawRot*RelativePitchAxis);

        return pitchRot*yawRot;
    }

    private void SetRelativePosition(float yaw, float pitch, float distance) {
        SetRelativePosition(RelativeRotation(yaw, pitch), distance);
    }

    private void SetRelativePosition(Quaternion rotation, float distance) {
        transform.position = rotation * (-_player.forward * distance) + PlayerTop;
    }
    #endregion
}