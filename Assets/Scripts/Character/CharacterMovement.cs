using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    public GameObject DeliverClockPartArea;
    public GameObject PathMarker;
    public Sprite DestinationMarker;
    public float OptimalDistanceBetweenPathPoints = 2f;
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
            StartCoroutine(DrawGreenHalos());
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
            _agent.ResetPath();
            GameOverlayController.gameOverlayController.DeactivateSlider();
            _agent.SetDestination(position);
        }
    }
    IEnumerator DrawGreenHalos()
    {
        yield return new WaitForSeconds(0.1f);
        // Start of: Creating Green halos
        GameObject previous = null;
        GameObject current;
        for (int i = 1; i < _agent.path.corners.Length; i++)
        {
            float lerpFactor = 0.0f;
            float distance = Vector3.Distance(_agent.path.corners[i], _agent.path.corners[i - 1]);
            if (previous && Vector3.Distance(previous.transform.position, _agent.path.corners[i]) < OptimalDistanceBetweenPathPoints) continue;
            lerpFactor = 1f/ (Mathf.Ceil((distance / OptimalDistanceBetweenPathPoints))+1f);

            distance = lerpFactor;
            while (distance < 1.1f)
            {
                Vector3 Pos = Vector3.Lerp(_agent.path.corners[i - 1], _agent.path.corners[i], distance);
                current = (GameObject) Instantiate(PathMarker, Pos, Quaternion.identity);
                distance += lerpFactor;
                if (previous)
                {
                    previous.transform.LookAt(current.transform);
                }
                previous = current;
            }

        }

        if(previous)
            previous.GetComponentInChildren<SpriteRenderer>().sprite = DestinationMarker;
    }
}