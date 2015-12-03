using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroLevelController : MonoBehaviour
{
    public float SyncWaitTime = 1f;
    public StoryTextController STC;
    public GameObject[] AnimationToEnable;
    public GameObject IntroObject;
    public Canvas canva;
    public Camera animationCamera;
    public Camera oldCamera;
    public bool Skipable;
    public Button button;
    public bool GoToMenu;

    float fadeout;

    void OnEnable()
    {
        // If we already picked a language, we wont repick it.
        if (PlayerPrefs.GetString(I18n.PlayerPrefKey) == I18n.LanguageKeys.Danish.ToString() || PlayerPrefs.GetString(I18n.PlayerPrefKey) == I18n.LanguageKeys.English.ToString())
        {
            if(IntroObject)
                IntroObject.SetActive(false); // LanguagePicker and StoryText
            PlayAnimations();
        }
    }

    IEnumerator WaitAndPlay()
    {
            yield return new WaitForSeconds(SyncWaitTime);
            if (oldCamera)
                oldCamera.gameObject.SetActive(false);
            Skipable = true;
            button.gameObject.SetActive(false);
            foreach (GameObject ani in AnimationToEnable)
            {
                ani.SetActive(true);
            }
            if(IntroObject)
                IntroObject.SetActive(false); // LanguagePicker and StoryText
            if(animationCamera)
                canva.worldCamera = animationCamera;
        yield return null;
    }

    public void StartText()
    {
        STC.Show("IntroScene");
    }

    public void PlayAnimations() // called from close button of storytext or language picker when the storytext is disabled
    {
        StartCoroutine(WaitAndPlay());
        return;
    }

    void Update()
    {
        if (!Skipable)
            return;
        if (button.gameObject.activeSelf)
        {
            if (fadeout > 2f)
                button.gameObject.SetActive(false);
            fadeout += Time.deltaTime;
            return;
        }
        if (Input.GetMouseButton(0))
        {
            button.gameObject.SetActive(true);
            fadeout = 0;
        }
    }

    public void SkipAnimation()
    {
        if (!GoToMenu)
            Application.LoadLevel(Application.loadedLevel + 1);
        else
            Application.LoadLevel(3);
    }
}
