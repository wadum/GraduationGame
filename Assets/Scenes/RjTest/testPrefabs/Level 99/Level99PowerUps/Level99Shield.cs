using UnityEngine;
using System.Collections;

public class Level99Shield : MonoBehaviour {

    public static bool Shield;
    public lvl99ShieldController level99Shield;

    public void StartShield()
    {
        Shield = true;
        StartCoroutine(ShieldDuration());
    }
    IEnumerator ShieldDuration()
    {
        level99Shield.ActivateShield();
        yield return new WaitForSeconds(0.5f);
        Level99RotateAroundPosition[] particles = level99Shield.BigShieldState.GetComponentsInChildren<Level99RotateAroundPosition>();
        foreach (Level99RotateAroundPosition ParticlePart in particles)
            ParticlePart.enabled = true;
        yield return new WaitForSeconds(Level99UIController.ShieldLevel);
        level99Shield.DeactivateShield();
    }
}
