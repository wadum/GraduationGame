using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level99Shield : MonoBehaviour {

	public GameObject ChoosinMenu;

    public static bool Shield;
    public lvl99ShieldController level99Shield;

	public int Cost;
	public Text CostText;
	
	Level99EnemySpawnController _s;
	
	void Start()
	{
		CostText.text = Cost.ToString();
		_s = FindObjectOfType<Level99EnemySpawnController>();
	}

    public void StartShield()
    {
		if(ChoosinMenu.activeInHierarchy || _s.AvailableGems < Cost)
			return;
		_s.GemsSpend += Cost;
        Shield = true;
        StartCoroutine(ShieldDuration());
		GetComponent<Image>().color = new Color(0xAC/255f, 0xFF/255f, 0xB0/255f, 0xFF/255f);
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
		GetComponent<Image>().color = Color.white;
    }
}
