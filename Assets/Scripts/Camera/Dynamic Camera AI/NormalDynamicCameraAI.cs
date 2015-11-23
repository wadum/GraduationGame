public class NormalDynamicCameraAI : BaseDynamicCameraAI {

    protected override void Begin() {
        DynCam.SetTarget(Player);
        DynCam.Run();
    }
}