using UnityEngine;

[RequireComponent(typeof(MultiTouch))]
public class TestTapping : MonoBehaviour {

    public string TagToListenOn = "Player";

    private MultiTouch _input;

	// Use this for initialization
	void Start () {
        MultiTouch.RegisterTapHandlerByTag(TagToListenOn, r => ExclaimTap(r.collider.tag));
        MultiTouch.RegisterTapAndHoldHandlerByTag(TagToListenOn, r => ExclaimTapAndHold(r.collider.tag));
	}

    private static void ExclaimTap(string objTag) {
        Debug.Log("Did a tap on an object with tag " + objTag);
    }
	
    private static void ExclaimTapAndHold(string objTag) {
        Debug.Log("Did a tap and hold on an object with tag " + objTag);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
