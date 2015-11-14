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
        string language = PlayerPrefs.GetString("Lan");
        if(language == "Danish")
        {
            I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.Danish);
        }else
            I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.English);

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
            {
                // If we have no previus level, this must be the first time we play, and we must set the start volume, or we have no sound.
                PlayerPrefs.SetFloat("Volume", 65);
                Application.LoadLevel("Intro Cinematic");
            }
    }
}
