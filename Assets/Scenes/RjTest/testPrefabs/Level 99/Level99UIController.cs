using UnityEngine;
using System.Collections;

public class Level99UIController : MonoBehaviour {

    public static int ChainLevel = 2;

    public GameObject DeathScreen;
    public GameObject PowerUpChoosinScreen;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Play()
    {
        FindObjectOfType<Level99EnemySpawnController>().Reset();
        FindObjectOfType<Level99GameOverDetector>().EnableDeathCollider(true);
    }
}
