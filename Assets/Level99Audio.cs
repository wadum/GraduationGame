using UnityEngine;
using System.Collections;

public class Level99Audio : MonoBehaviour {

	public AudioSource Low,Medium,Fast;

	Level99EnemySpawnController _s;

	void Start()
	{
		_s = FindObjectOfType<Level99EnemySpawnController>();
	}

	void Update ()
	{
		if(_s.enemies.Count < 10)
		{
			if(Low.isPlaying)return;
			if(Medium.isPlaying){
				Medium.loop = false;
				return;
			}
			Low.Play();
			Low.loop = true;
			Medium.Stop();
			Fast.Stop();
		} else if(_s.enemies.Count < 25)
		{
			if(Medium.isPlaying)return;
			if(Low.isPlaying){
				Low.loop = false;
				return;
			}
			if(Fast.isPlaying){
				Fast.loop = false;
				return;
			}
			Low.Stop();
			Medium.Play();
			Medium.loop = true;
			Fast.Stop();
		} else
		{
			if(Fast.isPlaying)return;
			if(Medium.isPlaying){
				Medium.loop = false;
				return;
			}
			Low.Stop();
			Medium.Stop();
			Fast.loop = true;
			Fast.Play();
		}
	}
}
