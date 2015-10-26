using UnityEngine;
using System.Linq;
using System.Collections;

public class WorldViewController : MonoBehaviour {

	public GameObject[] Positions;
	public float TimeToTravelInSeconds = 5;

	private int currentPosition = 0;
	private bool moving;

	public Transform GetCurrentTransform()
	{
		return Positions[currentPosition].transform;
	}
	
	public Transform NextPosition ()
	{
		currentPosition = Mathf.Abs((currentPosition + 1) % Positions.Length);
		return Positions[currentPosition].transform;
	}

	public Transform PreviousPosition()
	{
		if(currentPosition == 0)
			currentPosition = 3;
		else
			currentPosition = Mathf.Abs((currentPosition - 1) % Positions.Length);
		return Positions[currentPosition].transform;
	}

	public void Run(Camera cm)
	{
		if(moving) return;
		if(Input.GetKeyDown(KeyCode.D))
		{
			StartCoroutine(Orbit(cm, PreviousPosition()));
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			StartCoroutine(Orbit(cm, NextPosition()));
		}
	}

	private IEnumerator Orbit(Camera cm, Transform dest)
	{
		Vector3 originPosition = cm.transform.position;
		Quaternion originRotation = cm.transform.rotation;
		float travelTime = 0;
		moving = true;

		while(moving && travelTime < TimeToTravelInSeconds)
		{
			travelTime += Time.deltaTime;
			cm.transform.position = Vector3.Slerp(originPosition, dest.position, travelTime/TimeToTravelInSeconds);
			cm.transform.rotation = Quaternion.Slerp(originRotation, dest.rotation, travelTime/TimeToTravelInSeconds);
			yield return null;
		}
		moving = false;
	}

	void OnDrawGizmos(){
		foreach(var p in Positions.Select(s => s.transform))
		{
			Gizmos.DrawLine(p.position, p.position + p.forward*p.localScale.sqrMagnitude);
		}
	}
	
}
