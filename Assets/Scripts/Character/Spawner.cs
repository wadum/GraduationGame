using UnityEngine;

public class Spawner : MonoBehaviour {

    Vector3 _camstart;
    bool _respawning = false;

	void Start () {
        GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Respawnable>().SetCurrentLocationAsRespawn();
        Respawn();
	}

    public void Respawn()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Respawnable>().Respawn();
        _respawning = true;
        FindObjectOfType<GameOverlayController>().DeactivateSlider();

        foreach(var sidekick in FindObjectsOfType<SidekickElementController>())
            sidekick._myStatus = SidekickElementController.Status.FlyBack;
    }

    void Update()
    {
        if(_respawning)
            _respawning = false;

        if (Input.GetKeyDown(KeyCode.Q))
            Respawn();
    }
}
