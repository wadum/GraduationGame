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

    private SphereCollider _collider;
    private bool _InRange = false;

    void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        if (!_collider)
        {
            Debug.Log("Missing SphereCollider on " + name + " - please add");
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    // Use this for initialization
    void Start()
    {
        MultiTouch.RegisterTapAndHoldHandlerByTag("TimeManipulationObject", hit =>
        {
            if (hit.collider.gameObject.GetComponentInParent<ObjectTimeController>() == gameObject.GetComponent<ObjectTimeController>() && _InRange)
            {
                FindObjectOfType<GameOverlayController>().ActivateSlider(this);
            }
        });

        MultiTouch.RegisterTapAndHoldHandlerByTag("Rock", hit =>
        {
            if (hit.collider.gameObject.GetComponentInParent<ObjectTimeController>() == gameObject.GetComponent<ObjectTimeController>() && _InRange)
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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            _InRange = true;
            MasterHighlight master = GetComponent<MasterHighlight>();
            if (master)
                master.InRange = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        _InRange = false;
        MasterHighlight master = GetComponent<MasterHighlight>();
        if (master)
            master.InRange = false;

    }
}
