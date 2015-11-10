using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ObjectTimeController))]
public class TimeObjectAudio : MonoBehaviour {

	public AudioSource ForwardEffect, BackwardEffect;

	void Start()
	{
		if (Mathf.Abs (ForwardEffect.clip.length - BackwardEffect.clip.length) > 1f)
			Debug.Log (string.Format("AudioClip {0} is not the same length as {1}!", ForwardEffect.clip.name, BackwardEffect.clip.name));
	}
	
	public void PlayForward(float pos)
	{
		float percentage = (pos/100);
		if(percentage >= 1)
			return;
		ForwardEffect.time = percentage * ForwardEffect.clip.length;
		ForwardEffect.Play();
	}
	
	public void PlayBackward(float pos)
	{
		float percentage = (pos/100);
		if(percentage <= 0)
			return;
		BackwardEffect.time = BackwardEffect.clip.length - (percentage * BackwardEffect.clip.length);
		BackwardEffect.Play();
	}

	public void StopMusic(){
		ForwardEffect.Stop();
		BackwardEffect.Stop();
	}
}
