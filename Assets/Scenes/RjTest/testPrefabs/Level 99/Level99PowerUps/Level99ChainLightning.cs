using UnityEngine;
using System.Collections;
using System.Linq;

public class Level99ChainLightning : MonoBehaviour
{
	public GameObject ChoosinMenu;

	public float ActiveSeconds;
    public static bool Active;

    public void StartChainLightning()
    {
		if(ChoosinMenu.activeInHierarchy)
			return;
		Active = true;
		StartCoroutine(DisableAfterSeconds());
    }

	IEnumerator DisableAfterSeconds()
	{
		yield return new WaitForSeconds(ActiveSeconds);
		Active = false;
	}
}
