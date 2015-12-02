using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Level99SpawnPoint : MonoBehaviour {

    public bool AmIFree(List<GameObject> enemies)
    {
        return !enemies.Any(e => Vector3.Distance(transform.position, e.transform.position) < 1);
    }
}
