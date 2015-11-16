using UnityEngine;

public class IntroLevelController : MonoBehaviour
{
    public StoryTextController STC;
    public GameObject AnimationToEnable;

    public void StartText()
    {
        STC.Show("IntroScene");
    }

    public void PlayAnimations() // called from close button
    {
        AnimationToEnable.SetActive(true);
        gameObject.SetActive(false);
    }
}
