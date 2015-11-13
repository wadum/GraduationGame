using UnityEngine;
using System.Collections.Generic;

public class IntroLevelController : MonoBehaviour
{
    public StoryTextController STC;
    public GameObject AnimationToEnable;

    // Use this for initialization
    void Start()
    {
        STC.Show("IntroScene");
    }

    public void PlayAnimations()
    {
        AnimationToEnable.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
