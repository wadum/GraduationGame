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
        foreach (Transform child in _transform.GetComponentsInChildren<Transform>())
        {
            if (child.tag == "TimeManipulationObject" || child.tag == "Rock")
            {
                var mesh = child.GetComponent<MeshRenderer>();
                var mesh2 = child.GetComponent<SkinnedMeshRenderer>();
                if (mesh || mesh2)
                {
                    HighlightScript script = child.gameObject.AddComponent<HighlightScript>();
                    script.SetWidth(width);
                    _list.Add(script);
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