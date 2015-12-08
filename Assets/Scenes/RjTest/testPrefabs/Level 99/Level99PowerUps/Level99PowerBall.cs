using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level99PowerBall : MonoBehaviour {

	public GameObject ChoosinMenu;

    public static bool PowerBall;

	public int Cost;
	public Text CostText;
	
	Level99EnemySpawnController _s;
	
	void Start()
	{
		CostText.text = Cost.ToString();
		_s = FindObjectOfType<Level99EnemySpawnController>();
	}

    public void StartPowerball()
    {
		if(ChoosinMenu.activeInHierarchy || _s.AvailableGems < Cost)
			return;
		_s.GemsSpend += Cost;
        PowerBall = true;
        StartCoroutine(PowerBallDuration());
		GetComponent<Image>().color = new Color(0xAC/255f, 0xFF/255f, 0xB0/255f, 0xFF/255f);
    }
    IEnumerator PowerBallDuration()
    {
        while (PowerBall)
        {
            yield return null;
        }
		GetComponent<Image>().color = Color.white;
    }
}
