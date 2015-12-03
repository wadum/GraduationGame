using UnityEngine;
using System.Collections;

public class Level99ForkedLightning : MonoBehaviour {

	public float ActiveSeconds;
	public static bool Active;
	
	public void StartForkedLightning()
	{
		StartCoroutine(DisableAfterSeconds());
		Active = true;
	}
	
	IEnumerator DisableAfterSeconds()
	{
		yield return new WaitForSeconds(ActiveSeconds);
		Active = false;
	}
}
