using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
    public Animator anim;
    public GameObject settings;
    public GameObject shop;
    public GameObject main;
    public GameObject back;
    public GameObject confirm;
    AudioSource clicksound;
    public int lvl;

	// Use this for initialization
	void Start () {
        clicksound = GetComponent<AudioSource>();
        if(!clicksound)
            Debug.Log("Attach AudioSource to " + name + " for clicking sound, remember to set output SFX, and no play awake");
    }

    public void ChooseCharacter()
    {
        ClickSound();
        main.SetActive(!main.activeSelf);
        anim.SetBool("Character", true);
        foreach (LoadLevel load in GameObject.FindObjectsOfType<LoadLevel>())
            load.Enable();
        back.SetActive(!back.activeSelf);
    }

    public void BackFromCharacter()
    {
        ClickSound();
        main.SetActive(!main.activeSelf);
        anim.SetBool("Character", false);
        anim.SetBool("Settings", false);
        foreach (LoadLevel load in GameObject.FindObjectsOfType<LoadLevel>())
            load.Disable();
        back.SetActive(!back.activeSelf);
    }

    public void GoToSettings()
    {
        ClickSound();
        main.SetActive(!main.activeSelf);
        anim.SetBool("Settings", true);
        settings.SetActive(true);
    }

    public void GoBackFromSettings() {
        main.SetActive(!main.activeSelf);
        anim.SetBool("Settings", false);
        settings.SetActive(false);
    }

    public void ToggleShop()
    {
        ClickSound();
        main.SetActive(!main.activeSelf);
        shop.SetActive(!shop.activeSelf);
        anim.SetBool("Bazzaar", !anim.GetBool("Bazzaar"));
    }

    public void Load()
    {
        confirm.SetActive(false);
        if(SaveLoad.saveLoad)
            SaveLoad.saveLoad.SaveInterval = 2f;
        Application.LoadLevel(lvl);
    }

    public void RestartLevel()
    {
        confirm.SetActive(false);
        SaveLoad.saveLoad.ResetFrom(lvl);
        if (lvl == 5)
        {
            PlayerPrefs.SetInt(TutorialController.PlayerPrefAlreadySeen, 0);
            PlayerPrefs.SetInt(TutorialControllerLevel2.PlayerPrefAlreadySeen, 0);
            Application.LoadLevel(4);
        }
        else if (lvl == 7)
        {
            PlayerPrefs.SetInt(TutorialControllerLevel2.PlayerPrefAlreadySeen, 0);
            Application.LoadLevel(7);
        }
        else
        {
            SaveLoad.saveLoad.SaveInterval = 2f;
            Application.LoadLevel(lvl);
        }
    }

    public void HideConfirm()
    {
        confirm.SetActive(false);
        back.SetActive(true);
    }

    public void ClickSound()
    {
        if (clicksound)
            clicksound.Play();
    }
}
