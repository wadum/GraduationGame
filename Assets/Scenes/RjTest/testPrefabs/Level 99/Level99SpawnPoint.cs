using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Level99SpawnPoint : MonoBehaviour {

    bool _enemyHere = false;

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Wind")
        {
            _enemyHere = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wind")
        {
            _enemyHere = false;
        }
    }

    public bool AmIFree(List<GameObject> enemies)
    {
        return !enemies.Any(e => Vector3.Distance(transform.position, e.transform.position) < 1);
    }
}
