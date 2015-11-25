using UnityEngine;
using System.Collections;

public class CockRotator : MonoBehaviour {

    public float RotSpeed = 5;
    public float MoveInSpeed = 1;
    public float MoveOutSpeed = 3;
    public bool RotateClockwise;
    public GameObject PushAwayFromPoint;
    public bool moveIn = false;

    Quaternion _rot;
    float _rotationDirection { get { return (RotateClockwise ? 1f : -1f); } }

    Vector3 _originalPos;
    Vector3 _currentPos;
    Vector3 _PushOutState;

    private bool done = false;

    void Start()
    {
        _originalPos = transform.localPosition;
        _PushOutState = Vector3.LerpUnclamped(PushAwayFromPoint.transform.localPosition, transform.localPosition, 2);
        transform.localPosition = _PushOutState;
        done = true;
    }

    public void RotateWheel(float Direction, bool half = false)
    {
        transform.Rotate(Vector3.forward, (5 * RotSpeed * Time.deltaTime) * _rotationDirection * Direction * (half ? 0.4f : 1));
    }

    public void SaveRot()
    {
        _rot = transform.localRotation;
    }
    
    public void ResetRot()
    {
        transform.localRotation = _rot;
    }

    public void MoveInside()
    {
        StartCoroutine(moveInside());
    }

    public void MoveOutside()
    {
        StartCoroutine(moveOutside());
    }

    IEnumerator moveInside()
    {
        while (!done)
            yield return null;
        _currentPos = transform.localPosition;

        float fracJourney = Vector3.Distance(_PushOutState, _currentPos) / Vector3.Distance(_PushOutState, _originalPos);
        
        while (moveIn && fracJourney < 1)
        {
            fracJourney += Time.deltaTime * MoveInSpeed;
            transform.localPosition = Vector3.Lerp(_PushOutState, _originalPos, fracJourney);
            yield return null;
        }
        if (!moveIn)
        {
            yield break;
        }
        transform.localPosition = _originalPos;
    }

    IEnumerator moveOutside()
    {
        while (!done)
            yield return null;
        _currentPos = transform.localPosition;

        float fracJourney = Vector3.Distance(_originalPos, _currentPos) / Vector3.Distance(_originalPos, _PushOutState); ;
        
        while (!moveIn && fracJourney < 1)
        {
            fracJourney += Time.deltaTime * MoveOutSpeed;
            transform.localPosition = Vector3.Lerp(_originalPos, _PushOutState, fracJourney);
            yield return null;
        }
        if (moveIn)
        {
            yield break;
        }
        transform.localPosition = _PushOutState;
    }
}
