using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class ObjectTimeController : MonoBehaviour, TimeControllable {

    public float TimePos;

    public float workingStateStartPercent;
    public float workingStateEndPercent;

    public GameObject[] ItemsControlledByTime;
    
	// Use this for initialization
    void Start()
    {
        MultiTouch.RegisterTapHandlerByTag("TimeManipulationObject", hit =>
        {
            if (hit.collider.gameObject == gameObject)
            {
                FindObjectOfType<GameOverlayController>().Activate(this);
            }
        });

        foreach (var obj in ItemsControlledByTime)
        {
            obj.SetActive(TimePos > workingStateStartPercent && TimePos < workingStateEndPercent);
        }
    }


    public void SetFloat(float var)
    {
        TimePos = var;
        foreach (var obj in ItemsControlledByTime)
        {
            obj.SetActive(TimePos > workingStateStartPercent && TimePos < workingStateEndPercent);
        }

    }

    public float GetFloat()
    {
        return TimePos;
    }
}
