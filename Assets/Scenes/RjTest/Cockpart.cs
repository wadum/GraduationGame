using UnityEngine;
using System.Collections;

public class Cockpart : MonoBehaviour {

    GameObject player;

    bool pickedUp = false;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");

    }
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(Vector3.up,100*Time.deltaTime);

        if (pickedUp)
        {
            transform.position = player.transform.position;
        }

    }

    void OnTriggerEnter(Collider player)
    {

        if (player.tag != "Player")
        {
            return;
        }

        pickedUp = true;

    }
}
