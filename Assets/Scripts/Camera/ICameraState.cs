using UnityEngine;
using System.Collections;

public abstract class ICameraState : MonoBehaviour {

	public abstract void SetTransform(GameObject go);

	public abstract void UpdateCamera(Camera c);
}
