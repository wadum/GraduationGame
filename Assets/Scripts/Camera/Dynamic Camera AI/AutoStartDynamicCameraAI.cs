using UnityEngine;

public class AutoStartDynamicCameraAI : MonoBehaviour {

    public BaseDynamicCameraAI TargetDynamicCameraAi;

    void LateUpdate() {
        TargetDynamicCameraAi.AssumeDirectControl();
        enabled = false;
    }
}