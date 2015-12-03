using UnityEngine;
using System.Collections;

public class Level99RotateAroundPosition : MonoBehaviour {

    public float RotationSpeed = 40;
    public Transform RotateAroundPos;
    public float WaitTime = 0.5f;

    float ActualWaitTime;
    Vector3 _originalPos;
	
    void Start()
    {
        ActualWaitTime = WaitTime + Time.time;
        _originalPos = transform.position;
    }

	void Update () {
        if (ActualWaitTime < Time.time)
        {
            transform.RotateAround(RotateAroundPos.position, Vector3.up, (RotationSpeed + Random.Range(0, 200) * Random.Range(1, 4)) * Time.deltaTime);
        }
    }

    void OnDisable()
    {
        transform.position = _originalPos;
    }
    void OnEnable()
    {
        ActualWaitTime = WaitTime + Time.time; 
    }
}
