using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterDissolver : MonoBehaviour {
    List<Dissolver> Dissolvers;
    public GameObject root;
    public bool Dissolving;
    public float GonnaGetHigh = 3f;
	// Use this for initialization
    void OnEnable()
    {
        Dissolvers = new List<Dissolver>();
        Renderer[] rends = root.GetComponentsInChildren<Renderer>();
        foreach(Renderer rend in rends)
        {
            if (rend.material.shader.name == "Shader Forge/ObejctDissolver")
            {
                Dissolver script = rend.gameObject.AddComponent<Dissolver>();
                script.AmountOfTime = GonnaGetHigh;
                if (Dissolving)
                {
                    StartCoroutine(script.Dissolve());
                }
                else StartCoroutine(rend.gameObject.AddComponent<Dissolver>().Resolve());
            }
        }
	}
}
