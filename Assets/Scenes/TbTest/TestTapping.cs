using UnityEngine;

[RequireComponent(typeof(MultiTouch))]
public class TestTapping : MonoBehaviour {

    public string TagToListenOn = "Player";

	// Use this for initialization
	void Start () {
		MultiTouch.RegisterTapHandlerByTag(TagToListenOn, r => { ExclaimTap(r.collider.tag); return true; });
		MultiTouch.RegisterTapAndHoldHandlerByTag(TagToListenOn, r => { ExclaimTapAndHold(r.collider.tag); return true; });
	}

    private static void ExclaimTap(string objTag) {
        Debug.Log("Did a tap on an object with tag " + objTag);
    }
	
    private static void ExclaimTapAndHold(string objTag) {
        Debug.Log("Did a tap and hold on an object with tag " + objTag);
    }
}
