using UnityEngine;
using System.Collections;

public class HighlightScript : MonoBehaviour {

    Shader orgshader;
    Vector3 orgemmision;
    bool _inRange;
    public Renderer rend;

    void OnEnable()
    {
        rend = GetComponent<Renderer>();
    }

    public void Activate()
    {
        rend.material.SetFloat("_Emission2", 1);
    }

    public void Deactivate()
    {
        rend.material.SetFloat("_Emission2", 0);
    }

    public bool InRange
    {
        get { return _inRange; }
        set {
            if(!value)
                rend.material.SetVector("_Emission", orgemmision);
            _inRange = value; }
    }

    public bool TextureShader
    {
        get { return rend.material.shader.name == "Shader Forge/ObejctDissolver"; }
    }
}
