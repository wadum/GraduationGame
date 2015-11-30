using UnityEngine;
using UnityEngine.UI;

public class IntroLevelController : MonoBehaviour
{
    public StoryTextController STC;
    public GameObject AnimationToEnable;
    public GameObject IntroObject;
    public Canvas canva;
    public Camera animationCamera;
    public Camera oldCamera;
    public bool Skipable;
    public Button button;
    float fadeout;

    public void StartText()
    {
        STC.Show("IntroScene");
    }

    public void PlayAnimations() // called from close button of storytext or language picker when the storytext is disabled
    {
        oldCamera.gameObject.SetActive(false);
        Skipable = true;
        AnimationToEnable.SetActive(true);
        IntroObject.SetActive(false); // LanguagePicker and StoryText
        canva.worldCamera = animationCamera;
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
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
