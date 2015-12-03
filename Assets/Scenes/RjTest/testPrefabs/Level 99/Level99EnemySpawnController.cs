using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Level99EnemySpawnController : MonoBehaviour {

    public int EnemiesSpawned;
    private int _enemiesKilled;
    public int EnemiesKilled {
        get { return _enemiesKilled; } set { CurrentGems.text = value.ToString(); _enemiesKilled = value; } }
    public GameObject EnemyPrefab;

    public float RespawnTimer = 5f;

    public CapsuleCollider DeathDetection;

    float _backupRespawnTimer;
    public List<GameObject> enemies = new List<GameObject>();
    Level99SpawnPoint[] _spawnPoints;
    GameObject _player;

    public Text CurrentGems;

    void Start () {
        _spawnPoints = GetComponentsInChildren<Level99SpawnPoint>();
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("Spawner");
        _backupRespawnTimer = RespawnTimer;
    }

    void Update()
    {
        if(EnemiesSpawned == EnemiesKilled && DeathDetection.enabled)
        {
            SpawnEnemy();
        }
    }

    IEnumerator Spawner()
    {

        while (true)
        {
            yield return new WaitForSeconds(RespawnTimer / Mathf.Ceil(Mathf.Max(EnemiesKilled, 1) / RespawnTimer) + 0.2f);
            if (DeathDetection.enabled)
            {
                SpawnEnemy();
            }
            else
            {
                yield return null;
            }
        }
    }

    void SpawnEnemy()
    {
        int spawnPos = Random.Range(0,_spawnPoints.Length-1);

        // Enable this to make sure enemies does not spawn on top of each other.

        /*int initPos = spawnPos;
        while (!_spawnPoints[spawnPos].AmIFree(enemies))
        {
            spawnPos = (spawnPos + 1) % _spawnPoints.Length;
            if (initPos == spawnPos)
                return;
        }*/
        EnemiesSpawned++;
        GameObject Enemy = (GameObject) Instantiate(EnemyPrefab, _spawnPoints[spawnPos].transform.position, Quaternion.identity);
        enemies.Add(Enemy);
        Enemy.GetComponent<NavMeshAgent>().SetDestination(_player.transform.position);
    }

    public void SpawnEnemyAtPos(Transform Pos)
    {
        // Enable this to make sure enemies does not spawn on top of each other.

        /*int initPos = spawnPos;
        while (!_spawnPoints[spawnPos].AmIFree(enemies))
        {
            spawnPos = (spawnPos + 1) % _spawnPoints.Length;
            if (initPos == spawnPos)
                return;
        }*/
        EnemiesSpawned++;
        GameObject Enemy = (GameObject)Instantiate(EnemyPrefab, Pos.position, Quaternion.identity);
        enemies.Add(Enemy);
        Enemy.GetComponent<NavMeshAgent>().SetDestination(_player.transform.position);
    }

    public void KillEnemy (GameObject enemy)
    {
        enemies.Remove(enemy);
        EnemiesKilled++;
        Destroy(enemy);
    }

    public void EnemiesCelebrate()
    {
        enemies.ForEach(e => { e.GetComponent<NavMeshAgent>().ResetPath(); e.GetComponentInChildren<Animator>().Play("JumpUpTransition"); });
    }

    public void Reset()
    {
        EnemiesSpawned = 0;
        EnemiesKilled = 0;
        RespawnTimer = _backupRespawnTimer;
        enemies.ForEach(e => { Destroy(e); });
        enemies = new List<GameObject>();
    }
}
