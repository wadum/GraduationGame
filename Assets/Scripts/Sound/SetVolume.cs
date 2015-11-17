using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer GameAudioMixer;

    private float _volume;

    void Start()
    {
        _volume = PlayerPrefs.GetFloat("Volume");
//        _volume = -80 + _volume;
        GameAudioMixer.SetFloat("masterVol", _volume);
    }
}
