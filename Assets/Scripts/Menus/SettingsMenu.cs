using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer GameAudioMixer;
    public AudioSource clicksound;
    public Image image;
    bool active;
    public GameObject buttons;
    public Button back;
    public Slider volume, sfx, music;
    Image[] _clickers;
    public Image CreditsDanish;
    public Image CreditsEnglish;
    public GUIStyle tex;
    public AudioSource SFXVolSound;
    private bool _FirstSFXVol = true;

    public List<string> members;
    public bool showCredits;
    float h, w;

    void OnEnable()
    {
        back.gameObject.SetActive(true);
    }


    void Awake()
    {
        _clickers = GetComponentsInChildren<Image>();
        volume.value = PlayerPrefs.GetFloat("masterVol");
        sfx.value = PlayerPrefs.GetFloat("sfxVol");
        music.value = PlayerPrefs.GetFloat("musicVol");
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        GameAudioMixer.SetFloat("masterVol", volume);
        PlayerPrefs.SetFloat("masterVol", volume);
    }

    public void SetMusic(float volume)
    {
        if (volume == -30)
        {
            GameAudioMixer.SetFloat("musicVol", -80);
            PlayerPrefs.SetFloat("musicVol", -80);
        }
        else
        {
            GameAudioMixer.SetFloat("musicVol", volume);
            PlayerPrefs.SetFloat("musicVol", volume);
        }
    }

    public void SetSFX(float volume)
    {
        if(_FirstSFXVol)
        {
            _FirstSFXVol = false;
        }
        else
        {
            float oldVol = 0;
            if (Mathf.Abs(volume - oldVol) > 1.5f)
            {
                GameAudioMixer.GetFloat("sfxVol", out oldVol);
                if (SFXVolSound)
                {
                    if (!SFXVolSound.isPlaying)
                        SFXVolSound.Play();

                }
            }
        }
        if (volume == -30)
        {
            GameAudioMixer.SetFloat("sfxVol", -80);
            PlayerPrefs.SetFloat("sfxVol", -80);
        }
        else
        {
            GameAudioMixer.SetFloat("sfxVol", volume);
            PlayerPrefs.SetFloat("sfxVol", volume);

        }
    }

    public void SetDanish()
    {
        clicksound.Play();
        PlayerPrefs.SetString(I18n.PlayerPrefKey, I18n.LanguageKeys.Danish.ToString());
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
        PlayerPrefs.SetString(I18n.PlayerPrefKey, I18n.LanguageKeys.English.ToString());
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

        if (PlayerPrefs.GetString(I18n.PlayerPrefKey) == I18n.LanguageKeys.Danish.ToString())
        {
            CreditsDanish.gameObject.SetActive(true);
        } else
        {
            CreditsEnglish.gameObject.SetActive(true);
        }

        buttons.SetActive(false);
        back.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
    }

    public void ReturnFromCredits()
    {
        clicksound.Play();

        if (PlayerPrefs.GetString(I18n.PlayerPrefKey) == I18n.LanguageKeys.Danish.ToString())
        {
            CreditsDanish.gameObject.SetActive(false);
        }
        else
        {
            CreditsEnglish.gameObject.SetActive(false);
        }
        buttons.SetActive(true);
        back.gameObject.SetActive(true);
        showCredits = false;
        image.gameObject.SetActive(true);
    }

}
