using UnityEngine;
using System.Collections;

public class LoadNextSceneOnEnable : MonoBehaviour {

	void OnEnable()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
