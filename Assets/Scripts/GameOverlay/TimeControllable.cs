using UnityEngine;
using System.Collections;

public abstract class TimeControllable : MonoBehaviour {

	private bool failedToLoadAudio = false;

	TimeObjectAudio _loadedAudio;
	TimeObjectAudio _audio {
		get {
			if (_loadedAudio)
				return _loadedAudio;

			if(failedToLoadAudio)
				return null;

			_loadedAudio = GetComponent<TimeObjectAudio>();
			if (!_loadedAudio)
				failedToLoadAudio = true;

			return _loadedAudio;
		}
	}
	

    public abstract void SetFloat(float var);
	public abstract float GetFloat();

	public void MusicForward ()
	{
		if(_audio)
			_audio.PlayForward(GetFloat());
	}
	
	public void MusicBackward ()
	{
		if(_audio)
			_audio.PlayBackward(GetFloat());
	}
	
	public void StopMusic ()
	{
		if(_audio)
			_audio.StopMusic();
	}
}