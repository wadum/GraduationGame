using UnityEngine;
using System.Collections;

public class RiftCleanupZone : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        Cloneable cloneable = collider.GetComponent<Cloneable>();
        if (cloneable)
        {
            if (!cloneable.IsClone && cloneable.Clone)
            {
                cloneable.Clone.name = collider.name;
                cloneable.Clone.IsClone = false;
                Destroy(collider.gameObject);
            }
        }
    }
}
