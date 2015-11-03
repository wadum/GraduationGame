using UnityEngine;
using System.Linq;
using System.Collections;

public class ShadowRecording : MonoBehaviour {

    public int framesToKeep = 30;

    PlayerRecord[] _records;
    int currentFrame = 0;

	// Use this for initialization
	void Start () {
        _records = new PlayerRecord[framesToKeep];
    }
	
	// Update is called once per frame
	void Update () {
        _records[currentFrame % framesToKeep] = new PlayerRecord() { Rot = transform.rotation, Pos = transform.position, Time = Time.time };
        currentFrame++;
    }

    public PlayerRecord[] FormatRecordList(int frameCount)
    {
        PlayerRecord[] temp = new PlayerRecord[Mathf.Min(frameCount, framesToKeep)];

        for(int i = Mathf.Min(frameCount, framesToKeep); i > 0; i--)
        {
            temp[i-1] = _records[(i + currentFrame-1) % framesToKeep];
        }
        temp.Reverse();

        return temp;
    }
}
