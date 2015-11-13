using UnityEngine;
using System.Collections;

public class LoadNextScene : MonoBehaviour {

	void OnEnable()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
