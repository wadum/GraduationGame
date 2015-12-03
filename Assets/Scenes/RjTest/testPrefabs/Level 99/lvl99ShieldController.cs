using UnityEngine;
using System.Collections;

public class lvl99ShieldController : MonoBehaviour {

    public GameObject SmallShieldState;
    public GameObject BigShieldState;

    public Level99EnemySpawnController SpawnController;

    CapsuleCollider EnemyKillingCollider;

    void Start()
    {
        EnemyKillingCollider = GetComponent<CapsuleCollider>();
    }

    public void ActivateShield()
    {
        SmallShieldState.SetActive(false);
        BigShieldState.SetActive(true);
        EnemyKillingCollider.enabled = true;
    }
    public void DeactivateShield()
    {
        BigShieldState.SetActive(false);
        EnemyKillingCollider.enabled = false;
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
