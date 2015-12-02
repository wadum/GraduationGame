using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialControllerLevel2 : MonoBehaviour
{

    public bool ForceTutorial = false;
    public bool DisableTutorial = true;
    public GameObject SceneViews;

    public List<TutorialStep> Steps;
    public List<GameObject> ObjectsToDestroy;
    public List<GameObject> ObjectsToToggle;

    private static DynamicCamera _camera;
    public BaseDynamicCameraAI dynamicCameraAi;

    public const string PlayerPrefAlreadySeen = "TutorialAlreadySeen2";

    void Awake()
    {
        _camera = FindObjectOfType<DynamicCamera>();
        if ((PlayerPrefs.GetInt(PlayerPrefAlreadySeen) > 0 || DisableTutorial || Application.loadedLevelName != "lvl2") &&
            !ForceTutorial)
            Destroy(gameObject);
        else {
            ObjectsToToggle.ForEach(obj => { if (obj) obj.SetActive(false); });
            FindObjectsOfType<AutoStartDynamicCameraAI>().ToList().ForEach(Destroy);
        }
    }

    void Start()
    {
        if(SceneViews)
            SceneViews.SetActive(false);
        StartCoroutine(RunAllSteps());
    }

    IEnumerator RunAllSteps()
    {
        // We take control of the camera, so we turn off the built-in AI.
        _camera.Stop();
        _camera.StopAllCoroutines();

        DisableTouch();
        // Only once a tutorial start will we freeze the player.
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().TutorialMoveFreeze = true;
        foreach (var step in Steps)
        {
            yield return StartCoroutine(step.Run());
        }

        PlayerPrefs.SetInt(PlayerPrefAlreadySeen, 1);
        // When the tutorial is done the player is free to move around.
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().TutorialMoveFreeze = false;

        //We activate te sceneViews within he game.
        if (SceneViews)
            SceneViews.SetActive(true);

        // We yield control to the camera built-in AI.
        dynamicCameraAi.AssumeDirectControl();
        EnableTouch();
        //Destroy the tutorial
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        EnableTouch();
        ObjectsToToggle.ForEach(obj => { if (obj) obj.SetActive(true); });
        ObjectsToDestroy.ForEach(obj => { if (obj) Destroy(obj); });
    }

    public static void EnableTouch()
    {
        if(MultiTouch.Instance)
            MultiTouch.Instance.enabled = true;
    }

    public static void DisableTouch()
    {
        if (MultiTouch.Instance)
            MultiTouch.Instance.enabled = false;
    }
}
