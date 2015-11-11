using UnityEngine;
using System.Collections;

public class CenterClockworkDeliverence : MonoBehaviour {

    Cockpart[] cocks;
    public int turnedin;
	// Use this for initialization
	void Start () {
        cocks = GameObject.FindObjectsOfType<Cockpart>();
	}
	
	// Update is called once per frame
	void Update () {

        if (cocks.Length == turnedin)
        {
            SaveLoad.saveLoad.Reset();
            Application.LoadLevel("Main Menu");
        }
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
