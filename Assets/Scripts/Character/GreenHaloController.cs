using UnityEngine;
using System.Collections;

public class GreenHaloController : MonoBehaviour {

    NavMeshAgent _playerAgent;
    Vector3 _finalPos = Vector3.zero;

    void Update()
    {
        if (! _playerAgent)
        {
            _playerAgent = GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>();
        }
        if (_finalPos == Vector3.zero)
        {
            _finalPos = _playerAgent.destination;
        }
        if (_finalPos != _playerAgent.destination)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
