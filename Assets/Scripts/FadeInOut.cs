using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour {

    public bool ToBlack, FadeOnEnable, LoadNextAfterFade;
    CanvasGroup canvasGroup;
    public float FadeTime;

    void OnEnable()
    {
        if (FadeOnEnable)
            if (ToBlack)
                FadeToBlack();
            else
                FadeFromBlack();
    }

    void Start()
    {
       canvasGroup = GetComponent<CanvasGroup>();            
    }

    public void FadeToBlack()
    {
        StartCoroutine(Black(FadeTime));
    }

    public void FadeFromBlack()
    {
        StartCoroutine(Clear(FadeTime));
    }

    IEnumerator Black(float time)
    {
        float elapsedTime = 0;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        while (elapsedTime < time)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (LoadNextAfterFade)
            LoadNextLevel();
        yield return null;
    }

    IEnumerator Clear(float time)
    {
        float elapsedTime = 0;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        while (elapsedTime < time)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    public void LoadNextLevel()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
