using UnityEngine;
using System.Linq;
using System.Collections;

public class Level99Input : MonoBehaviour {

    public Camera Cam;
    public Animator PlayerAnimator;
    public GameObject Player;

    bool _shooting;
    LightningGenerator[] _lightnings;
    Level99EnemySpawnController _spawnController;

    void Start()
    {
        _lightnings = FindObjectsOfType<LightningGenerator>();
        PlayerAnimator.speed = 4;
        _spawnController = FindObjectOfType<Level99EnemySpawnController>();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
            DoRaycast(Input.mousePosition);
    }

    void DoRaycast(Vector2 pos)
    {
        Debug.Log("Doing something");
        RaycastHit hit;
        if (Physics.Raycast(Cam.ScreenPointToRay(pos), out hit, 1000))
        {
            Debug.Log(hit.transform.name);
            if(hit.transform.root.tag == "Wind")
            {
                if (_shooting) return;
                _shooting = true;
                PlayerAnimator.Play("Magic");
                Player.transform.LookAt(hit.transform.position);
                StartCoroutine(Shoot(hit.transform.root.gameObject));
            }
        }
    }

    IEnumerator Shoot(GameObject enemy)
    {

        yield return new WaitForSeconds(0.2f);
        foreach (LightningGenerator l in _lightnings) { l.LightningConductors = enemy.GetComponentsInChildren<Electrified>().Select(b => b.gameObject).ToArray(); l.timePassed = 5; }
        yield return new WaitForSeconds(0.2f);
        foreach (LightningGenerator l in _lightnings) { l.LightningConductors = null; l.lightningConductor = null; }
        _spawnController.KillEnemy(enemy);
        _shooting = false;
    }
}
