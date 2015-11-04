using UnityEngine;

public class TestRespawnable : MonoBehaviour
{
	void Start ()
	{
	    var inputSystem = FindObjectOfType<TouchHandling>();
	    var respawnable = GetComponent<Respawnable>();

	    if (!(inputSystem && respawnable))
	    {
	        Debug.Log("Something is wrong, chief.");
	        return;
	    }

        inputSystem.RegisterTapHandlerByTag(tag, t => respawnable.Respawn());
	}
}