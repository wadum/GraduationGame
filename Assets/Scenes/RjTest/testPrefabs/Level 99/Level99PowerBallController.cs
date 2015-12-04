using UnityEngine;
using System.Collections;

public class Level99PowerBallController : MonoBehaviour
{
    public float RadiusToAdd;

    public Level99EnemySpawnController SpawnController;

    bool ClashedWithGround;

    void Start()
    {

    }

    public IEnumerator PowerBallDuration()
    {
        yield return new WaitForSeconds(1);

        CapsuleCollider[] GlassPieces = GetComponentsInChildren<CapsuleCollider>();

        foreach(CapsuleCollider GlassPiece in GlassPieces)
        {
            GlassPiece.radius += RadiusToAdd;
        }

        yield return new WaitForSeconds(1);

        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider Object)
    {
        if (Object.transform.gameObject.tag == "Wind")
        {
            SpawnController.KillEnemy(Object.gameObject);
        }
    }
}
