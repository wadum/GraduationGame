using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
    public Animator anim;
    public SettingsMenu settings;
    public GameObject shop;
    public GameObject main;
    public GameObject back;
    AudioSource clicksound;

	// Use this for initialization
	void Start () {
        //        anim = GetComponent<Animator>();
        clicksound = GetComponent<AudioSource>();
        if(!clicksound)
            Debug.Log("Attach AudioSource to " + name + " for clicking sound, remember to set output SFX, and no play awake");
    }

    // Update is called once per frame
    void Update () {
	
	}


    public void ChooseCharacter()
    {
        clicksound.Play();
        main.SetActive(!main.activeSelf);
        anim.SetBool("Character", true);
        foreach (LoadLevel load in GameObject.FindObjectsOfType<LoadLevel>())
            load.Enable();
        back.SetActive(!back.activeSelf);
    }

    public void BackFromCharacter()
    {
        clicksound.Play();
        main.SetActive(!main.activeSelf);
        anim.SetBool("Character", false);
        foreach (LoadLevel load in GameObject.FindObjectsOfType<LoadLevel>())
            load.Disable();
        back.SetActive(!back.activeSelf);
    }

    public void GoToSettings()
    {
        clicksound.Play();
        main.SetActive(!main.activeSelf);
        anim.SetBool("Settings", true);
        settings.Active = true;
    }

    public void GoBackFromSettings() {
        main.SetActive(!main.activeSelf);
        anim.SetBool("Settings", false);
        settings.Active = false;
    }

    public void LoadLevel(string name)
    {

    }

    public void ToggleShop()
    {
        clicksound.Play();
        main.SetActive(!main.activeSelf);
        shop.SetActive(!shop.activeSelf);
    }


}
