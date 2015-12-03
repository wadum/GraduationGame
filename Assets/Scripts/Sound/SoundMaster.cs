using UnityEngine;
using System.Collections.Generic;

public class SoundMaster : MonoBehaviour {

	private List<ZoneAudio> _zones;
	public List<ZoneAudio> Zones{
		get {
			if(_zones == null)
				_zones = new List<ZoneAudio>();
			return _zones;
		}
	}

	void Start()
	{
        if(GameObject.FindGameObjectWithTag("Player"))
    		transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
		transform.localPosition = Vector3.zero;
	}

	public void PlayNormal()
	{
		_zones.ForEach(zone => zone.PlayNormal());
	}

	public void PlayReversed()
	{
		_zones.ForEach(zone => zone.PlayReversed());
	}
}
