using System;
using UnityEngine;

/// <summary>
/// Base dynamic camera ai. This abstract class functions both to collect common
/// operations and to expose a common function, AssumeDirectControl, for
/// stopping all other dynamic camera ais from running.
/// 
/// Notice that this assumes that dynamic camera ais function using coroutines
/// running directly on the dynamic camera itself.
/// </summary>
public abstract class BaseDynamicCameraAI : MonoBehaviour {

    private static DynamicCamera _dynCam;
    protected DynamicCamera DynCam { get { return _dynCam ?? (_dynCam = FindObjectOfType<DynamicCamera>()); } }

    private static GameObject _player;
    protected GameObject Player { get { return _player ?? (_player = GameObject.FindGameObjectWithTag("Player")); } }

    protected static Action Finalizer;

    void Start() {
        _dynCam = FindObjectOfType<DynamicCamera>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void AssumeDirectControl() {
        DynCam.Stop();
        DynCam.StopAllCoroutines();

        if (Finalizer != null) {
            Finalizer();
            Finalizer = null;
        }

        Begin();
    }

    protected abstract void Begin();
}