using UnityEngine;
using System.Collections;

public class ZoneAudio : MonoBehaviour {

	public AudioSource Normal, Reversed;
	
	void Start () {
		GameObject.FindGameObjectWithTag("SoundBank").GetComponent<SoundMaster>().Zones.Add(this);
		if (Mathf.Abs (Normal.clip.length - Reversed.clip.length) > 0.1f)
			Debug.Log (string.Format("AudioClip {0} is not the same length as {1}!", Normal.clip.name, Reversed.clip.name));
	}

	public void PlayNormal()
	{
		var playbackPos = Normal.clip.length - Reversed.time;
		Reversed.Stop();	
		Normal.time = playbackPos;
		Normal.Play();
	}
	
	public void PlayReversed()
	{
		var playbackPos = Mathf.Abs((Reversed.clip.length - Normal.time)%Normal.clip.length);
		Normal.Stop();
		Reversed.time = playbackPos;
		Reversed.Play();
	}

}
