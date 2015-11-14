using UnityEngine;
using System.Collections;

public class BackFromSettings: MonoBehaviour {

    public Animator anim;
    public Canvas canvas;
    float wait = 0.5f;
    float _time;

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
    {
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject == gameObject)
            {
                    _time = Time.time;
                    anim.SetBool("Settings", false);

                }
            }
        }

        if (!canvas.enabled && !anim.GetBool("Settings") && !anim.GetBool("Character"))
        {
            if (Time.time > _time + wait)
                canvas.enabled = true;
        }

    }


}
