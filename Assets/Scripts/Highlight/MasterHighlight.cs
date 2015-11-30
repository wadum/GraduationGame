using UnityEngine;
using System.Collections.Generic;

public class MasterHighlight : MonoBehaviour
{
    List<HighlightScript> _list;
    private bool _inRange, active, highlighted;
    // p is the intensity of the blinking, emission is used to ensure a blink always start from 0
    private float p, emission;

    void Awake()
    {
        // We have a list of all the children connected to this MasterHighlight script
        _list = new List<HighlightScript>();
        // Find children, and add them to list
        TagChildren(transform);
    }

    // We locate all relevant children, and assign them a highlight script.
    void TagChildren(Transform _transform)
    {
        // A child is a transform.
        foreach (Transform child in _transform.GetComponentsInChildren<Transform>())
        {
            // If the child is has a Master of its own, we break - Elef wrote this!
            if (child.GetComponent<MasterHighlight>() && child.transform != transform)
            {
                break;
            }
            // If the child has a tag which needs to be highlighted, we continue, expand this as needed
            if (child.tag == "TimeManipulationObject" || child.tag == "Rock")
            {
                // The child should have some sort of Renderer.
                var mesh = child.GetComponent<MeshRenderer>();
                var mesh2 = child.GetComponent<SkinnedMeshRenderer>();
                if (mesh || mesh2)
                {
                    HighlightScript script = child.gameObject.AddComponent<HighlightScript>();
                    // We ask the script if it has a useable renderer, and kill if it's not the case.
                    if (script.TextureShader)
                    {
                        _list.Add(script);
                    }
                    else
                        Destroy(script);
                }
            }
        }
    }

    void Update()
    {
        // If selected, we dont need to blink
        if (active)
            return;
        // If we're not active, but still in range, we blink
        if (highlighted)
        {
            emission += Time.deltaTime;
            // A nice back and forward fade in and out effect is achieved with PingPong, adjust the last value to make it faster or slower.
            p = Mathf.PingPong(emission, 1.5f);
            foreach (HighlightScript script in _list)
            {
                script.rend.material.SetFloat("_Emission1", 0.125f + 0.375f * p);
            }
        }

        // If we are in range, but not already blinking, we start blinking, from zero so it fades in
        if (_inRange && !highlighted)
        {
            emission = 0;
            highlighted = true;
        }
        // If we are no longer in range, but were blinking, we stop blinking
        else if (!_inRange && highlighted)
        {
            highlighted = false;
            foreach (HighlightScript script in _list)
            {
                script.rend.material.SetFloat("_Emission1", 0);
            }
        }
    }

    // If a slider is telling us to be active, we stop blinking and keep values at max
    public void Activate()
    {
        active = true;
        foreach (HighlightScript script in _list)
        {
            script.rend.material.SetFloat("_Emission1", 1);
        }
        // for when we fade down, we should be at max intensity
        emission = 1;
    }

    // If a slider tells us we are no longer active, we return to blinking.
    public void Deactivate()
    {
        active = false;
    }
    /*
    public void ActivatePath()
    {
        foreach (HighlightScript script in _list)
            if (script.tag == "Rock")
                script.Activate();
    }

    public void DeactivatePath()
    {
        foreach (HighlightScript script in _list)
            if (script.tag == "Rock")
                script.Deactivate();
    }*/

    // GET / SET for a InRange boolean.
    public bool InRange
    {
        get { return _inRange; }
        set {
                foreach (HighlightScript script in _list)
                script.InRange = value;
                _inRange = value; }
    }
}