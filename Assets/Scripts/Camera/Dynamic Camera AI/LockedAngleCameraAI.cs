using System;
using System.Collections;
using UnityEngine;

public class LockedAngleCameraAI : BaseDynamicCameraAI {
    public GameObject Target;

    [Header("Positioning")]
    public bool Relative = false;
    [Range(0, 360)] public float Yaw;
    [Range(-90, 90)] public float Pitch;
    [Range(0, 100)] public float Distance;

    [Header("Smoothing")]
    public bool EnableSmoothing = true;
    public float YawMaximumDegreesPerSecond = 5f;
    public float PitchMaximumDegreesPerSecond = 3f;
    public float DistanceMaximumDeltaPerSecond = 1.25f;
    public float LookAtMaximumDegreesPerSecond = 25f;

    protected override void Begin() {
        DynCam.SetTarget(Target);

        if (EnableSmoothing) {
            DynCam.StartCoroutine(SmoothMovement());
            DynCam.StartCoroutine(SmoothLookAt());
        }
        else {
            DynCam.StartCoroutine(Movement());
            DynCam.StartCoroutine(LookAt());
        }
    }

    private IEnumerator LookAt() {
        while (true) {
            DynCam.transform.LookAt(Target.transform);
            yield return true;
        }
    }

    private IEnumerator SmoothLookAt() {
        Func<Quaternion> lookRotation = () => Quaternion.LookRotation(Target.transform.position - DynCam.transform.position);

        while (true) {
            DynCam.transform.rotation = Quaternion.Slerp(DynCam.transform.rotation, lookRotation(), Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Movement() {
        while (true) {
            DynCam.SetPosition(Yaw, Pitch, Distance, Relative);
            DynCam.transform.LookAt(Target.transform);

            yield return null;
        }
    }

    private IEnumerator SmoothMovement() {
        var distanceVelocity = 0f;
        var yawVelocity = 0f;
        var pitchVelocty = 0f;

        // These are functions as they _may_ change if the target moves while we transition.
        Func<float> currentYaw = () => Relative? DynCam.CurrentRelativeYaw: DynCam.CurrentAbsoluteYaw;
        Func<float> currentPitch = () => DynCam.CurrentPitch;
        Func<float> currentDistance = () => DynCam.CurrentDistance;
        
        Func<float> smoothYaw = () => Mathf.SmoothDampAngle(currentYaw(), Yaw, ref yawVelocity, 0.1f, YawMaximumDegreesPerSecond);
        Func<float> smoothPitch = () => Mathf.SmoothDampAngle(currentPitch(), Pitch, ref pitchVelocty, 0.1f, PitchMaximumDegreesPerSecond);
        Func<float> smoothDistance = () => Mathf.SmoothDampAngle(currentDistance(), Distance, ref distanceVelocity, 0.1f, DistanceMaximumDeltaPerSecond);

        while (true) {
            DynCam.SetPosition(smoothYaw(), smoothPitch(), smoothDistance(), Relative);
            yield return null;
        }
    }

    void OnDrawGizmosSelected() {
        if (!Target)
            return;

        Gizmos.color = Color.black;
        var position = DynCam.GetPosition(Yaw, Pitch, Distance, Relative, Target);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Target.transform.position, position);
        Gizmos.DrawSphere(position, 0.25f);
    }
}