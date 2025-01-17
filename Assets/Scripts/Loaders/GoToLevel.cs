﻿using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class GoToLevel : MonoBehaviour
{
    public int timeToWait = 2;
    private float _time = 0.0f;
    private int level;
    public AudioMixer Audiomixer;

    void Start()
    {
        level = PlayerPrefs.GetInt("LastLevel");
        SaveLoad.saveLoad.SaveInterval = 0f;
        string language = PlayerPrefs.GetString(I18n.PlayerPrefKey);
        if(language == I18n.LanguageKeys.Danish.ToString())
        {
            I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.Danish);
        }else
            I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.English);

    }

    void Update()
    {
        _time += Time.deltaTime;
        if (_time > timeToWait)
            if (level > 2) // this script should only be in the scene with the team logo
            {
              if (level == 5 && PlayerPrefs.GetInt(TutorialController.PlayerPrefAlreadySeen) == 0)
                   SaveLoad.saveLoad.ResetLevel(5);
                Application.LoadLevel(level);
            }
            else
            {
                // If we have no previus level, this must be the first time we play, and we must set the start volume, or we have no sound.
                float v;
                Audiomixer.GetFloat("masterVol", out v);
                PlayerPrefs.SetFloat("masterVol",v);
                Audiomixer.GetFloat("sfxVol", out v);
                PlayerPrefs.SetFloat("sfxVol", v);
                Audiomixer.GetFloat("musicVol", out v);
                PlayerPrefs.SetFloat("musicVol", v);

			Application.LoadLevel("intro_cinemtaic_2");
            }
    }
}
