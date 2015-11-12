using UnityEngine;
using System.Collections;

public class CenterClockworkDeliverence : MonoBehaviour {

    Clockpart[] clocks;
    public int turnedin;
	// Use this for initialization
	void Start () {
        clocks = GameObject.FindObjectsOfType<Clockpart>();
	}
	
	// Update is called once per frame
	void Update () {

        if (clocks.Length == turnedin)
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
