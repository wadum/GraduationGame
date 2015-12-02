using UnityEngine;

public class ScreenSleep : MonoBehaviour
{
    void Start()
    {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

	void OnDestroy()
	{
		Screen.sleepTimeout = SleepTimeout.SystemSetting;
	}
}
