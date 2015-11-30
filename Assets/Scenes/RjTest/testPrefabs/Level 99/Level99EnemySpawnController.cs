using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level99EnemySpawnController : MonoBehaviour {

    public int EnemiesSpawned;
    public int EnemiesKilled;
    public GameObject EnemyPrefab;

    public float RespawnTimer = 5f;
    public float TimerDecrease = 0.3f;


    List<GameObject> enemies = new List<GameObject>();
    float _lastSpawn = 0;
    Level99SpawnPoint[] _spawnPoints;
    GameObject _player;

    void Start () {
        _spawnPoints = GetComponentsInChildren<Level99SpawnPoint>();
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("Spawner");
	}

    void Update()
    {
        if(EnemiesSpawned == EnemiesKilled)
        {
            SpawnEnemy();
        }
    }

    IEnumerator Spawner()
    {

        while (true)
        {
            yield return new WaitForSeconds(RespawnTimer/Mathf.Ceil(Mathf.Max(EnemiesKilled,1)/RespawnTimer) + 0.2f);
            SpawnEnemy();
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
        _lastSpawn = Time.time;
    }

    public void KillEnemy (GameObject enemy)
    {
        enemies.Remove(enemy);
        EnemiesKilled++;
        Destroy(enemy);
    }
}
