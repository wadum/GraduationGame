using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCamera : MonoBehaviour {
    [Header("Position and Rotation")]
    public float NeutralDistance = 5f;
    [Range(-89, 89)] public float NeutralPitch = 20f;
    [Range(-180, 180)] public float NeutralYaw = 0f;

    [Header("Field of View")]
    [Range(10, 170)] public float NeutralFieldOfView = 60;
    [Range(10, 170)] public float MinimumFieldOfView = 20;
    [Range(10, 170)] public float MaximumFieldOfView = 100;

    [Header("Minimum and Maximum Values")]
    public float MinimumDistance = 5f;
    public float MaximumDistance = 10f;
    [Range(-90, 90)] public float MinimumPitch = -20f;
    [Range(-90, 90)] public float MaximumPitch = 80f;
    [Range(-180, 180)] public float MinimumYaw = -180;
    [Range(-90, 90)] public float MaximumYaw = 180;

    [Header("Player Controls")]
    public float PitchByHeight = 90f;
    public float YawByWidth = 360f;
    public float ZoomFactor = 90f;
    
    private Camera _camera;
    private Transform _player;
    private Collider _playerCollider;

    // Used to override the camera agent
    private bool _playerIntent;
    private float _playerDesiredAbsoluteYaw;
    private float _playerDesiredPitch;
    private float _playerDesiredZoom;

    #region Helper properties

    private static Vector3 AbsoluteYawAxis { get { return -Vector3.up; } }

    private static Vector3 AbsoluteDirection { get { return -Vector3.forward; } }

    private static Vector3 BaseAbsolutePitchAxis { get { return Vector3.right; } }

    private Vector3 RelativeYawAxis { get { return -_player.up; } }

    private Vector3 RelativeDirection { get { return -_player.forward; } }

    private Vector3 BaseRelativePitchAxis { get { return _player.right; } }

    private Vector3 RelativePosition { get { return transform.position - PlayerTop; } }

    private Quaternion NeutralRelativeRotation { get { return Rotation(NeutralYaw, NeutralPitch); } }

    private Quaternion NeutralAbsoluteRotation { get { return Rotation(NeutralYaw, NeutralPitch, false); } }

    private float CurrentDistance { get { return RelativePosition.magnitude; } }

    private Quaternion CurrentRelativeRotation { get { return Quaternion.FromToRotation(RelativeDirection, RelativePosition); } }

    private Quaternion CurrentAbsoluteRotation { get { return Quaternion.FromToRotation(AbsoluteDirection, RelativePosition); } }

    private float CurrentAbsoluteYaw {
        get {
            var yawComponent = Vector3.ProjectOnPlane(RelativePosition, AbsoluteYawAxis);
            var angle = Vector3.Angle(yawComponent, AbsoluteDirection);

            return Vector3.Dot(Vector3.Cross(yawComponent, AbsoluteDirection), AbsoluteYawAxis) >= 0 ? -angle : angle;
        }
    }

    private float CurrentRelativeYaw {
        get {
            var yawComponent = Vector3.ProjectOnPlane(RelativePosition, RelativeYawAxis);
            var angle = Vector3.Angle(yawComponent, RelativeDirection);

            return Vector3.Dot(Vector3.Cross(yawComponent, RelativeDirection), RelativeYawAxis) >= 0 ? -angle : angle;
        }
    }

    private float CurrentPitch {
        get {
            var yawRot = CurrentRelativeYawRotation;
            var yawForward = yawRot * RelativeDirection;

            var relativePitchAxis = yawRot*BaseRelativePitchAxis;
            var pitchComponent = Vector3.ProjectOnPlane(RelativePosition, relativePitchAxis);
            
            var angle = Vector3.Angle(pitchComponent, yawForward);

            return Vector3.Dot(Vector3.Cross(pitchComponent, yawForward), relativePitchAxis) >= 0 ? -angle : angle;
        }
    }

    private Quaternion CurrentRelativeYawRotation { get { return Quaternion.AngleAxis(CurrentRelativeYaw, RelativeYawAxis); } }

    private Quaternion CurrentRelativePitchRotation { get { return Quaternion.AngleAxis(CurrentPitch, CurrentRelativeYawRotation*BaseRelativePitchAxis); } }

    private Vector3 PlayerTop {
        get {
            return _playerCollider?
                _player.position + new Vector3(0, _playerCollider.bounds.extents.y, 0):
                _player.position;
        }
    }

    #endregion

    #region Helper methods

    // Transforms v -> ([0, 1], [0, 1]) given v is in screen coordinates
    private static Vector2 ScreenNormalize(Vector2 v) {
        return new Vector2(v.x / Screen.width, v.y / Screen.height);
    }

    private Quaternion Rotation(float yaw, float pitch, bool relative = true) {
        var yawRot = Quaternion.AngleAxis(yaw, relative? RelativeYawAxis: AbsoluteYawAxis);
        var pitchRot = Quaternion.AngleAxis(pitch, yawRot * (relative? BaseRelativePitchAxis: BaseAbsolutePitchAxis));

        return pitchRot*yawRot;
    }

    private void SetPosition(Quaternion rotation, float distance, bool relative = true) {
        transform.position = rotation * (relative? RelativeDirection: AbsoluteDirection) * distance + PlayerTop;
    }

    #endregion

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

    private bool _manualOverride = false;
    void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.A))
            Debug.Log("Yaw: (" + CurrentAbsoluteYaw + ", " + CurrentRelativeYaw + "), Pitch: " + CurrentPitch);

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (_manualOverride)
                _manualOverride = false;
            else {
                _playerDesiredAbsoluteYaw = CurrentAbsoluteYaw;
                _playerDesiredPitch = CurrentPitch;
                _manualOverride = true;
            }
        }

        if (_playerIntent || _manualOverride)
            ConstrainedPlayerControl();
        else
            FullyAutomaticCameraControl();
    }

    private void ConstrainedPlayerControl() {
        var desiredRotation = Rotation(_playerDesiredAbsoluteYaw, _playerDesiredPitch, false);

        SetPosition(desiredRotation, NeutralDistance, false);

        transform.LookAt(_player);
    }

    private void FullyAutomaticCameraControl() {
        var rot = Quaternion.RotateTowards(CurrentRelativeRotation, NeutralRelativeRotation, 40f * Time.deltaTime);

        SetPosition(rot, NeutralDistance);

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
}