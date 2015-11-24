using System;
using System.Collections;
using System.Runtime.InteropServices;
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
        DynCam.SetTarget(Target, Vector3.zero);

        if (EnableSmoothing)
            DynCam.StartCoroutine(SmoothMovement());
        else {
            DynCam.StartCoroutine(Movement());
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
        // Calculate moving into position
        var currentPosition = DynCam.transform.position;
        var desiredPosition = DynCam.GetPosition(Yaw, Pitch, Distance, Relative);
        var timeToTravel = Vector3.Distance(currentPosition, desiredPosition)*UnitTimeToTravel;

        // if the travel time is too small, we simply cut. This removes problems with near-zero distance travels
        if (timeToTravel > 0.05f) {
            // We find the pivot point by finding the closest point to the target that is equidistant from the current and desired positions.
            var midpoint = (currentPosition + desiredPosition)/2;
            var relTarget = Target.transform.position - midpoint;
            var projectionNormal = (currentPosition - midpoint).normalized;
            var relPivot = Vector3.ProjectOnPlane(relTarget, projectionNormal);
            var pivot = relPivot + midpoint;
            // make motion more or less circular depending on relative target closeness to relative pivot. Closer means more circular, further means more linear.
            pivot += relPivot.normalized*Vector3.Distance(relPivot, relTarget);

            var aroundPivot = Quaternion.FromToRotation(currentPosition - pivot, desiredPosition - pivot);
            Func<float, Vector3> smoothedMovement = ratio => Quaternion.Lerp(Quaternion.identity, aroundPivot, ratio) * (currentPosition - pivot) + pivot;

            // Calculate look at rotation
            var currentLookDirection = DynCam.transform.forward;
            var desiredLookDirection = Target.transform.position - desiredPosition;
            Func<float, Quaternion> smoothedLookAt = ratio => Quaternion.LookRotation(Vector3.Slerp(currentLookDirection, desiredLookDirection, ratio));

            var startTime = Time.time;
            Func<float, float> timePos = time => (time - startTime)/timeToTravel;

            var pos = 0f;
            while (pos < 1) {
                pos = timePos(Time.time);
                if (pos > 1)
                    pos = 1;

                var ratio = MovementCurve.Evaluate(pos);

                DynCam.transform.position = smoothedMovement(ratio);
                DynCam.transform.rotation = smoothedLookAt(ratio);

                yield return null;
            }
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