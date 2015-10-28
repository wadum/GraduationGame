using UnityEngine;
using System.Collections;

public class CharacterInventory : MonoBehaviour {

    public GameObject[] clockParts;
    public GameObject centerClock;

    public int clockpartsToCollect;

    int clockPartCounter = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (clockPartCounter == clockpartsToCollect)
        {
            for (int i = 0; i < clockpartsToCollect; i++)
            {
                clockParts[i].GetComponent<Cockpart>().goToCenterClock(centerClock.transform.position);
                clockParts[i] = null;
                clockPartCounter--;
            }
        }
	}

    public void AddClockPart(GameObject clockpart)
    {
        clockParts[clockPartCounter] = clockpart;
        clockPartCounter++;
        for(int i = 0; i < clockPartCounter; i++)
        {
            clockParts[i].transform.rotation = Quaternion.Euler(0, 360 / clockPartCounter * i, 0);

            //Rotate(Vector3.up, 360/clockPartCounter * i);

            //transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
}
