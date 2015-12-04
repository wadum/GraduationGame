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
		GetComponent<Image>().color = new Color(0xAC/255f, 0xFF/255f, 0xB0/255f, 0xFF/255f);
		StartCoroutine(DisableAfterSeconds());
    }

	IEnumerator DisableAfterSeconds()
	{
		yield return new WaitForSeconds(ActiveSeconds);
		GetComponent<Image>().color = Color.white;
		Active = false;
	}
}
