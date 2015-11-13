using UnityEngine;
using System.Collections;

public class ResetProgressScript : MonoBehaviour
{
    public Animator anim;
    public Canvas canvas;
    public ResetPopUp popup;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    popup.Popup = true;
                }
            }
        }

    }

    /*   void OnGUI()
       {
           var w = 50;
           var h = 50;
           Rect bx = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
           GUI.color = Color.yellow;
           GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

           if (GUI.Button(new Rect((Screen.width- w) / 2, (Screen.height- h)/2, w, h), "Yes"))
               Debug.Log("Clicked the button with an image");

       }*/
}