using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterHighlight : MonoBehaviour {
    List<HighlightScript> _list;

	// Use this for initialization
	void Start () {
        _list = new List<HighlightScript>();
        TagChildren(transform);
    }

    void TagChildren(Transform _transform)
    {
        foreach (Transform child in _transform.GetComponentInChildren<Transform>())
        {
            if (child.tag == "TimeManipulationObject")
            {
                var mesh = child.GetComponent<MeshRenderer>();
                if (mesh)
                {
                    child.gameObject.AddComponent<HighlightScript>();
                    _list.Add(child.GetComponent<HighlightScript>());
                }
                else
                {
                    TagChildren(child);
                }
            }
        }
    }

    public void Activate()
    {
        foreach (HighlightScript script in _list)
            script.Activate();
    }

    public void Deactivate()
    {
        foreach (HighlightScript script in _list)
            script.Deactivate();
    }

    // Update is called once per frame
    void Update () {
	
	}
}