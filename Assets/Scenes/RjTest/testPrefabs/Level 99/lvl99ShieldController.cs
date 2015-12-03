using UnityEngine;
using System.Collections;

public class lvl99ShieldController : MonoBehaviour {

    public GameObject SmallShieldState;
    public GameObject BigShieldState;

    public Level99EnemySpawnController SpawnController;

    public void ActivateShield()
    {
        SmallShieldState.SetActive(false);
        BigShieldState.SetActive(true);
    }
    public void DeactivateShield()
    {
        BigShieldState.SetActive(false);
        SmallShieldState.SetActive(true);
    }

    void OnTriggerEnter(Collider Object)
    {
        if(Object.transform.gameObject.tag == "Wind")
        {
            SpawnController.KillEnemy(Object.gameObject);
        }
    }
}
