using UnityEngine;
using System.Collections;

public class GoToLevel : MonoBehaviour {

    string level;

    void Start()
    {
        level = PlayerPrefs.GetString("LastLevel");
        Debug.Log(level);
    }
    // Use this for initialization
    void Update () {
        //        SaveLoad.saveLoad.SaveInterval = 2f;
        if (Time.time > 0.5f)
            if (level != "")
            {
                Application.LoadLevel(level);
            }
            else
                Application.LoadLevel("lvl1");
        else
            Debug.Log("waiting");
	}
}
