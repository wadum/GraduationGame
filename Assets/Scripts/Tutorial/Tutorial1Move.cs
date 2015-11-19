using UnityEngine;
using System.Collections;

public class Tutorial1Move : TutorialStep
{
    bool done;

    override public IEnumerator Run()
    {
        // We allow the player to move freely.. For one click, then the player is frozen again.
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().TutorialMoveFreeze = false;
        // The TapHandler will still remember this command, even after the tutorial is destroyed, so I'm making sure that it only actually does something once (when it's destroyed the bool is always false (null)).
        MultiTouch.RegisterTapHandlerByTag("Terrain", hit => { if (done) return; done = true;  GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>().TutorialMoveFreeze = true; Completed = true; });

        yield return StartCoroutine(base.Run());
    }
}