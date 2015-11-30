using UnityEngine;
using System.Collections;

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

    public bool AmIFree()
    {
        return !_enemyHere;
    }
}
