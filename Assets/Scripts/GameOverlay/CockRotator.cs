using UnityEngine;
using System.Collections;

public class CockRotator : MonoBehaviour {

    public float speed = 5;
    public bool RotateClockwise;

    Quaternion _rot;
    float _rotationDirection;

    void Start()
    {
        _rotationDirection = RotateClockwise ? 1 : -1;
    }

    public void RotateWheel(float Direction, bool half = false)
    {
        transform.Rotate(Vector3.forward, (5 * speed * Time.deltaTime) * _rotationDirection * Direction * (half ? 0.4f : 1));
    }

    public void SaveRot()
    {
        _rot = transform.localRotation;
    }
    
    public void ResetRot()
    {
        transform.localRotation = _rot;
    }
}
