using UnityEngine;
using System.Linq;
using System.Collections;

public class Level99Input : MonoBehaviour {

    public Camera Cam;
    public Animator PlayerAnimator;
    public GameObject Player;
    public CapsuleCollider DeathDetection;

    public GameObject[] Overlays;

    bool _shooting;
    LightningGenerator[] _lightnings;
    Level99EnemySpawnController _spawnController;

    void Start()
    {
        _lightnings = Player.GetComponentsInChildren<LightningGenerator>();
        PlayerAnimator.speed = 4;
        _spawnController = FindObjectOfType<Level99EnemySpawnController>();
    }

	void Update () {
        if (Input.GetMouseButtonDown(0) && DeathDetection.enabled)
            DoRaycast(Input.mousePosition);
    }

    void DoRaycast(Vector2 pos)
    {
        if (Overlays.Any(o => o.activeInHierarchy)) return;
        RaycastHit hit;
        if (Physics.Raycast(Cam.ScreenPointToRay(pos), out hit, 1000))
        {
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
        if (Level99ChainLightning.ChainLightning)
            yield return StartCoroutine(enemy.GetComponent<EnemyPowers>().ChainShoot(Level99UIController.ChainLevel));
        else yield return new WaitForSeconds(0.2f);
        foreach (LightningGenerator l in _lightnings) { l.LightningConductors = null; l.lightningConductor = null; }
        
        _spawnController.KillEnemy(enemy);
        _shooting = false;
    }
}
