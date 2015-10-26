using UnityEngine;
using System.Collections;

public class TriggerSound : MonoBehaviour {

    AudioSource sound;

    // Use this for initialization
    void Start () {
        sound = this.gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter (Collider player)
    {

        if (player.tag != "Player")
        {
            return;
        }

        Debug.Log("sound!");
        sound.Play();
    }
}
