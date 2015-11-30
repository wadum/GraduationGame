using System;
using UnityEngine;
using System.Collections;

public class StationaryCameraAI : BaseDynamicCameraAI {

    public GameObject LookAt;

    [Header("Smoothing")]
    public bool EnableSmoothing = true;
    public AnimationCurve MovementCurve;
    public float UnitTimeToTravel = 1f;
    public float MinimalTimeToTravel = 1.5f;

    [Header("Chaining")]
    public GameObject NextAI;

    protected override void Begin() {
        DynCam.StartCoroutine(EnableSmoothing ? SmoothMovement() : Movement());
    }

    private IEnumerator Movement() {
        while (true) {
            DynCam.transform.position = transform.position;
            DynCam.transform.LookAt(LookAt.transform);

            if (NextAI == null)
                yield return null;
            else {
                var ai = NextAI.GetComponent<BaseDynamicCameraAI>();
                if (!ai)
                    Debug.Log("Next AI is not valid - no dynamic camera ai script attached");
                else {
                    ai.AssumeDirectControl();
                    break;
                }
            }
        }
    }

    private IEnumerator SmoothMovement() {
        // Calculate moving into position
        var currentPosition = DynCam.transform.position;
        var desiredPosition = transform.position;
        var timeToTravel = Vector3.Distance(currentPosition, desiredPosition)*UnitTimeToTravel;
        if (timeToTravel < MinimalTimeToTravel)
            timeToTravel = MinimalTimeToTravel;

        // if the travel time is too small, we simply cut. This removes problems with near-zero distance travels
        if (timeToTravel > 0.05f) {
            // We find the pivot point by finding the closest point to the target that is equidistant from the current and desired positions.
            var midpoint = (currentPosition + desiredPosition)/2;
            var relTarget = LookAt.transform.position - midpoint;
            var projectionNormal = (currentPosition - midpoint).normalized;
            var relPivot = Vector3.ProjectOnPlane(relTarget, projectionNormal);
            var pivot = relPivot + midpoint;
            // make motion more or less circular depending on relative target closeness to relative pivot. Closer means more circular, further means more linear.
            pivot += relPivot.normalized*Vector3.Distance(relPivot, relTarget);

            var aroundPivot = Quaternion.FromToRotation(currentPosition - pivot, desiredPosition - pivot);
            Func<float, Vector3> smoothedMovement = ratio => Quaternion.Lerp(Quaternion.identity, aroundPivot, ratio) * (currentPosition - pivot) + pivot;

            // Calculate look at rotation
            var currentLookDirection = DynCam.transform.forward;
            Func<float, Quaternion> smoothedLookAt = ratio => Quaternion.LookRotation(Vector3.Slerp(currentLookDirection, LookAt.transform.position - desiredPosition, ratio));

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

        if (NextAI == null)
            yield return DynCam.StartCoroutine(Movement());
        else {
            var ai = NextAI.GetComponent<BaseDynamicCameraAI>();
            if (!ai)
                Debug.Log("Next AI is not valid - no dynamic camera ai script attached");
            else 
                ai.AssumeDirectControl();
        }
    }

    void OnDrawGizmosSelected() {
        if (!LookAt)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(LookAt.transform.position, transform.position);
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}