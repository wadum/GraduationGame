using UnityEngine;

public class LoadNextAfterTime : MonoBehaviour
{
    public int timeToWait = 2;
    private float _time = 0.0f;

    void Update()
    {
        _time += Time.deltaTime;
        if (_time > timeToWait)
            Application.LoadLevel(Application.loadedLevel + 1);
    }
}
