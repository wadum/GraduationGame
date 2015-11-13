using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class I18nCutscene : MonoBehaviour
{
    public string Filename;
    private Text _text;

    // Use this for initialization
    void Start()
    {
        _text = GetComponent<Text>();
        _text.text = GetText();
    }

    private string GetText()
    {
        return Resources.Load<TextAsset>(
            string.Format("Texts/{0}/{1}", I18n.GetInstance().GetLanguageName(), Filename))
            .text;
    }
}
