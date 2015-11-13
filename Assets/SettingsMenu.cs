using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    public bool Active;
    public GameObject panel;
    public int w = 500;
    public int h = 200;
    string language;
    public GUIStyle tex;
    public GUIStyle outer;
    public GUIStyle credits;
    public Canvas canvas;
    float wait = 0.5f;
    float _time;

    public List<string> members;
    public bool credit;


    public Animator anim;

    public float hSliderValue;
    AudioSource clicksound;
    void Start()
    {
        clicksound = GetComponent<AudioSource>();
        if (!clicksound)
            Debug.Log("Attach AudioSource to " + name + " for clicking sound, remember to set output SFX, and no play awake");
    }
    void Update()
    {
        if (!canvas.enabled && !anim.GetBool("Settings") && !anim.GetBool("Character"))
        {
            if (Time.time > _time + wait)
                canvas.enabled = true;
        }

    }
    void OnGUI()
    {
        if (Active)
        {
            language = PlayerPrefs.GetString("Lan");
			if(language == "Danish")
				I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.Danish);
			else
				I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.English);
            Rect windowRect = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);

            if (!credit)
                windowRect = GUI.Window(0, windowRect, Settings, "", outer);
            else
                windowRect = GUI.Window(0, windowRect, Credits, "", credits);

        }
    }
    void Settings(int windowID)
    {
        int elementheight = 35;
        string sound = "Sound: ";
        if (language == "Danish")
            sound = "Lyd: ";
        GUI.Label(new Rect((w - 100) / 2, elementheight * 2, 100, elementheight), sound, tex);
        float volume = PlayerPrefs.GetFloat("Volume");
        hSliderValue = GUI.HorizontalSlider(new Rect((w - 100) / 2, elementheight * 3.5f, 100, elementheight + 15), volume, 0.0F, 10.0F);
        PlayerPrefs.SetFloat("Volume", hSliderValue);

        string lan = "Language: ";
        if (language == "Danish")
            lan = "Sprog: ";
        GUI.Label(new Rect((w - 100) / 2, elementheight * 5, 100, elementheight), lan, tex);
        if (GUI.Button(new Rect((w - 100) / 2 + 100, elementheight * 6, 100, elementheight), "English", tex))
        {
            clicksound.Play();
            PlayerPrefs.SetString("Lan", "English");
			I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.English);
        }
        if (GUI.Button(new Rect((w - 100) / 2 - 100, elementheight * 6, 100, elementheight), "Danish", tex))
        {
            clicksound.Play();
            PlayerPrefs.SetString("Lan", "Danish");
			I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.Danish);

        }

        string credits = "Credits";
        if (language == "Danish")
            credits = "Holdet Bag";

        if (GUI.Button(new Rect((w - 100) / 2, elementheight * 9, 100, elementheight), credits, tex))
        {
            clicksound.Play();
            credit = true;
        }

        if (GUI.Button(new Rect((w - 100), h - elementheight * 2, 100, elementheight), "Back", tex))
        {
            clicksound.Play();
            _time = Time.time;
            Active = false;
            anim.SetBool("Settings", false);
        }
    }


    void Credits(int windowID)
    {
        int elementheight = 25;
        int n = 0;
        foreach (string member in members)
        {
            GUI.Label(new Rect(10, elementheight * (1+n), 100, elementheight), member, credits);
            n += 1;
        }
        GUI.Box(new Rect(0, elementheight, w, elementheight * n), "");
        if (GUI.Button(new Rect((w - 100), h - elementheight * 2, 100, elementheight), "Back", tex))
        {
            clicksound.Play();
            credit = false;
        }
    }


}
