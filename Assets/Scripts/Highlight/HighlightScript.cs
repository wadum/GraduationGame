using UnityEngine;
using System.Collections;

public class HighlightScript : MonoBehaviour {

    Shader orgshader;
    Vector3 orgemmision;
    bool _inRange;
    public Renderer rend;

    // When assigned, if there's no a collider, add one, so we can click the object
    void Awake()
    {
        MeshCollider collider = GetComponent<MeshCollider>();
        if (!collider)
            gameObject.AddComponent<MeshCollider>();
        rend = GetComponent<Renderer>();
    }

    // When active, turn up the Emission of the top, should only happen on rocks (called from CharacterJumping script).
    public void Activate()
    {
        rend.material.SetFloat("_Emission2", 1);
    }
    // When deactivated, turn off the Emission on the top, once again from CharacterJumping.
    public void Deactivate()
    {
        rend.material.SetFloat("_Emission2", 0);
    }

    // A bool for testing if the shader is ObjectDissolver.
    public bool TextureShader
    {
        get { return rend.material.shader.name == "Shader Forge/ObejctDissolver"; }
    }

    public bool InRange
    {
        get { return _inRange; }
        set { _inRange = value; }
    }
}
