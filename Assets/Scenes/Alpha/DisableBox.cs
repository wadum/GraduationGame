using UnityEngine;
using System.Collections;

public class DisableBox : MonoBehaviour {

    public GameObject obj;
    public GameObject obj2;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "mainPillar")
        {
            obj.SetActive(!obj.activeSelf);
            obj2.SetActive(obj.activeSelf);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.name == "mainPillar")
        {
            obj.SetActive(!obj.activeSelf);
            obj2.SetActive(obj.activeSelf);
        }
    }
}
