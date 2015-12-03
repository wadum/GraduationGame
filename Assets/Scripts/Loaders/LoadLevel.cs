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
    public MoveClock Clock;


    public GUIStyle tex;
    AudioSource crystalclick;

    // Use this for initialization
    void OnEnable()
    {
        if (!File.Exists(Application.persistentDataPath + "/save" + level + ".save") && level != 5)
        {
            this.gameObject.SetActive(false);
        }

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
                    Clock.SetLevel(level);
                    confirm.SetActive(true);
                    MainMenuScript script = GameObject.FindObjectOfType<MainMenuScript>();
                    script.lvl = level;
                    if (PlayerPrefs.GetInt("Playing" + level) == 0 || PlayerPrefs.GetInt(TutorialController.PlayerPrefAlreadySeen) == 0)
                    {
                        script.Restart.SetActive(false);
                        script.Continue.text = "Start";
                    }
                    else
                    {
                        script.Restart.SetActive(true);
                        script.Continue.text = I18n.GetInstance().Translate("resume");
                    }

                    button.gameObject.SetActive(false);
                }
            }
        }
    }


    public void Enable()
    {
        if (level == 5 || File.Exists(Application.persistentDataPath + "/save" + level + ".save"))
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
