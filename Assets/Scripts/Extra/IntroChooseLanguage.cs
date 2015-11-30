using UnityEngine;
using System.Collections.Generic;

public class IntroChooseLanguage : MonoBehaviour
{
    public List<GameObject> ObjectsToEnable;
    public IntroLevelController IntroController;
    public bool ShowStoryText = false;

    public void SetDanish()
    {
        I18n.GetInstance().LoadLanguage(I18n.LanguageKeys.Danish);
        ShutDown();
    }

    public void SetEnglish()
    {
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
