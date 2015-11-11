using UnityEngine;
using System.Collections;
using System.IO;

public class LoadLevel : MonoBehaviour
{
    public string level;

    // Use this for initialization
    void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "/save" + level + ".save"))
            GetComponent<MeshRenderer>().enabled = false;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject == gameObject)
                {
                    Application.LoadLevel(level);
                    SaveLoad.saveLoad.SaveInterval = 2f;
                }
            }
        }
    }


    public void Enable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        this.gameObject.AddComponent<MeshCollider>();

    }
}
