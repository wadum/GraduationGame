using UnityEngine;
using System.Collections;

public class CloseUpController : MonoBehaviour {

	public GameObject CloseUpPosition;

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine(CloseUpPosition.transform.position, 
		                CloseUpPosition.transform.position + 
		                CloseUpPosition.transform.forward * 
		                CloseUpPosition.transform.localScale.sqrMagnitude);
		Gizmos.DrawSphere(CloseUpPosition.transform.position, CloseUpPosition.transform.localScale.sqrMagnitude);
	}
}
