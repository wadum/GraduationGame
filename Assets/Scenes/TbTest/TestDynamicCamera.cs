using UnityEngine;

[RequireComponent(typeof(DynamicCamera))]
public class TestDynamicCamera : MonoBehaviour {

    public string[] TapAndHoldRetargetingCameraTags;

    private DynamicCamera _dynamicCamera;

    private void Start() {
        _dynamicCamera = GetComponent<DynamicCamera>();

        foreach (var objTag in TapAndHoldRetargetingCameraTags)
            MultiTouch.RegisterTapAndHoldHandlerByTag(objTag, hit => _dynamicCamera.SetTarget(hit.transform));
    }
}
