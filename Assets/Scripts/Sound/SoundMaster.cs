using UnityEngine;
using System.Collections;

public class SoundMaster : MonoBehaviour {

	public AudioSource Ambience, AmbienceReversed;

	void Start()
	{
		if (Mathf.Abs (Ambience.clip.length - AmbienceReversed.clip.length) > 0.1f)
			Debug.Log ("Ambience music tracks are not the same length!");
	}

	public void PlayAmbience()
	{
		var playbackPos = Ambience.clip.length - AmbienceReversed.time;
		AmbienceReversed.Stop();	
		Ambience.time = playbackPos;
		Ambience.Play();
	}

	public void PlayAmbienceReversed()
	{
		var playbackPos = Mathf.Abs((AmbienceReversed.clip.length - Ambience.time)%Ambience.clip.length);
		Ambience.Stop();
		AmbienceReversed.time = playbackPos;
		AmbienceReversed.Play();
	}

}
