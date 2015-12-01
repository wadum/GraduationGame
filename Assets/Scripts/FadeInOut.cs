using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FadeInOut : MonoBehaviour {

    public bool ToBlack, FadeOnEnable, LoadNextAfterFade;
    public float FadeTime, Delay;
    public Image image;

    void OnEnable()
    {
        if (FadeOnEnable)
            if (ToBlack)
                FadeToBlack();
            else
                FadeFromBlack();
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
        image.raycastTarget = true;
        float elapsedTime = 0;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
//        canvasGroup.interactable = false;
        float dist = 1;
        while(elapsedTime < Delay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;

        while (elapsedTime < time)
        {
            dist = Mathf.Lerp(0, 1, elapsedTime / time);
            canvasGroup.alpha = dist;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (LoadNextAfterFade)
        {
            LoadNextLevel();
        }
        yield return null;
    }

    IEnumerator Clear(float time)
    {
        image.raycastTarget = true;
        float elapsedTime = 0;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        float dist = 0;

        while (elapsedTime < Delay)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;

        while (elapsedTime < time)
        {
            dist = Mathf.Lerp(1, 0, elapsedTime / time);
            canvasGroup.alpha = dist;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.raycastTarget = false;
        yield return null;
    }

    public void LoadNextLevel()
    {
        if(SaveLoad.saveLoad)
            SaveLoad.saveLoad.Reset(); ;
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
