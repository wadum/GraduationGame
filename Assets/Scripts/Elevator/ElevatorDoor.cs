using UnityEngine;
using System.Collections;

public class ElevatorDoor : MonoBehaviour {

    public void open()
    {
        transform.localPosition = Vector3.up * 2;
    }

    public void close()
    {
        transform.localPosition = Vector3.zero;
    }
}
