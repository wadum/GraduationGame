using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class PlayerPrefsControl : MonoBehaviour
{

    public static List<string> IntKeys = new List<string>(){ TutorialController.PlayerPrefAlreadySeen };
    public static List<string> StringKeys = new List<string>() {  };

    [MenuItem("PlayerPrefs/Delete All")]
    public static void DeletePlayePrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("PlayerPrefs/Print All")]
    public static void PrintPlayePrefs()
    {
        IntKeys.ForEach(k => {
            if(PlayerPrefs.HasKey(k))
                Debug.Log(string.Format("Key: {0}\nValue: {1}", k, PlayerPrefs.GetInt(k)));
        });
        StringKeys.ForEach(k => {
            if (PlayerPrefs.HasKey(k))
                Debug.Log(string.Format("Key: {0}\nValue: {1}", k, PlayerPrefs.GetInt(k)));
        });
    }
}
