using UnityEngine;
using System.Collections;

public class DisableBox : MonoBehaviour {

    public GameObject obj;
    public GameObject obj2;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Breakable Wall")
        {
            obj.SetActive(!obj.active);
            obj2.SetActive(obj.active);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Breakable Wall")
        {
            obj.SetActive(!obj.active);
            obj2.SetActive(obj.active);
        }
    }
}
