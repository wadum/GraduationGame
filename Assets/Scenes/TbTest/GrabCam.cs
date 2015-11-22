using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GrabCam : MonoBehaviour {

    public float DistanceFromSceneCenter;
    public Transform SceneCenter;

    private DynamicCamera _dynCam;
    private Collider _collider;

	void Start () {
	    if (!SceneCenter) {
	        Debug.Log("No scene center chosen for grab cam trigger!");
	        enabled = false;
	        return;
	    }

	    _dynCam = FindObjectOfType<DynamicCamera>();
	    _collider = GetComponent<Collider>();
	}

    void OnTriggerEnter(Collider col) {
        if (col.tag != "Player")
            return;

        _dynCam.Stop();
        StartCoroutine(Scene());
    }

    void OnTriggerExit(Collider col) {
        if (col.tag != "Player")
            return;

        StopAllCoroutines();
        _dynCam.SetTarget(col.transform);
        _dynCam.Run();
    }

    private IEnumerator Scene() {
        _dynCam.SetTarget(SceneCenter);
        while (true) {
            _dynCam.SetPosition(0, 0, DistanceFromSceneCenter);
            _dynCam.transform.LookAt(SceneCenter);
            yield return null;
        }
    }

    void OnDrawGizmos() {
        if (!SceneCenter)
            return;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(SceneCenter.position, SceneCenter.position - SceneCenter.forward * DistanceFromSceneCenter);
    }
}