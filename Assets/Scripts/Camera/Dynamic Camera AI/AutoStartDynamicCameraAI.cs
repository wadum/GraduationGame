using UnityEngine;

public class AutoStartDynamicCameraAI : MonoBehaviour {

    public BaseDynamicCameraAI TargetDynamicCameraAi;

    private void LateUpdate() {
        TargetDynamicCameraAi.AssumeDirectControl();
        enabled = false;
    }
}