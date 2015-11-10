using UnityEngine;
using System.Collections;

public class AdjustVolumeCameraDistance : MonoBehaviour {

	public AudioSource Audio;

	private float origVolume;
	
	void Start()
	{
		origVolume = Audio.volume;
	}

	void Update()
	{
		Audio.volume = origVolume * 
			(1-(Vector3.Distance(Audio.transform.position, Camera.main.transform.position) - Audio.minDistance) 
			 / (Audio.maxDistance - Audio.minDistance));
	}
}
