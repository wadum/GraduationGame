using UnityEngine;
using System.Collections;

public class ObjectMoveScript : MonoBehaviour {

    public Vector3 _Direction;
    public bool UseRigidBody = false;
    Rigidbody Body;

	// Use this for initialization
	void Start () {
        if (!UseRigidBody)
        {
            Body = GetComponent<Rigidbody>();
            if (Body)
            {
                Body.useGravity = false;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (UseRigidBody)
            return;
        if (_Direction != Vector3.zero)
            transform.localPosition += _Direction*Time.deltaTime;
        if (Body.velocity != Vector3.zero)
            Body.velocity = Vector3.zero;
	}

    public void SetDirection(Vector3 Direction)
    {
        _Direction = Direction;
    }
}
