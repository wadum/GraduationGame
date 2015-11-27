using UnityEngine;
using System.Collections;
using System;

public class Tutorial1Move : TutorialStep
{
    Func<RaycastHit, bool> handler = null;
    private GameObject _player;
    public Transform MoveToPoint;
    public BaseDynamicCameraAI dynamicCameraAi;

    override public IEnumerator Run()
    {
        handler = MultiTouch.RegisterTapHandlerByTag("Terrain", hit => { Completed = true; return true; });

        yield return StartCoroutine(base.Run());
        _player = GameObject.FindGameObjectWithTag("Player");
        _player.GetComponent<NavMeshAgent>().SetDestination(MoveToPoint.position);
        dynamicCameraAi.AssumeDirectControl();
        MultiTouch.RemoveSpecificTapHandler(handler);

        while (_player.GetComponent<NavMeshAgent>().remainingDistance > 0.001f)
        {
            yield return null;
        }
    }

    void OnDestroy()
    {
        if(handler != null)
            MultiTouch.RemoveSpecificTapHandler(handler);
    }
}