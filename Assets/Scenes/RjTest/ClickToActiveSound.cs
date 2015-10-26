using UnityEngine;
using System.Collections;

public class ClickToActivateSound : MonoBehaviour
{

    AudioSource sound;

    // Use this for initialization
    void Start()
    {
        sound = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) == true)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);

            if (hit.transform.gameObject.tag == "PlayOnClick")
            {
                sound.Play();
            }
        }

   /* void OntriggerEnter(Collider player)
    {
        if (player.tag != "Player")
        {
            return;
        }
        sound.Play();*/
    }
}