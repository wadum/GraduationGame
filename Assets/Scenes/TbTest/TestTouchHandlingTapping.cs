using UnityEngine;

[RequireComponent(typeof(TouchHandling))]
public class TestTouchHandlingTapping : MonoBehaviour {

    public string TagToListenOn = "Player";

    private TouchHandling _input;

	// Use this for initialization
	void Start () {
	    _input = GetComponent<TouchHandling>();
	    if (!_input) {
	        Debug.Log("WHIIIIINE! NO INPUT SYYYYSSSSTEM!");
	        enabled = false;
	        return;
	    }

        _input.RegisterTapHandlerByTag(TagToListenOn, r => ExclaimTap(r.collider.tag));
        _input.RegisterTapAndHoldHandlerByTag(TagToListenOn, r => ExclaimTapAndHold(r.collider.tag));
	}

    private void ExclaimTap(string objTag) {
        Debug.Log("Did a tap on an object with tag " + objTag);
    }
	
    private void ExclaimTapAndHold(string objTag) {
        Debug.Log("Did a tap and hold on an object with tag " + objTag);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
