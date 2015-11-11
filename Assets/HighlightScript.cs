using UnityEngine;
using System.Collections;

public class HighlightScript : MonoBehaviour {

    Renderer rend;
    Shader orgshader;

    void OnEnable()
    {
        rend = GetComponent<Renderer>();
        if (!rend)
            Debug.Log(name);
        orgshader = rend.material.shader;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        MasterHighlight master = transform.root.GetComponent<MasterHighlight>();
        if(!master)
            master = transform.parent.GetComponent<MasterHighlight>();

        master.Activate();
                rend.material.shader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
    }

    void OnMouseExit()
    {
        MasterHighlight master = transform.root.GetComponent<MasterHighlight>();
        if (!master)
            master = transform.parent.GetComponent<MasterHighlight>();
        master.Deactivate();
        rend.material.shader = orgshader;//Shader.Find("Diffuse");
    }

    public void Activate()
    {
        rend.material.shader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
    }

    public void Deactivate()
    {
        rend.material.shader = orgshader;// Shader.Find("Diffuse");
    }
}
