using UnityEngine;
using System.Collections;

public class HighlightScript : MonoBehaviour {

    Renderer rend;
    Shader orgshader;
    Vector3 orgemmision;
    bool _inRange;
    bool _activated;

    void OnEnable()
    {
        rend = GetComponent<Renderer>();
        if (!rend)
            Debug.Log(name);
        //   orgshader = rend.material.shader;
        if (rend.material.shader.name != "Shader Forge/ObejctDissolver")
        {
            Destroy(this);
         //   Debug.Log(name + "Shader does not support highlight");
        }
        else
            orgemmision = rend.material.GetVector("_Emission");
    }

    public void SetWidth(float width)
    {
        rend.material.SetFloat("_OutlineWidth", width);
    }


    public void Activate()
    {
        rend.material.SetFloat("_AlphaToggle", 1);
        rend.material.SetVector("_Emission", orgemmision);
        _activated = true;
    }

    public void Deactivate()
    {
        rend.material.SetFloat("_AlphaToggle", 0);
        _activated = false;
    }

    void Update()
    {
        if (_activated)
            return;
        if (InRange)
            rend.material.SetVector("_Emission", new Vector3(Mathf.PingPong(Time.time * 0.2f, 0.5f), Mathf.PingPong(Time.time*0.2f, 0.5f), orgemmision.z));

    }

    /*    public void InRange(bool value)
        {
            Vector3 v3 = new Vector3();
            if (value)
            {
                rend.material.SetVector("_Emission", new Vector3(0, 1, 0));

            }else
                rend.material.SetVector("_Emission", orgemmision);
        //    rend.material.
        }*/

    public bool InRange
    {
        get { return _inRange; }
        set {
            if(!value)
                rend.material.SetVector("_Emission", orgemmision);
            _inRange = value; }
    }
}
