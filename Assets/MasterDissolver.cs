using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterDissolver : MonoBehaviour {
    [Header("On activate, will dissolve, else they'll solidify")]
    List<Material> Dissolvers;
    [Tooltip("Objects, in which all dissolve shaders will be triggered")]
    public GameObject root;
    [Tooltip("If set, objects will fade out, else they will fade in")]
    public bool Dissolving;
    [Tooltip("Amount of time to dissolve, at start and end not much happens")]
    public float AmountOfTime = 3f;
    private float _disAmount = 0, _timeAmount = 0;

    void OnEnable()
    {
        Dissolvers = new List<Material>();
        Renderer[] rends = root.GetComponentsInChildren<Renderer>();
        // We find all the materials which we need to change dissolve float on
        foreach(Renderer rend in rends)
        {
            if (rend.material.shader.name == "Shader Forge/ObejctDissolver")
            {
                Dissolvers.Add(rend.material);
            }
        }
        _disAmount = 0;
        _timeAmount = 0;
    }

    void Update()
    {
        if (_timeAmount >= AmountOfTime)
            return;
        // We either dissolve from 0 to 1, or from 1 to 0
        if (Dissolving)
        { 
            if(_disAmount < 1)
            {
                foreach (Material mat in Dissolvers)
                {
                    // We dissolve to zero
                    mat.SetFloat("_DissolveAmount", _disAmount);
                }
                _disAmount = _timeAmount / AmountOfTime;
                _timeAmount += Time.deltaTime;
            }

        }
        else
        {
            if (_disAmount < 1)
            {
                foreach (Material mat in Dissolvers)
                {
                    // We un-dissolve from zero
                    mat.SetFloat("_DissolveAmount", 1 - _disAmount);
                }
                _timeAmount += Time.deltaTime;
                _disAmount = _timeAmount / AmountOfTime;
            }
        }
    }
}
