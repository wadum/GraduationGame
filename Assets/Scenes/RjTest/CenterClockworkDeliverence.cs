using UnityEngine;
using System.Collections;

public class CenterClockworkDeliverence : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider player)
    {

        if (player.tag != "Player")
        {
            return;
        }
        player.GetComponent<CharacterInventory>().Deliver(true);
    }

    void OnTriggerExit(Collider player)
    {

        if (player.tag != "Player")
        {
            return;
        }
        player.GetComponent<CharacterInventory>().Deliver(false);
    }
}
