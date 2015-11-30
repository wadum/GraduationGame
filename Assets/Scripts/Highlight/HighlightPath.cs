using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighlightPath : MonoBehaviour
{
    private Stack<Transform> disabledRocks;
    public List<Transform> objectsToHighlight;
    [Header("This is for the way down in lvl2")]
    [Header("we need to reactivate the rocks when the slider is at max")]
    public bool ActivateSelectedItemsAfterValue;
    public float ValueToActivateSelectedItems;
    private ObjectTimeController _obj;
    private bool _activatedExtra;

    void Awake()
    {
        disabledRocks = new Stack<Transform>();
        if (ActivateSelectedItemsAfterValue)
            _obj = transform.root.GetComponent<ObjectTimeController>();
    }

    void Update()
    {
        if (!_obj)
            return;
        if(ActivateSelectedItemsAfterValue)
            if(_activatedExtra && _obj.TimePos < ValueToActivateSelectedItems)
                foreach (Transform child in objectsToHighlight)
                {
                    child.tag = "TimeManipulationObject";

                        HighlightScript script = child.GetComponent<HighlightScript>();
                    if (script)
                        script.Deactivate();
                }
        else if (!_activatedExtra && _obj.TimePos >= ValueToActivateSelectedItems)
                foreach (Transform child in objectsToHighlight)
                {
                    if (child.tag == "TimeManipulationObject")
                        child.tag = "Rock";
                    HighlightScript script = child.GetComponent<HighlightScript>();
                    if (script)
                        script.Activate();
                }
    }

    void OnEnable()
    {
        if (ActivateSelectedItemsAfterValue)
            return;
        while (disabledRocks.Count > 0)
        {
            Transform rock = disabledRocks.Pop();
            rock.tag = "Rock";
            HighlightScript script = rock.GetComponent<HighlightScript>();
            if (script)
                script.Activate();
        }
        foreach(Transform child in objectsToHighlight)
        {
            HighlightScript script = child.GetComponent<HighlightScript>();
            if (script)
                script.Activate();
        }
    }

    void OnDisable()
    {
        if (ActivateSelectedItemsAfterValue)
            return;
        foreach (Transform child in transform.root.GetComponentsInChildren<Transform>())
        {
            if (child.tag == "Rock")
            {
                child.tag = "TimeManipulationObject";
                HighlightScript script = child.GetComponent<HighlightScript>();
                if(script)
                  script.Deactivate();
                disabledRocks.Push(child);
            }
        }
        foreach (Transform child in objectsToHighlight)
        {
            HighlightScript script = child.GetComponent<HighlightScript>();
            if (script)
                script.Deactivate();
        }
    }
}