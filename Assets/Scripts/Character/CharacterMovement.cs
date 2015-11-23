using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public GameObject DeliverClockPartArea;
    public float WaypointFloatSpeed = 1.0f;
    public float RotationSpeed = 3.0f;
    // The TutorialMoveFreeze is a way to freeze the player, ignoring selected input while true
    public bool TutorialMoveFreeze = false;

    private bool _lootAtMovingObject = false;
    private GameObject _lookAtTarget = null;
    private NavMeshAgent _agent;

    LookHERE tmpLTarget;

    // Use this for initialization
    void Start()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();

        MultiTouch.RegisterTapHandlerByTag("Terrain", hit =>
        {
            if (TutorialMoveFreeze) return false;
            GoTo(hit.point);
            GameOverlayController.gameOverlayController.DeactivateSlider();
			return true;
        });
        MultiTouch.RegisterTapHandlerByTag("Clockpart", hit => {
            if (TutorialMoveFreeze) return false;
            GoTo(hit.collider.transform.position);
			return true;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (_lootAtMovingObject)
        {
            if (_lookAtTarget != null)
            {
                if (tmpLTarget != null)
                {
                    Vector3 tmpActualTarget = new Vector3(tmpLTarget.transform.position.x, this.gameObject.transform.position.y, tmpLTarget.transform.position.z);
                    Quaternion targetRotation = Quaternion.LookRotation(tmpActualTarget - transform.position);
                    float str = RotationSpeed * Time.deltaTime;
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
                }
                else
                    Debug.LogWarning("LookHERE script has to be assigned to one of the children of the target gameObject: " + _lookAtTarget.name);
            }
        }

        if (!DeliverClockPartArea)
        {
            Debug.Log("Add DeliverClockPartArea");
            return;
        }
    }

    public void SetPlayerLookAtWhenMagic(bool value, GameObject go)
    {
        _lootAtMovingObject = value;
        _lookAtTarget = go;
        if (go)
            tmpLTarget = _lookAtTarget.GetComponentInChildren<LookHERE>();
    }

    public void GoTo(Vector3 position)
    {
        if (_agent.enabled)
        {
            GameOverlayController.gameOverlayController.DeactivateSlider();
            _agent.SetDestination(position);
        }
    }
}