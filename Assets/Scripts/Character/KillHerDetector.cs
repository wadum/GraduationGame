using UnityEngine;
using System.Collections;

public class KillHerDetector : MonoBehaviour {
    
    public Transform SpawnLocation;

    GameObject _player;
    float _oldTimePos;
    ObjectTimeController _timeController;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _timeController = gameObject.GetComponentInParent<ObjectTimeController>();
    }

    void OnTriggerEnter(Collider _object)
    {
        if (_object.gameObject.tag == "Player")
        {
            _object.GetComponent<Respawnable>().SetTransformLocationAsRespawn(SpawnLocation);
            _oldTimePos = _timeController.TimePos;
        }
    }

    void OnTriggerStay(Collider _object)
    {
        if (_timeController.Moving)
            StartCoroutine(_player.GetComponent<AddLethalCollisionOnTimeObjects>().MurderTheBitch());
    }
}
