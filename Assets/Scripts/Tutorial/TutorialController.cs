using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialController : MonoBehaviour
{

    public bool ForceTutorial = false;
    public bool DisableTutorial = true;

    public List<TutorialStep> Steps;
    public List<GameObject> ObjectsToDestroy;
    public List<GameObject> ObjectsToToggle;

    private static MultiTouch _multiTouch;

    public const string PlayerPrefAlreadySeen = "TutorialAlreadySeen";

    void Awake()
    {
        _multiTouch = FindObjectOfType<MultiTouch>();
        if ((PlayerPrefs.GetInt(PlayerPrefAlreadySeen) > 0 || DisableTutorial || Application.loadedLevelName != "lvl1") && !ForceTutorial)
            Destroy(gameObject);
        else
            ObjectsToToggle.ForEach(obj => { if (obj) obj.SetActive(false); });
    }

    void Start()
    {
        StartCoroutine(RunAllSteps());
    }

    IEnumerator RunAllSteps()
    {
        // Only once a tutorial start will we freeze the player.
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().TutorialMoveFreeze = true;
        foreach (var step in Steps)
        {
            yield return StartCoroutine(step.Run());
        }

        PlayerPrefs.SetInt(PlayerPrefAlreadySeen, 1);
        // When the tutorial is done the player is free to move around.
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().TutorialMoveFreeze = false;

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        EnableTouch();
        ObjectsToToggle.ForEach(obj => { if (obj) obj.SetActive(true); });
        ObjectsToDestroy.ForEach(obj => { if (obj) Destroy(obj); });
        var cam = GameObject.FindObjectOfType<TopDownCamController>();
        if (cam)
            cam.RegisterHandlers();
    }

    public static IEnumerator MoveCamera(Vector3 desiredPos, float cameraRotationTime)
    {
        float time = 0;
        Vector3 origCamPos = Camera.main.transform.position;

        while (time < cameraRotationTime)
        {
            Camera.main.transform.position = Vector3.Slerp(origCamPos, desiredPos, time / cameraRotationTime);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public static void EnableTouch()
    {
        if (_multiTouch)
            _multiTouch.enabled = true;
    }

    public static void DisableTouch()
    {
        if (_multiTouch)
            _multiTouch.enabled = false;
    }
}
