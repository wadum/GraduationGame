using UnityEngine;
using System.Collections;

public class TutorialChangeCamPos : TutorialStep {

    public BaseDynamicCameraAI dynamicCameraAi;
    public GameObject SkipButton;

    private int _taps = 0;

    private GameObject _player;

    protected override bool Continue()
    {
        return _taps > 1;
    }

    public void IncrementTaps()
    {
        Debug.Log(transform.name);
        _taps++;
    }

    override public IEnumerator Run()
	{
		yield return null;
        Text.SetActive(true);
        SkipButton.SetActive(true);
        dynamicCameraAi.AssumeDirectControl();

        float timer = Time.time + 4;
        while (timer > Time.time){
            if (Continue())
            {
                Text.SetActive(false);
                SkipButton.SetActive(false);
                yield break;
            }
			yield return null;
		}
        _taps = 0;

        while (_taps < 1)
        {
            yield return null;
        }
        Text.SetActive(false);
        SkipButton.SetActive(false);
    }

}
