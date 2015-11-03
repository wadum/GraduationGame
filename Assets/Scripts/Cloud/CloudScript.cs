using UnityEngine;
using System.Collections;
using System;

public class CloudScript : BaseTimeTraveller
{
    public TimePeriod DefaultTimePeriod;

    public GameObject Wind;
    public Transform PastLocation, PresentLocation, FutureLocation;

    private Vector3 _PastLocation, _PresentLocation, _FutureLocation;
    private TimePeriod _currenTimePeriod;


    void Awake()
    {
        // Since Past,Present and FutureLocation are childs of this object, we save their world position at Awake, so they dont change when we move.
        _PastLocation = PastLocation.transform.position;
        _PresentLocation = PresentLocation.transform.position;
        _FutureLocation = FutureLocation.transform.position;

        switch (DefaultTimePeriod)
        {
            case TimePeriod.Past:
                transform.position = _PastLocation;
                break;
            case TimePeriod.Present:
                transform.position = _PresentLocation;
                break;
            case TimePeriod.Future:
                transform.position = _FutureLocation;
                break;
        }

        _currenTimePeriod = DefaultTimePeriod;
    }
    // Use this for initialization
    void Start () {
        InvokeRepeating("SpawnWind", 0.5f, 0.5f);
    }

    void SpawnWind()
    {
        GameObject la = (GameObject)Instantiate(Wind, transform.position, Quaternion.identity);

        // The outgoing direction of the wind matters, according to the transform rotation of the Locations (They move along the Z Axis (The Blue arrow), which will rotate with the object.
        switch (_currenTimePeriod)
        {
            case TimePeriod.Past:
                la.GetComponent<ObjectMoveScript>().SetDirection(PastLocation.forward);
                break;
            case TimePeriod.Present:
                la.GetComponent<ObjectMoveScript>().SetDirection(PresentLocation.forward);
                break;
            case TimePeriod.Future:
                la.GetComponent<ObjectMoveScript>().SetDirection(FutureLocation.forward);
                break;
        }
        la.transform.parent = transform;
    }

    public override TimePeriod GetCurrentTimePeriod()
    {
        return _currenTimePeriod;
    }

    public override TimePeriod GetDefaultTimePeriod()
    {
        return DefaultTimePeriod;
    }

    public override void SetPast()
    {
        if (_currenTimePeriod == TimePeriod.Past)
            return;

        // GO to past position
        Prolicide();
        transform.position = _PastLocation;
        _currenTimePeriod = TimePeriod.Past;
    }

    public override void SetPresent()
    {
        if (_currenTimePeriod == TimePeriod.Present)
            return;

        // GO to present position
        Prolicide();
        transform.position = _PresentLocation;
        _currenTimePeriod = TimePeriod.Present;
    }

    public override void SetFuture()
    {
        if (_currenTimePeriod == TimePeriod.Future)
            return;

        // GO to future position
        Prolicide();
        transform.position = _FutureLocation;
        _currenTimePeriod = TimePeriod.Future;
    }

    // When we move in Time, old wind particles should be killed.
    void Prolicide()
    {
        foreach(Transform child in transform)
        {
            if (child.tag == "Wind")
                Destroy(child.gameObject);
        }
    }
}
