using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class Tutorial7TapAndHoldEnd : TutorialStep {

    private GameObject _player;

    protected override bool Continue()
    {
        return GameObject.FindGameObjectWithTag("Player").transform.parent != null && GameObject.FindGameObjectWithTag("Player").transform.parent.root.tag == "Moveable Rock";
    }

    override public IEnumerator Run()
    {
        //_player = GameObject.FindGameObjectWithTag("Player");
		yield return StartCoroutine(base.Run());
	}
}
