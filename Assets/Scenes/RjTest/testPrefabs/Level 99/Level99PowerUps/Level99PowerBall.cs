using UnityEngine;
using System.Collections;

public class Level99PowerBall : MonoBehaviour {

    public static bool PowerBall;

    public void StartPowerball()
    {
        PowerBall = true;
        StartCoroutine(PowerBallDuration());
    }
    IEnumerator PowerBallDuration()
    {
        while (PowerBall)
        {
            yield return null;
        }
    }
}
