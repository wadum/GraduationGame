using UnityEngine;
using System.Collections;

public class GoToLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string level = PlayerPrefs.GetString("LastLevel");
        if (level != "")
        {
            Application.LoadLevel(level);
        }
        else
            Application.LoadLevel("lvl1");
	}
}
