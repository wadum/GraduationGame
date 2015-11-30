using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class PlayerPrefsControl : MonoBehaviour
{

    public static List<string> IntKeys = new List<string>(){ TutorialController.PlayerPrefAlreadySeen, StoreScreenController.PurchasedLevels, TutorialControllerLevel2.PlayerPrefAlreadySeen };
	public static List<string> FloatKeys = new List<string>() { "masterVol", "sfxVol", "musicVol" };
    public static List<string> StringKeys = new List<string>() { "LastLevel" };

    [MenuItem("PlayerPrefs/Delete All")]
    public static void DeletePlayePrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("PlayerPrefs/Print All")]
    public static void PrintPlayePrefs()
    {
        IntKeys.ForEach(k => {
            Debug.Log(string.Format("Key: {0}\nValue: {1}", k, PlayerPrefs.HasKey(k) ? PlayerPrefs.GetInt(k).ToString() : "Not Set"));
        });
		FloatKeys.ForEach(k => {
			Debug.Log(string.Format("Key: {0}\nValue: {1}", k, PlayerPrefs.HasKey(k) ? PlayerPrefs.GetInt(k).ToString() : "Not Set"));
		});
        StringKeys.ForEach(k => {
        	Debug.Log(string.Format("Key: {0}\nValue: {1}", k, PlayerPrefs.HasKey(k) ? PlayerPrefs.GetString(k) : "Not Set"));
        });
    }
}
