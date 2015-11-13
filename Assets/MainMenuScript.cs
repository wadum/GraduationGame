using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
    public Animator anim;
    public SettingsMenu settings;
    public GameObject shop;

	// Use this for initialization
	void Start () {
//        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void ChooseCharacter()
    {
        GetComponent<Canvas>().enabled = false;
        anim.SetBool("Character", true);
        foreach (LoadLevel load in GameObject.FindObjectsOfType<LoadLevel>())
            load.Enable();
    }

    public void GoToSettings()
    {
        GetComponent<Canvas>().enabled = false;
        anim.SetBool("Settings", true);
        settings.Active = true;
    }

    public void GoBackFromSettings() {
        GetComponent<Canvas>().enabled = false;
        anim.SetBool("Settings", false);
        settings.Active = false;
    }

    public void LoadLevel(string name)
    {

    }

    public void ToggleShop()
    {
        shop.SetActive(!shop.activeSelf);
    }


}
