using UnityEngine;

public class TestRespawnable : MonoBehaviour
{
	void Start ()
	{
	    var respawnable = GetComponent<Respawnable>();

	    if (!respawnable)
	    {
	        Debug.Log("Missing Respawnable - cannot test");
	        return;
	    }

        MultiTouch.RegisterTapHandlerByTag(tag, t => respawnable.Respawn());
	}
}