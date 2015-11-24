using UnityEngine;
using System.Collections.Generic;

public class MasterHighlight : MonoBehaviour
{
    List<HighlightScript> _list;
    public float width;
    private bool _inRange, active, highlighted, blinking;
    Vector3 emission;
    public Color OutlineColour = new Color(0, 1, 0.9586205f, 0);
  //  public Color Emmission = new Color(0.5f, 0.5f, 0);

    void Awake()
    {
        _list = new List<HighlightScript>();

    }

    void Start()
    {
        TagChildren(transform);
    }

    void TagChildren(Transform _transform)
    {
        foreach (Transform child in _transform.GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<MasterHighlight>() && child.transform != transform)
            {
                //Debug.Log(child.name);
                break;
            }
            if (child.tag == "TimeManipulationObject" || child.tag == "Rock")
            {
                var mesh = child.GetComponent<MeshRenderer>();
                var mesh2 = child.GetComponent<SkinnedMeshRenderer>();
                if (mesh || mesh2)
                {
                    HighlightScript script = child.gameObject.AddComponent<HighlightScript>();
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
        if (active)
            return;
        if (highlighted)
        {
       //     Debug.Log("I should work now?");
            float p = Mathf.PingPong(Time.time * 0.2f, 0.5f);
            foreach (HighlightScript script in _list)
            {
                script.rend.material.SetFloat("_Emission1", p);
            }
        }
        /*
        if (active)
        {
            Debug.Log("I'm active");
            if (!blinking)
                blinking = true;
            float p = Mathf.PingPong(Time.time * 0.2f, 0.5f);
            foreach (HighlightScript script in _list)
            {
                script.rend.material.SetFloat("_Emission2", 0.5f + p);
            }
            return;
        }
        else if (blinking)
        {
            foreach (HighlightScript script in _list)
                script.Deactivate();
//                script.OrgEmission();
            blinking = false;
        }
        */
        if (_inRange && !highlighted)
        {
            highlighted = true;
     //       foreach (HighlightScript script in _list)
      //          script.Activate();
        }
        else if (!_inRange && highlighted)
        {
            highlighted = false;
      //      foreach (HighlightScript script in _list)
      //          script.Deactivate();
        }
    }

    public void Activate()
    {
        Debug.Log("I'm being activated");
        active = true;
//        active = true;
        foreach (HighlightScript script in _list)
            script.Activate();
     //   blinking = true;
   //     highlighted = false;

    }

    public void Deactivate()
    {
        Debug.Log("I'm being deactivated");
        active = false;
        foreach (HighlightScript script in _list)
            script.Deactivate();

//        active = false;
    }

    public bool InRange
    {
        get { return _inRange; }
        set
        {
            foreach (HighlightScript script in _list)
            {
                script.InRange = value;
            }
            _inRange = value;
        }
    }
}