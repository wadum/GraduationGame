using UnityEngine;
using System.Collections;

public class GreenHaloController : MonoBehaviour {

	public float FadeTime = 0.2f;

    NavMeshAgent _playerAgent;
    Vector3 _finalPos = Vector3.zero;
	bool triggered;
	Color c = Color.white;

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
        if(col.tag == "Player" && !triggered)
        {
			triggered = true;
			StartCoroutine(Disappear());
        }
    }

	IEnumerator Disappear()
	{

		SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();

		float time = Time.time;

		while(time + FadeTime > Time.time)
		{
			c.a = 1f - (Time.time - time)/FadeTime;
			sprite.color = c;
			yield return null;
		}
		Destroy(this.gameObject);
	}
}
