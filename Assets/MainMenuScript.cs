using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
    public Animator anim;
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

    public void LoadLevel(string name)
    {

    }


}
