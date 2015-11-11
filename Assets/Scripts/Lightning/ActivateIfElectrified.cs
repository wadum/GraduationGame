using UnityEngine;
using System.Linq;

public class ActivateIfElectrified : MonoBehaviour {

    public GameObject ImActive;
    ElectrifiedBridge[] On;

    void Awake()
    {
        On = transform.root.GetComponentsInChildren<ElectrifiedBridge>();
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
