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
    [NonSerialized]
    public Electrified[] DeactiveObjects;

    [Space(5)]
    [Header("Objects Controlled By Time")]
    public GameObject[] EnableObjects;
    public GameObject[] DisableObjects;

    // Use this for initialization
    void Start()
    {
        MultiTouch.RegisterTapHandlerByTag("TimeManipulationObject", hit =>
        {
            if (hit.collider.gameObject.GetComponentInParent<ObjectTimeController>() == gameObject.GetComponent<ObjectTimeController>())
            {
                FindObjectOfType<GameOverlayController>().Activate(this);
            }
        });

        foreach (GameObject obj in EnableObjects)
        {
            obj.SetActive(TimePos > workingStateStartPercent && TimePos <= workingStateEndPercent);
        }
        foreach (GameObject obj in DisableObjects)
        {
            obj.SetActive(!(TimePos > workingStateStartPercent && TimePos <= workingStateEndPercent));
        }
    }
	
	override public void SetFloat(float var)
    {
        TimePos = var;

        if (ActiveObjects.Any(b => !b.Active)) return;
        if (DeactiveObjects.Any(b => b.Active)) return;
        
        foreach (GameObject obj in EnableObjects)
        {
            obj.SetActive(TimePos > workingStateStartPercent && TimePos <= workingStateEndPercent);
        }
        foreach (GameObject obj in DisableObjects)
        {
            obj.SetActive(!(TimePos > workingStateStartPercent && TimePos <= workingStateEndPercent));
        }

    }

	override public float GetFloat()
    {
        return TimePos;
    }

}
