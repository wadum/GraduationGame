using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class ObjectTimeController : TimeControllable {

    [Header("Time Settings")]
    [Range(0,100)]
    public float TimePos;

    [Range(0, 100)]
    public float workingStateStartPercent;
    [Range(0, 100)]
    public float workingStateEndPercent;

    [Space(5)]
    [Header("Prerequisites")]
    public Electrified[] ActiveObjects;
    public Electrified[] DeactiveObjects;

    [Space(5)]
    [Header("Objects Controlled By Time")]
    //Enabling
    public GameObject[] EnableObjectsBeforeTimeLimit;
    public GameObject[] EnableObjectsWithinTimeLimit;
    public GameObject[] EnableObjectsAfterTimeLimit;
    //Disabling
    public GameObject[] DisableObjectsBeforeTimeLimit;
    public GameObject[] DisableObjectsWithinTimeLimit;
    public GameObject[] DisableObjectsAfterTimeLimit;

    // Use this for initialization
    void Start()
    {
        MultiTouch.RegisterTapHandlerByTag("TimeManipulationObject", hit =>
        {
            if (hit.collider.gameObject.GetComponentInParent<ObjectTimeController>() == gameObject.GetComponent<ObjectTimeController>())
            {
                FindObjectOfType<GameOverlayController>().ActivateSlider(this);
            }
        });
        //Enabling
        foreach (GameObject obj in EnableObjectsBeforeTimeLimit)
        {
            obj.SetActive(TimePos <= workingStateEndPercent);
        }
        foreach (GameObject obj in EnableObjectsWithinTimeLimit)
        {
            obj.SetActive(TimePos > workingStateStartPercent && TimePos <= workingStateEndPercent);
        }
        foreach (GameObject obj in EnableObjectsAfterTimeLimit)
        {
            obj.SetActive(TimePos > workingStateStartPercent);
        }

        //Disabling
        foreach (GameObject obj in DisableObjectsBeforeTimeLimit)
        {
            obj.SetActive(!(TimePos <= workingStateEndPercent));
        }
        foreach (GameObject obj in DisableObjectsWithinTimeLimit)
        {
            obj.SetActive(!(TimePos > workingStateStartPercent && TimePos <= workingStateEndPercent));
        }
        foreach (GameObject obj in DisableObjectsAfterTimeLimit)
        {
            obj.SetActive(!(TimePos > workingStateStartPercent));
        }
    }
	
	override public void SetFloat(float var)
    {
        TimePos = var;

        if (ActiveObjects.Any(b => !b.Active)) return;
        if (DeactiveObjects.Any(b => b.Active)) return;

        //Enabling
        foreach (GameObject obj in EnableObjectsBeforeTimeLimit)
        {
            obj.SetActive(TimePos <= workingStateEndPercent);
        }
        foreach (GameObject obj in EnableObjectsWithinTimeLimit)
        {
            obj.SetActive(TimePos > workingStateStartPercent && TimePos <= workingStateEndPercent);
        }
        foreach (GameObject obj in EnableObjectsAfterTimeLimit)
        {
            obj.SetActive(TimePos > workingStateStartPercent);
        }

        //Disabling
        foreach (GameObject obj in DisableObjectsBeforeTimeLimit)
        {
            obj.SetActive(!(TimePos <= workingStateEndPercent));
        }
        foreach (GameObject obj in DisableObjectsWithinTimeLimit)
        {
            obj.SetActive(!(TimePos > workingStateStartPercent && TimePos <= workingStateEndPercent));
        }
        foreach (GameObject obj in DisableObjectsAfterTimeLimit)
        {
            obj.SetActive(!(TimePos > workingStateStartPercent));
        }
    }

	override public float GetFloat()
    {
        return TimePos;
    }

}
