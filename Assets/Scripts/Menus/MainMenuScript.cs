﻿using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {
    public Animator anim;
    public GameObject back;
    public GameObject confirm;
    public GameObject Restart;
    public Text Continue;
    AudioSource clicksound;
    public int lvl;
    public GameObject Gems;
    public GameObject ResetConfirm;
    private bool _entering;
	// Use this for initialization
	void Start () {
        clicksound = GetComponent<AudioSource>();
        if(!clicksound)
            Debug.Log("Attach AudioSource to " + name + " for clicking sound, remember to set output SFX, and no play awake");
    }

    public void ChooseCharacter()
    {
        ClickSound();
        anim.SetBool("Character", true);
        foreach (LoadLevel load in GameObject.FindObjectsOfType<LoadLevel>())
            load.Enable();
        back.SetActive(!back.activeSelf);
    }

    public void BackFromCharacter()
    {
        ClickSound();
        anim.SetBool("Character", false);
        anim.SetBool("Settings", false);
        foreach (LoadLevel load in GameObject.FindObjectsOfType<LoadLevel>())
            load.Disable();
        back.SetActive(!back.activeSelf);
    }

    public void GoToSettings()
    {
        ClickSound();
        anim.SetBool("Settings", true);
    }

    public void GoBackFromSettings() {
        anim.SetBool("Settings", false);
    }

    public void ToggleShop()
    {
        ClickSound();
        anim.SetBool("Bazzaar", !anim.GetBool("Bazzaar"));
    }

    public void Load()
    {
        if (_entering)
            return;
        confirm.SetActive(false);
        if (lvl == 5 &&  (!File.Exists(Application.persistentDataPath + "/save5.save") || PlayerPrefs.GetInt("Playing5") == 0))
        {
            _entering = true;
            Application.LoadLevel(4);
            return;
        }
        if (SaveLoad.saveLoad)
            SaveLoad.saveLoad.SaveInterval = 2f;
        if (lvl == 5 && PlayerPrefs.GetInt(TutorialController.PlayerPrefAlreadySeen) == 0)
        {
            _entering = true;
            SaveLoad.saveLoad.ResetLevel(5);
        }
        _entering = true;
        Application.LoadLevel(lvl);
    }

    public void RestartLevel()
    {
        if (!SaveLoad.saveLoad)
        {
            Debug.LogError("You dont have a SaveLoad instance, remember to start from the DADIU screen lvl");
            return;
        }
        confirm.SetActive(false);
        SaveLoad.saveLoad.ResetLevel(lvl);
        if (lvl == 5)
        {
            PlayerPrefs.SetInt(TutorialController.PlayerPrefAlreadySeen, 0);
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

    public void ToggleReset()
    {
        ResetConfirm.SetActive(!ResetConfirm.activeSelf);
    }

    public void RestartEVEYTHING()
    {
        ResetConfirm.SetActive(!ResetConfirm.activeSelf);
        PlayerPrefs.SetInt("Playing5", 0);
        PlayerPrefs.SetInt("Playing7", 0);
        PlayerPrefs.SetInt("Playing9", 0);
        PlayerPrefs.DeleteKey(TutorialController.PlayerPrefAlreadySeen);
        PlayerPrefs.DeleteKey(TutorialControllerLevel2.PlayerPrefAlreadySeen);
        SaveLoad.saveLoad.ResetEverything();
        Gems.SetActive(false);
        Gems.SetActive(true);
    }

}
