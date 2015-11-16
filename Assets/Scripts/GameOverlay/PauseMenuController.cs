using UnityEngine;
using System.Collections;

public class PauseMenuController : MonoBehaviour
{
    void OnDisable()
    {
        Time.timeScale = 1;
    }

    void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void Restart()
    {
        if (SaveLoad.saveLoad)
            SaveLoad.saveLoad.Reset();
        Application.LoadLevel(Application.loadedLevel);
    }
}