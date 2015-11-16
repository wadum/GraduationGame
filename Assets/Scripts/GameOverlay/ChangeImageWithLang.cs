using UnityEngine;
using UnityEngine.UI;

public class ChangeImageWithLang : MonoBehaviour
{
    public Image ThisImage;
    public Sprite EnglishImage,
        DanishImage;

    void OnEnable()
    {
        if (I18n.GetInstance().GetLanguageName() == "English")
            ThisImage.sprite = EnglishImage;
        else
            ThisImage.sprite = DanishImage;
    }
}
