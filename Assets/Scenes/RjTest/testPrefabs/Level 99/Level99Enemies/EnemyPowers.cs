using UnityEngine;
using System.Linq;
using System.Collections;

public class EnemyPowers : MonoBehaviour {

    public GameObject Lightnings;

    Level99EnemySpawnController _spawnController;

    void Start () {
        _spawnController = FindObjectOfType<Level99EnemySpawnController>();
    }

    private float DistanceTo(GameObject obj)
    {
        return Vector3.Distance(obj.transform.position, transform.position);
    }

    private GameObject SmallestDistance(GameObject obj1, GameObject obj2)
    {
        if (obj1.GetComponentInChildren<Electrified>().isActiveAndEnabled) return obj2;
        return DistanceTo(obj1) < DistanceTo(obj2) ? obj1 : obj2;
    }

    public IEnumerator ChainShoot(int chainNumber)
    {
        if (chainNumber < Level99UIController.ChainLevel)
        {
            yield return new WaitForSeconds(0.2f);
            yield break;
        }

        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Wind");

        GameObject enemy = Enemies.Aggregate(SmallestDistance);
        if (!enemy || enemy.transform.root == transform.root)
        {
            yield return new WaitForSeconds(0.2f);
            yield break;
        }
        Lightnings.SetActive(true);
        foreach (LightningGenerator l in Lightnings.GetComponentsInChildren<LightningGenerator>()) { l.LightningConductors = enemy.GetComponentsInChildren<Electrified>().Select(b => b.gameObject).ToArray(); l.timePassed = 5; }
        yield return StartCoroutine(enemy.GetComponent<EnemyPowers>().ChainShoot(chainNumber + 1));
        yield return new WaitForSeconds(0.1f);
        foreach (LightningGenerator l in Lightnings.GetComponentsInChildren<LightningGenerator>()) { l.LightningConductors = null; l.lightningConductor = null; }
        _spawnController.KillEnemy(enemy);
    }
}
