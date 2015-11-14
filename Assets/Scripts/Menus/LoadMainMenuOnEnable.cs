using UnityEngine;
using System.Collections;

public class LoadMainMenuOnEnable : MonoBehaviour {

    void OnEnable()
    {
        Application.LoadLevel("Main Menu");
    }
}
