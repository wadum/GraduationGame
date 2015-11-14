using UnityEngine;
using System.Collections;

public class GoToLevel : MonoBehaviour
{
    public int timeToWait = 2;
    private float _time = 0.0f;
    private string level;

    void Start()
    {
        level = PlayerPrefs.GetString("LastLevel");
        SaveLoad.saveLoad.SaveInterval = 0f;
    }

    void Update()
    {
        _time += Time.deltaTime;
        if (_time > timeToWait)
            if (level != "") // this script should only be in the scene with the team logo
            {
                Application.LoadLevel(level);
            }
            else
                Application.LoadLevel("Intro Cinematic");
    }
}
