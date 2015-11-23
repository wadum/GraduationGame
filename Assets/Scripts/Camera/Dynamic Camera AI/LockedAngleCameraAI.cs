using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LockedAngleCameraAI : BaseDynamicCameraAI {
    public GameObject Target;

    [Header("Positioning")]
    public bool Relative = false;
    [Range(0, 360)] public float Yaw;
    [Range(-90, 90)] public float Pitch;
    [Range(0, 100)] public float Distance;

    [Header("Smoothing")]
    public bool EnableSmoothing = true;
    public AnimationCurve MovementCurve;
    public float UnitTimeToTravel = 1f;

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
        while (true) {
            // lol soooooo smooooooooth. Not.
            DynCam.transform.LookAt(Target.transform);
            yield return null;
        }
    }

    private IEnumerator Movement() {
        while (true) {
            DynCam.SetPosition(Yaw, Pitch, Distance, Relative);

            yield return null;
        }
    }

    private IEnumerator SmoothMovement() {
        var currentPosition = DynCam.transform.position;
        var desiredPosition = DynCam.GetPosition(Yaw, Pitch, Distance, Relative);
        var timeToTravel = Vector3.Distance(currentPosition, desiredPosition)*UnitTimeToTravel;

        var midpoint = (currentPosition + desiredPosition)/2;
        var relStart = currentPosition - midpoint;
        var relEnd = desiredPosition - midpoint;

        var rotateAroundMidpoint = Quaternion.FromToRotation(relStart, relEnd);

        var startTime = Time.time;
        Func<float, float> timePos = time => (time - startTime)/timeToTravel;
        Func<float, Vector3> smooth = ratio => Quaternion.Lerp(Quaternion.identity, rotateAroundMidpoint, ratio) * relStart + midpoint;

        var pos = 0f;
        while (pos < 1) {
            pos = timePos(Time.time);
            DynCam.transform.position = smooth(MovementCurve.Evaluate(pos));

            yield return null;
        }

        yield return DynCam.StartCoroutine(Movement());
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