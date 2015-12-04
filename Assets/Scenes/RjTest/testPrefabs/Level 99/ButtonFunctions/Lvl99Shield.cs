using UnityEngine;
using System.Collections;

public class Lvl99Shield : MonoBehaviour {

	public GameObject ChoosinMenu;

    public static bool Shield;
    public lvl99ShieldController Level99Shield;

    public void StartShield()
    {
		if(ChoosinMenu.activeInHierarchy)
			return;
        Shield = true;
        StartCoroutine(ShieldDuration());
    }
    IEnumerator ShieldDuration()
    {
        Level99Shield.ActivateShield();
        yield return new WaitForSeconds(0.5f);
        Level99RotateAroundPosition[] particles = Level99Shield.BigShieldState.GetComponentsInChildren<Level99RotateAroundPosition>();
        foreach (Level99RotateAroundPosition ParticlePart in particles)
            ParticlePart.enabled = true;
        yield return new WaitForSeconds(Level99UIController.ShieldLevel);
        Level99Shield.DeactivateShield();
    }
}
