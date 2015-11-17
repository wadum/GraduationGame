using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer GameAudioMixer;
    AudioSource clicksound;
    public Image image;
    bool active;
    public GameObject buttons;
    public Button back;
    public Slider volume;
    Image[] _clickers;
    public Image credits;
    public GUIStyle tex;

    public List<string> members;
    public bool showCredits;
    float h, w;

    void OnEnable()
    {
        image.fillAmount = 0;
        back.gameObject.SetActive(true);
        
    }

    void Update()
    {
        if (active)
            return;
        if (image.fillAmount < 1)
        {
            image.fillAmount += Time.deltaTime*1.5f;
            return;
        }
        active = true;
        buttons.SetActive(true);
    }

    void Awake()
    {
        clicksound = GetComponent<AudioSource>();
        _clickers = GetComponentsInChildren<Image>();
        volume.value = PlayerPrefs.GetFloat("Volume");
    }

    public void Hide()
    {
        buttons.SetActive(false);
        active = false;
        this.gameObject.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        GameAudioMixer.SetFloat("masterVol", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetDanish()
    {
        clicksound.Play();
        PlayerPrefs.SetString("Lan", "Danish");
        I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.Danish);
        foreach(Image butt in _clickers) {
            if (butt.gameObject.activeSelf)
            {
                butt.gameObject.SetActive(false);
                butt.gameObject.SetActive(true);
            }
        }
        back.gameObject.SetActive(false);
        back.gameObject.SetActive(true);
    }

    public void SetEnglish()
    {
        clicksound.Play();
        PlayerPrefs.SetString("Lan", "English");
        I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.English);
        foreach (Image butt in _clickers)
        {
            if (butt.gameObject.activeSelf)
            {
                butt.gameObject.SetActive(false);
                butt.gameObject.SetActive(true);
            }
        }
        back.gameObject.SetActive(false);
        back.gameObject.SetActive(true);
    }

    public void ShowCredits()
    {
        clicksound.Play();
        credits.gameObject.SetActive(true);
        buttons.SetActive(false);
        back.gameObject.SetActive(false);
        showCredits = true;
        image.gameObject.SetActive(false);
    }

    void OnGUI()
    {
        if (showCredits)
        {
            w = Screen.width;
            h = Screen.height;

            Rect windowRect = new Rect(5,5,w-10, h-10);
                        
            windowRect = GUI.Window(0, windowRect, Credits, "");
        }
    }

    void Credits(int windowID)
    {
        float elementheight = (h / members.Count) - 1;
        float elementwidth = w - 15;
        int n = 0;
        foreach (string member in members)
        {
            GUI.Label(new Rect(0, 5 + elementheight * n, elementwidth, elementheight), member, tex);
            n += 1;
        }
        if (GUI.Button(new Rect(0, 0, w - 5, h - 5), ""))
        { 
            clicksound.Play();
            credits.gameObject.SetActive(false);
            buttons.SetActive(true);
            back.gameObject.SetActive(true);
            showCredits = false;
            image.gameObject.SetActive(true);
        }
    }

}
