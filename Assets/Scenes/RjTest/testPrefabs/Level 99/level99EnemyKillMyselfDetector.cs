using UnityEngine;
using System.Collections;

public class level99EnemyKillMyselfDetector : MonoBehaviour {

    Level99EnemySpawnController _spawnController;

    void Start()
    {
        _spawnController = FindObjectOfType<Level99EnemySpawnController>();
    }

    void OnTriggerEnter(Collider Object)
    {
        if (Object.transform.gameObject.tag == "Rock")
        {
            _spawnController.KillEnemy(this.gameObject.transform.root.gameObject);
        }
    }
}
