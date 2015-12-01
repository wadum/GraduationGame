using UnityEngine;
using System.Collections.Generic;

public class IntroChooseLanguage : MonoBehaviour
{
    public List<GameObject> ObjectsToEnable;
    public IntroLevelController IntroController;
    public bool ShowStoryText = false;

    public void SetDanish()
    {
        PlayerPrefs.SetString(I18n.PlayerPrefKey, I18n.LanguageKeys.Danish.ToString());
        I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.Danish);
        ShutDown();
    }

    public void SetEnglish()
    {
        PlayerPrefs.SetString(I18n.PlayerPrefKey, I18n.LanguageKeys.English.ToString());
        I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.English);
        ShutDown();
    }

    void ShutDown()
    {
        if (ShowStoryText)
        {
            IntroController.StartText();
            ObjectsToEnable.ForEach(obj => { if (obj) { obj.SetActive(true); } });
        }
        else
        {
            IntroController.PlayAnimations();
        }

        gameObject.SetActive(false);
    }
}
