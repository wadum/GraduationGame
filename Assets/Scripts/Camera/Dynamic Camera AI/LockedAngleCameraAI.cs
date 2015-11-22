using System.Collections;
using UnityEngine;

public class LockedAngleCameraAI : BaseDynamicCameraAI {
    public GameObject Target;

    [Header("Positioning")]
    public bool Relative = false;
    [Range(0, 360)] public float Yaw;
    [Range(-90, 90)] public float Pitch;
    [Range(0, 100)] public float Distance;

    protected override void Begin() {
        DynCam.SetTarget(Target);
        DynCam.StartCoroutine(Lock());
    }

    private IEnumerator Lock() {
        while (true) {
            DynCam.SetPosition(Yaw, Pitch, Distance, Relative);
            DynCam.transform.LookAt(Target.transform);
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
