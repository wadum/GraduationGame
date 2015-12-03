using UnityEngine;
using System.Collections;

public class Level99MotherOfEnemies : MonoBehaviour {

    public Material color;
    public int EnemiesToSpawn = 2;

    GameObject MyEnemy;
    Level99EnemySpawnController Spawner;

    // Use this for initialization
    void Start () {
        MyEnemy = transform.GetComponent<GameObject>();
    }

    void OnDestroy()
    {
        for (int i = 0; i < EnemiesToSpawn; i++)
        {
            Spawner.SpawnEnemyAtPos(this.transform);
        }
    }

}
