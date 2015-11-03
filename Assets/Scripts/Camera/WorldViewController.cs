using UnityEngine;
using System.Linq;
using System.Collections;

public class WorldViewController : MonoBehaviour {

	public GameObject[] Positions;
	public float TimeToTravelInSeconds = 5;
	public float CloseUpTrackingLength = 0.1f;

	private int currentPosition = 0;
	private bool moving;
	private bool closeUp = false;
	private GameObject player;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void Run(GameObject cam)
	{
		if(moving) return;
		if(closeUp) {
			if(!Input.GetKey(KeyCode.S)) return;
			closeUp = false;
			StartCoroutine(Orbit(cam, Positions[currentPosition].transform));
		}

		if(Input.GetKey(KeyCode.W))
		{
			StartCoroutine(RunCloseUp(cam));
		}
		if(Input.GetKey(KeyCode.D))
		{
			StartCoroutine(Orbit(cam, PreviousPosition()));
		}
		if(Input.GetKey(KeyCode.A))
		{
			StartCoroutine(Orbit(cam, NextPosition()));
		}

	}

	public IEnumerator RunCloseUp(GameObject cam){
		//if(Time.timeScale < 0.5) yield return null;
		closeUp = true;
		Vector3 mask = GetCloseUpPosition().transform.right * 
			(GetCloseUpPosition().transform.rotation.eulerAngles.y > 181 ||
			 GetCloseUpPosition().transform.rotation.eulerAngles.y < 1 ? -1 : 1) 
			+ GetCloseUpPosition().transform.up * -1;
		while(closeUp)
		{

			Vector3 diff = GetCloseUpPosition().transform.position - player.transform.position;
			Vector3 dest = GetCloseUpPosition().transform.position + new Vector3(diff.x * mask.x,
			                                                  diff.y * mask.y,
			                                                  diff.z * mask.z);
			float movementLength = CloseUpTrackingLength / Vector3.Distance(
				dest, cam.transform.position);
			cam.transform.position = Vector3.Lerp(cam.transform.position, dest, movementLength);
			yield return null;
		}
	}

	public Transform GetCurrentTransform()
	{
		return Positions[currentPosition].transform;
	}
	
	public Transform NextPosition ()
	{
		currentPosition = Mathf.Abs((currentPosition + 1) % Positions.Length);
		return Positions[currentPosition].transform;
	}

	public GameObject GetCloseUpPosition(){
		return Positions[currentPosition].GetComponent<CloseUpController>().CloseUpPosition;
	}

	public Transform PreviousPosition()
	{
		if(currentPosition == 0)
			currentPosition = Positions.Length - 1;
		else
			currentPosition = Mathf.Abs((currentPosition - 1) % Positions.Length);
		return Positions[currentPosition].transform;
	}

	private IEnumerator Orbit(GameObject cam, Transform dest)
	{
		Vector3 originPosition = cam.transform.localPosition;
		Quaternion originRotation = cam.transform.localRotation;
		float travelTime = 0;
		moving = true;

		while(moving && travelTime < TimeToTravelInSeconds)
		{
			travelTime += Time.deltaTime;
			cam.transform.localPosition = Vector3.Slerp(originPosition, dest.localPosition, travelTime/TimeToTravelInSeconds);
			cam.transform.localRotation = Quaternion.Slerp(originRotation, dest.localRotation, travelTime/TimeToTravelInSeconds);
			yield return null;
		}
		moving = false;
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.green;
		foreach(var p in Positions.Select(s => s.transform))
		{
			Gizmos.DrawLine(p.position, p.position + p.forward*p.localScale.sqrMagnitude);
			Gizmos.DrawCube(p.position, Vector3.one * p.localScale.sqrMagnitude);
		}
	}
	
}
