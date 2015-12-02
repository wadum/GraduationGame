using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public AudioMixer GameAudioMixer;
    public Slider volume, sfx, music;

    private bool _restoreTouch;

    void OnDisable()
    {
        Time.timeScale = 1;
        if (_restoreTouch)
        {
            if (!GameOverlayController.gameOverlayController.isActiveAndEnabled)
                return;

            // We need a place to run the coroutine, and it cannot be on us since we are disabled...
            // We know that the game overlay controller must be here. Sorta a hack.
            GameOverlayController.gameOverlayController.StartCoroutine(FrameDelayedRestore());
        }
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
        _restoreTouch = MultiTouch.Instance.enabled;
        MultiTouch.Instance.enabled = false;
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

    private static IEnumerator FrameDelayedRestore() {
        /* Reasoning:
         * We activate the MultiTouch system when we disable the Pause Menu
         * overlay, with the unfortunate result that the MultiTouch system does
         * not consider the menu as being in the way when checking if a touch
         * was made. Due to timing, this can result in the touch going "through"
         * the menu.
         */

        yield return new WaitForEndOfFrame();
        MultiTouch.Instance.enabled = true;
    }
}