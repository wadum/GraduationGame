using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterHighlight : MonoBehaviour {
    List<HighlightScript> _list;
    public float width;
    private bool _inRange;

    void Awake()
    {
        _list = new List<HighlightScript>();

    }
    // Use this for initialization
    void Start () {
        TagChildren(transform);
    }

    void TagChildren(Transform _transform)
    {
        foreach (Transform child in _transform.GetComponentInChildren<Transform>())
        {
            if (child.tag == "TimeManipulationObject" || child.tag == "Rock")
            {
                var mesh = child.GetComponent<MeshRenderer>();
                if (mesh)
                {
                    HighlightScript script = child.gameObject.AddComponent<HighlightScript>();
                    script.SetWidth(width);
                    _list.Add(script);
                }
                    TagChildren(child);
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

    public bool InRange
    {
        get { return _inRange; }
        set {
            foreach(HighlightScript script in _list)
            {
                script.InRange = value;
            }
            _inRange = value;
        }
    }
}