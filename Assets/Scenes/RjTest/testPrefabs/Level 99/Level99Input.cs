using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Level99Input : MonoBehaviour {

    public Camera Cam;
    public Animator PlayerAnimator;
    public GameObject Player;
	public List<GameObject> LightningGenerators;
    public CapsuleCollider DeathDetection;

    public GameObject[] Overlays;

    public GameObject PowerBallPrefab;

    bool _shooting;
    Level99EnemySpawnController _spawnController;

    void Start()
    {
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
                if (Level99ForkedLightning.Active)
                {
                    GameObject[] es = _spawnController.enemies
                        .OrderBy(e => Vector3.Distance(e.transform.position, hit.transform.root.gameObject.transform.position))
                            .Take(Mathf.Min(3, _spawnController.enemies.Count)).ToArray();
                    List<GameObject> ls = GetLightningGenerators(Mathf.Min(3, _spawnController.enemies.Count));
                    for (int i = 0; i < Mathf.Min(3, _spawnController.enemies.Count); i++)
                    {
                        StartCoroutine(Shoot(es[i], ls[i]));
                    }
                    //If we have activated the powerball power spawn a powerball
                    if (Level99PowerBall.PowerBall)
                    {
                        GameObject Ball = (GameObject)Instantiate(PowerBallPrefab, es[0].transform.position + Vector3.up * 5, Quaternion.identity);
                        StartCoroutine(Ball.GetComponent<Level99PowerBallController>().PowerBallDuration());
                        Level99PowerBall.PowerBall = false;
                    }
                }
                else
                {
                    //If we have activated the powerball power spawn a powerball
                    if (Level99PowerBall.PowerBall)
                    {
                        GameObject Ball = (GameObject)Instantiate(PowerBallPrefab, hit.transform.root.gameObject.transform.position + Vector3.up * 5, Quaternion.identity);
                        StartCoroutine(Ball.GetComponent<Level99PowerBallController>().PowerBallDuration());
                        Level99PowerBall.PowerBall = false;
                    }
                    StartCoroutine(Shoot(hit.transform.root.gameObject));
                }
            }
        }
    }

	private List<GameObject> GetLightningGenerators(int amount)
	{
		if(amount > LightningGenerators.Count)
		{
			int missing = amount - LightningGenerators.Count;
			for (int i = 0; i < missing; i++){
				GameObject l = (GameObject) Instantiate(LightningGenerators[0], LightningGenerators[0].transform.position, Quaternion.identity);
				LightningGenerators.Add(l);
			}
		}
		return LightningGenerators.Take(amount).ToList();
	}

	IEnumerator Shoot (GameObject enemy)
	{
		return Shoot(enemy, LightningGenerators[0]);
	}

    IEnumerator Shoot(GameObject enemy, GameObject lightning)
    {
        LightningGenerator[] _lightnings = lightning.GetComponentsInChildren<LightningGenerator>();
        yield return new WaitForSeconds(0.2f);
        foreach (LightningGenerator l in _lightnings) { l.LightningConductors = enemy.GetComponentsInChildren<Electrified>().Select(b => b.gameObject).ToArray(); l.timePassed = 5; }
        if (Level99ChainLightning.Active)
            yield return StartCoroutine(enemy.GetComponent<EnemyPowers>().ChainShoot(Level99UIController.ChainLevel));
        else yield return new WaitForSeconds(0.2f);
        foreach (LightningGenerator l in _lightnings) { l.LightningConductors = null; l.lightningConductor = null; }
        
        _spawnController.KillEnemy(enemy);
        _shooting = false;
    }
}
