using UnityEngine;
using System.Collections;

public class FinalPieceAI : MonoBehaviour {

    public Transform ClockPosition;
    public float speed = 1.0f;
    float _startTime;
    float _distCovered = 0;
    float _fracJourney = 0;
    // Update is called once per frame

    void Start()
    {
        this.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = transform.parent.position + Vector3.up * 2;
        transform.gameObject.SetActive(false);
    }
   
    public IEnumerator PositionFinalPiece()
    {
        float _startTime = Time.time;
        Vector3 _lerpStartingPos = transform.position;
        Quaternion _lerpStartingot = transform.rotation;
        float _journeyLength = Vector3.Distance(transform.position, ClockPosition.position);

        // fly to position in clock
        while (_fracJourney < 1)
        {
            _distCovered = (Time.time - _startTime) * speed;
            _fracJourney = _distCovered / _journeyLength;
            transform.position = Vector3.Lerp(_lerpStartingPos, ClockPosition.position, _fracJourney);
            transform.rotation = Quaternion.Lerp(_lerpStartingot, ClockPosition.transform.rotation, _fracJourney);
            yield return null;
        }

        //spray gay particles
        // to b implemented

        //Disable this script since it will never need to be active again
        this.enabled = false;
        // this will never be reached but Ienumerators demands a return value on all endings
        yield return null;
    }
}
