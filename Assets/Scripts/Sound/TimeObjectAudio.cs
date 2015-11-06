using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ObjectTimeController))]
public class TimeObjectAudio : MonoBehaviour {

	public AudioSource ForwardEffect, BackwardEffect;

	void Start()
	{
		if (Mathf.Abs (ForwardEffect.clip.length - BackwardEffect.clip.length) > 0.1f)
			Debug.Log (string.Format("AudioClip {0} is not the same length as {1}!", ForwardEffect.clip.name, BackwardEffect.clip.name));
	}
	
	public void PlayForward(float pos)
	{
		ForwardEffect.time = (pos/100) * ForwardEffect.clip.length;
		ForwardEffect.Play();
	}
	
	public void PlayBackward(float pos)
	{
		BackwardEffect.time = BackwardEffect.clip.length - ((pos/100) * BackwardEffect.clip.length);
		BackwardEffect.Play();
	}

	public void StopMusic(){
		ForwardEffect.Stop();
		BackwardEffect.Stop();
	}
}
