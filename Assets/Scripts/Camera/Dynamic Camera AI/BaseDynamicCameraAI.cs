using UnityEngine;

/// <summary>
/// Base dynamic camera ai. This abstract class functions both to collect common
///  operations and to expose a common function, AssumeDirectControl, for
///  stopping all other dynamic camera ais from running.
/// 
/// Notice that this assumes that dynamic camera ais function using coroutines
/// running directly on the dynamic camera itself.
/// </summary>
public abstract class BaseDynamicCameraAI : MonoBehaviour {

    protected DynamicCamera DynCam;
    protected GameObject Player;

    void Start() {
        DynCam = FindObjectOfType<DynamicCamera>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void AssumeDirectControl() {
        DynCam.Stop();
        DynCam.StopAllCoroutines();
        Begin();
    }

    protected abstract void Begin();
}