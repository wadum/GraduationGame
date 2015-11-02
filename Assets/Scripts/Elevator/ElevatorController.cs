using UnityEngine;
using System.Collections;

public class ElevatorController : MonoBehaviour {

    public bool Active;
    public float AnimationTimer;
    public bool MoveUp;

    public GameObject DefaultPosition, GoToPosition;

    GameObject _elevatorModel;
    ElevatorDoor _elevatorDoor;
    GameObject _player;
    bool _moving;
    bool _inDefaultPos = true;

	// Use this for initialization
	void Start () {
        _player = GameObject.FindGameObjectWithTag("Player");
        _elevatorDoor = GetComponentInChildren<ElevatorDoor>();
        _elevatorModel = gameObject.transform.FindChild("ElevatorModel").gameObject;
        _elevatorModel.transform.position = DefaultPosition.transform.position;
        _elevatorModel.transform.rotation = DefaultPosition.transform.rotation;
        _elevatorDoor.open();
    }

    public void changePosition()
    {
        //Don do anything if we arent active
        if (!Active) return;

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
        _elevatorDoor.close();

        _moving = true;

        Vector3 startPos = _elevatorModel.transform.position;
        Vector3 endPos = startPos + Vector3.down*4f * (MoveUp ? -1 : 1);
        float time = Time.time + AnimationTimer;
        while (Time.time < time)
        {
            _elevatorModel.transform.position = Vector3.Lerp(endPos, startPos, (time - Time.time)/AnimationTimer);
            yield return null;
        }

        startPos = _elevatorModel.transform.position;
        endPos = position.transform.position + Vector3.down * 4f * (MoveUp ? -1 : 1);
        time = Time.time + (AnimationTimer);

        foreach (var mr in _elevatorModel.GetComponentsInChildren<MeshRenderer>())
            mr.enabled = false;
        while (Time.time < time)
        {
            _elevatorModel.transform.position = Vector3.Lerp(endPos, startPos, (time - Time.time) / AnimationTimer);
            yield return null;
        }

        foreach (var mr in _elevatorModel.GetComponentsInChildren<MeshRenderer>())
            mr.enabled = true;

        startPos = _elevatorModel.transform.position;
        endPos = position.transform.position;
        time = Time.time + AnimationTimer;

        while (Time.time < time)
        {
            _elevatorModel.transform.position = Vector3.Lerp(endPos, startPos, (time - Time.time) / AnimationTimer);
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
