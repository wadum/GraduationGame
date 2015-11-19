using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public AudioMixer GameAudioMixer;
    public Slider volume, sfx, music;

    void OnDisable()
    {
        Time.timeScale = 1;
    }

    void Awake()
    {
        volume.value = PlayerPrefs.GetFloat("masterVol");
        sfx.value = PlayerPrefs.GetFloat("sfxVol");
        music.value = PlayerPrefs.GetFloat("musicVol");

    }

    void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void Restart()
    {
        if (SaveLoad.saveLoad)
            SaveLoad.saveLoad.Reset();
        Application.LoadLevel(Application.loadedLevel);
    }

    public void SetVolume(float volume)
    {
        GameAudioMixer.SetFloat("masterVol", volume);
        PlayerPrefs.SetFloat("masterVol", volume);
    }

    public void SetMusic(float volume)
    {
        GameAudioMixer.SetFloat("musicVol", volume);
        PlayerPrefs.SetFloat("musicVol", volume);
    }

    public void SetSFX(float volume)
    {
        GameAudioMixer.SetFloat("sfxVol", volume);
        PlayerPrefs.SetFloat("sfxVol", volume);
    }
}