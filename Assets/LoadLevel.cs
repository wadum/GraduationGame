using UnityEngine;
using System.Collections;
using System.IO;

public class LoadLevel : MonoBehaviour
{
    public int level;
    public bool entering;
    int w = 250;
    int h = 200;
    string language;


    // Use this for initialization
    void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "/save" + level + ".save"))
            GetComponent<MeshRenderer>().enabled = false;
        language = PlayerPrefs.GetString("Language");
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
                    entering = true;
                }
            }
        }
    }


    public void Enable()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        this.gameObject.AddComponent<MeshCollider>();

    }

    void OnGUI()
    {
        if (entering)
        {
            Rect windowRect = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
            string cont = "Continue?";
            if (language == "Danish")
                cont = "Fortsæt?";
            windowRect = GUI.Window(0, windowRect, DoMyWindow, cont) ;
        }
    }
    void DoMyWindow(int windowID)
    {
        int elementheight = 35;
        string resume = "Resume!";
        if (language == "Danish")
            resume = "Forsæt!";

        if (GUI.Button(new Rect((w / 2 - 75), elementheight, 75, elementheight), resume))
        {
            SaveLoad.saveLoad.SaveInterval = 2f;
            Application.LoadLevel(level);
        }
        string restart = "Restart!";
        if (language == "Danish!")
            resume = "Forfra!";

        if (GUI.Button(new Rect((w / 2 - 75), elementheight * 2.5f, 75, elementheight), restart))
        {
            SaveLoad.saveLoad.ResetFrom(level);
            SaveLoad.saveLoad.SaveInterval = 2f;
            Application.LoadLevel(level);
        }
        string back = "Back";
        if (language == "Danish")
            restart = "Tilbage";
        if (GUI.Button(new Rect(w-100, h - elementheight - 15f, 75, elementheight), back))
        {
            entering = false;
        }
    }

}
