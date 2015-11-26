using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterDissolver : MonoBehaviour {
    List<Dissolver> Dissolvers;
	// Use this for initialization
	void Start () {
        Dissolvers = new List<Dissolver>();
        Renderer[] rends = GameObject.FindObjectsOfType<Renderer>();
        foreach(Renderer rend in rends)
        {
            if (rend.material.shader.name == "Shader Forge/ObejctDissolver")
            {
                Dissolvers.Add(rend.gameObject.AddComponent<Dissolver>());
            }

        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach(Dissolver script in Dissolvers)
                StartCoroutine(script.Dissolve());
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (Dissolver script in Dissolvers)
                StartCoroutine(script.Resolve());
        }
    }

}
