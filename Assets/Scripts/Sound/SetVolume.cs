using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer GameAudioMixer;

    private float _volume;

    void Start()
    {
        GameAudioMixer.SetFloat("masterVol", PlayerPrefs.GetFloat("masterVol"));
        GameAudioMixer.SetFloat("sfxVol", PlayerPrefs.GetFloat("sfxVol"));
        GameAudioMixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol"));
    }
}
