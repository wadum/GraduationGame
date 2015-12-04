using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Level99ChainLightning : MonoBehaviour
{
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

    public void StartChainLightning()
    {
		if(ChoosinMenu.activeInHierarchy || _s.AvailableGems < Cost)
			return;
		_s.GemsSpend += Cost;
		Active = true;
		StartCoroutine(DisableAfterSeconds());
    }

	IEnumerator DisableAfterSeconds()
	{
		yield return new WaitForSeconds(ActiveSeconds);
		Active = false;
	}
}
