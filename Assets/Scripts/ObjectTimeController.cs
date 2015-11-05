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
        TouchHandling touchHandling = FindObjectOfType<TouchHandling>();
        touchHandling.RegisterTapHandlerByTag("TimeManipulationObject", hit =>
        {
            if (hit.collider.gameObject.GetComponentInParent<ObjectTimeController>() == gameObject.GetComponent<ObjectTimeController>())
            {
                FindObjectOfType<GameOverlayController>().Activate(this);
            }
        });

        foreach (GameObject obj in ItemsControlledByTime)
        {
            obj.SetActive(TimePos > workingStateStartPercent && TimePos < workingStateEndPercent);
        }
    }


    public void SetFloat(float var)
    {
        TimePos = var;
        foreach (GameObject obj in ItemsControlledByTime)
        {
            obj.SetActive(TimePos > workingStateStartPercent && TimePos < workingStateEndPercent);
        }

    }

    public float GetFloat()
    {
        return TimePos;
    }
}
