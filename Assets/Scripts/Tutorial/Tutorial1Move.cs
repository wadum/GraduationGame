using UnityEngine;
using System.Collections;
using System;

public class Tutorial1Move : TutorialStep
{
    Func<RaycastHit, bool> handler = null;
    private GameObject _player;
    public Transform MoveToPoint;

    override public IEnumerator Run()
    {
        handler = MultiTouch.RegisterTapHandlerByTag("Terrain", hit => { Completed = true; return true; });

        yield return StartCoroutine(base.Run());
        _player = GameObject.FindGameObjectWithTag("Player");
        _player.GetComponent<NavMeshAgent>().SetDestination(MoveToPoint.position);
        MultiTouch.RemoveSpecificTapHandler(handler);
    }

    void OnDestroy()
    {
        if(handler != null)
            MultiTouch.RemoveSpecificTapHandler(handler);
    }
}