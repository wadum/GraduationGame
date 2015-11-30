using UnityEngine;
using System.Collections;

public class Level99EnemySpawnController : MonoBehaviour {

    
    public GameObject EnemyPrefab;
    public float RespawnTimer = 5;

    float _lastSpawn = 0;
    Level99SpawnPoint[] _spawnPoints;
    GameObject _player;

    void Start () {
        _spawnPoints = GetComponentsInChildren<Level99SpawnPoint>();
        _player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("Spawner");
	}

    IEnumerator Spawner()
    {

        while (true)
        {
            yield return new WaitForSeconds(RespawnTimer);
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        int spawnPos = Random.Range(0,_spawnPoints.Length-1);
        int initPos = spawnPos;
        while (!_spawnPoints[spawnPos].AmIFree())
        {
            spawnPos = (spawnPos + 1) % _spawnPoints.Length;
            if (initPos == spawnPos)
                return;
        }
        GameObject Enemy =(GameObject) Instantiate(EnemyPrefab, _spawnPoints[spawnPos].transform.position, Quaternion.identity);
        Enemy.GetComponent<NavMeshAgent>().SetDestination(_player.transform.position);
        _lastSpawn = Time.time;
    }
}
