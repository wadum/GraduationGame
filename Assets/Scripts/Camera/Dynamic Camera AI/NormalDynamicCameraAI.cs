public class NormalDynamicCameraAI : BaseDynamicCameraAI {

    protected override void Begin() {
        DynCam.TargetPlayer();
        DynCam.Run();
    }
}