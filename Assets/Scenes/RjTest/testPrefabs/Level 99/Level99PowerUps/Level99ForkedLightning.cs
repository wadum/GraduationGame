using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level99ForkedLightning : MonoBehaviour {

	public GameObject ChoosinMenu;

	public float ActiveSeconds;
	public static bool Active;

	public int Cost;
	public Text CostText;
	
	Level99EnemySpawnController _s;
	
	void Start()
	{
		CostText.text = Cost.ToString();
		_s = FindObjectOfType<Level99EnemySpawnController>();
	}

	public void StartForkedLightning()
	{
		if(ChoosinMenu.activeInHierarchy || _s.AvailableGems < Cost)
			return;
		_s.GemsSpend += Cost;
		StartCoroutine(DisableAfterSeconds());
		Active = true;
	}
	
	IEnumerator DisableAfterSeconds()
	{
		yield return new WaitForSeconds(ActiveSeconds);
		Active = false;
	}
}
