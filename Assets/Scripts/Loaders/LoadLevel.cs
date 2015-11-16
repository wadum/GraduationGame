using UnityEngine;
using System.Collections;
using System.IO;

public class LoadLevel : MonoBehaviour
{
    public int level;
    public bool entering;
    int w = 250;
    int h = 200;
    bool active;

    public GUIStyle tex;
    AudioSource crystalclick;
    AudioSource clicksound;


    // Use this for initialization
    void Start()
    {
        if (!File.Exists(Application.persistentDataPath + "/save" + level + ".save"))
            GetComponent<MeshRenderer>().enabled = false;
        clicksound = GameObject.FindObjectOfType<SettingsMenu>().GetComponent<AudioSource>();
        crystalclick = GetComponent<AudioSource>();
        if (!clicksound)
            Debug.Log(name + " is trying to borrow Setting's ClikSound, something went wrong");
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
                    entering = true;
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

    void OnGUI()
    {
        if (entering)
        {
            Rect windowRect = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
            windowRect = GUI.Window(0, windowRect, DoMyWindow, "") ;
        }
    }
    void DoMyWindow(int windowID)
    {
        int elementheight = 35;

        if (GUI.Button(new Rect((w / 2 - 75), elementheight, 75, elementheight), I18n.GetInstance().Translate("resume"), tex))
        {
            clicksound.Play();
            SaveLoad.saveLoad.SaveInterval = 2f;
            Application.LoadLevel(level);
        }

        if (GUI.Button(new Rect((w / 2 - 75), elementheight * 2.5f, 75, elementheight), I18n.GetInstance().Translate("restart"), tex))
        {
            clicksound.Play();
            SaveLoad.saveLoad.ResetFrom(level);
            if (level == 5)
            {
                PlayerPrefs.SetInt(TutorialController.PlayerPrefAlreadySeen, 0);
                Application.LoadLevel("Intro Cinematic");
            }
            else
            {
                SaveLoad.saveLoad.SaveInterval = 2f;
                Application.LoadLevel(level);
            }
        }
        if (GUI.Button(new Rect(w-100, h - elementheight - 15f, 75, elementheight), I18n.GetInstance().Translate("back"), tex))
        {
            clicksound.Play();
            entering = false;
        }
    }

}
