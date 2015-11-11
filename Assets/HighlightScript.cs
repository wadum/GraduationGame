using UnityEngine;
using System.Collections;

public class HighlightScript : MonoBehaviour {

    Renderer rend;

    void OnEnable()
    {
        rend = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseOver()
    {
        MasterHighlight master = transform.root.GetComponent<MasterHighlight>();
        master.Activate();
                rend.material.shader = Shader.Find("Outlined/Silhouetted Diffuse");
    }

    void OnMouseExit()
    {
        MasterHighlight master = transform.root.GetComponent<MasterHighlight>();
        master.Deactivate();
        rend.material.shader = Shader.Find("Diffuse");
    }

    public void Activate()
    {
        rend.material.shader = Shader.Find("Outlined/Silhouetted Diffuse");
    }

    public void Deactivate()
    {
        rend.material.shader = Shader.Find("Diffuse");
    }
}
