using UnityEngine;

public class Respawnable : MonoBehaviour {

    private class RespawnLocation
    {
        public RespawnLocation(Vector3 position, Quaternion rotation)
        {
            _position = position;
            _rotation = rotation;
        }

        public RespawnLocation(Transform transform) : this(transform.position, transform.rotation) { }

        private readonly Vector3 _position;
        private readonly Quaternion _rotation;

        public Vector3 GetPosition()
        {
            return _position;
        }

        public Quaternion GetRotation()
        {
            return _rotation;
        }
    }

    private RespawnLocation _respawnLocation;
    private NavMeshAgent _nav;

	void Start ()
    {
        SetCurrentLocationAsRespawn();
	    _nav = GetComponent<NavMeshAgent>();
    }

    public void Respawn()
    {
        if (_nav)
        {
            _nav.Warp(_respawnLocation.GetPosition());
            _nav.SetDestination(transform.position);
        }
        else
            transform.position = _respawnLocation.GetPosition();

        transform.rotation = _respawnLocation.GetRotation();
    }

    public void SetCurrentLocationAsRespawn()
    {
        SetTransformLocationAsRespawn(transform);
    }

    public void SetTransformLocationAsRespawn(Transform targetTransform)
    {
        _respawnLocation = new RespawnLocation(targetTransform);
    }

    public void SetLocationAsRespawn(Vector3 position, Quaternion rotation)
    {
        _respawnLocation = new RespawnLocation(position, rotation);
    }
}