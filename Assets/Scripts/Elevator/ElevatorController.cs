using UnityEngine;
using System.Collections;

public class ElevatorController : MonoBehaviour {

    public float AnimationTimer;
    public bool MoveUp;

    public GameObject DefaultPosition, GoToPosition;

    GameObject _elevatorModel;
    ElevatorDoor _elevatorDoor;
    GameObject _player;
    ElectrifiedElevator _electrode;
    public bool _moving { get; private set;}
    bool _inDefaultPos = true;

	// Use this for initialization
	void Start () {
        _player = GameObject.FindGameObjectWithTag("Player");
        _elevatorDoor = GetComponentInChildren<ElevatorDoor>();
        _electrode = GetComponentInChildren<ElectrifiedElevator>();
        _elevatorModel = gameObject.transform.FindChild("ElevatorModel").gameObject;
        _elevatorModel.transform.position = DefaultPosition.transform.position;
        _elevatorModel.transform.rotation = DefaultPosition.transform.rotation;
    }

    public void changePosition()
    {
        //Don do anything if we arent active
        if (!_electrode.Active) return;

        if (_moving) return;

        StartCoroutine(moveElevator(_inDefaultPos ? GoToPosition : DefaultPosition));
    }

    public void goToDefault()
    {
        if(!_inDefaultPos)
            changePosition();
    }

    IEnumerator moveElevator(GameObject position)
    {
        _player.GetComponent<NavMeshAgent>().enabled = false;
        _player.transform.parent = _elevatorModel.transform;

        _elevatorDoor.close();

        _moving = true;

        Vector3 startPos = _elevatorModel.transform.position;
        Vector3 endPos = position.transform.position;
        float time = Time.time + AnimationTimer;
        while (Time.time < time)
        {
            _elevatorModel.transform.position = Vector3.Lerp(endPos, startPos, (time - Time.time)/AnimationTimer);
            yield return null;
        }

        _elevatorModel.transform.position = position.transform.position;
        _moving = false;

        _inDefaultPos = !_inDefaultPos;

        _elevatorDoor.open();

        _player.GetComponent<NavMeshAgent>().enabled = true;
        _player.transform.parent = null;
    }
}
