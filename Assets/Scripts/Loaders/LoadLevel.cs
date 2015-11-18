using UnityEngine;
using System.Collections;
using System.IO;

public class LoadLevel : MonoBehaviour
{
    public int level;
    public bool entering;
    bool active;
    public UnityEngine.UI.Button button;
    public GameObject confirm;

    public GUIStyle tex;
    AudioSource crystalclick;

    // Use this for initialization
    void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "/save" + level + ".save"))
            GetComponent<MeshRenderer>().enabled = false;
        crystalclick = GetComponent<AudioSource>();
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
                    crystalclick.Play();
                    confirm.SetActive(true);
                    GameObject.FindObjectOfType<MainMenuScript>().lvl = level;
                    button.gameObject.SetActive(false);
                }
            }
        }
    }


    public void Enable()
    {
        if (File.Exists(Application.persistentDataPath + "/save" + level + ".save"))
        {
            this.gameObject.layer = LayerMask.NameToLayer("Default");
            this.gameObject.AddComponent<MeshCollider>();
        }
    }

    public void Disable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }
}
