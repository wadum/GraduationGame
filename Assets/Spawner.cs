using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    Vector3 camstart;
    bool respawning = false;

	void Start () {
        GameObject.FindGameObjectWithTag("Player").transform.position = this.transform.position;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Respawnable>().SetCurrentLocationAsRespawn();
        Respawn();
	}

    public void Respawn()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Respawnable>().Respawn();
        respawning = true;
        GameObject.FindObjectOfType<GameOverlayController>().DeactivateSlider();
        foreach(SidekickElementController sidekick in GameObject.FindObjectsOfType<SidekickElementController>())
        {
            sidekick._myStatus = SidekickElementController.Status.FlyBack;
        }
    }

    void Update()
    {
        if(respawning)
        {
            respawning = false;
            Camera.main.transform.position = GameObject.FindObjectOfType<TopDownCamController>().StartingPos;
        }

        if (Input.anyKey)
        {
            if (Input.GetKeyDown(KeyCode.Q)) { Respawn(); }
        }
    }
}
