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
     //   orgshader = rend.material.shader;
    }

    public void SetWidth(float width)
    {
        rend.material.SetFloat("_OutlineWidth", width);
    }

    // Update is called once per frame
    void Update () {
	
	}

/*    void OnMouseOver()
    {
        MasterHighlight master = transform.root.GetComponent<MasterHighlight>();
        if(!master)
            master = transform.parent.GetComponent<MasterHighlight>();

        master.Activate();
        //                rend.material.shader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
        rend.material.SetFloat("_AlphaToggle", 1);
    }

    void OnMouseExit()
    {
        MasterHighlight master = transform.root.GetComponent<MasterHighlight>();
        if (!master)
            master = transform.parent.GetComponent<MasterHighlight>();
        master.Deactivate();
        //        rend.material.shader = orgshader;//Shader.Find("Diffuse");
        rend.material.SetFloat("_AlphaToggle", 0);

    }*/

    public void Activate()
    {
        //        rend.material.shader = Shader.Find("Outlined/Silhouetted Bumped Diffuse");
        rend.material.SetFloat("_AlphaToggle", 1);
    }

    public void Deactivate()
    {
        //        rend.material.shader = orgshader;// Shader.Find("Diffuse");
        rend.material.SetFloat("_AlphaToggle", 0);

    }
}
