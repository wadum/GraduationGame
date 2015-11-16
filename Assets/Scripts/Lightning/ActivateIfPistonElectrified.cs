using UnityEngine;
using System.Linq;

public class ActivateIfPistonElectrified : MonoBehaviour
{

    public GameObject ImActive;
    ElectrifiedElevator[] On;

    void Awake()
    {
        On = transform.root.GetComponentsInChildren<ElectrifiedElevator>();
    }

    void Update()
    {
        if (On.Any(b => b.Active))
        {
            ImActive.gameObject.SetActive(true);
        }
        else
        {
            ImActive.gameObject.SetActive(false);
        }
    }
}
