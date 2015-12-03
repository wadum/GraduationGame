using UnityEngine;
using System.Collections;

public class Lvl99Shield : MonoBehaviour {

    public static bool Shield;
    public lvl99ShieldController Level99Shield;

    public void StartShield()
    {
        Shield = true;
        StartCoroutine(ShieldDuration());
    }
    IEnumerator ShieldDuration()
    {
        Level99Shield.ActivateShield();
        yield return new WaitForSeconds(Level99UIController.ShieldLevel);
        Level99Shield.DeactivateShield();
    }
}
