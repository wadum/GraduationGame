using UnityEngine;

public class ScreenSleep : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Screen.sleepTimeout = 300; // 5 minutes, I guess...
    }
}
