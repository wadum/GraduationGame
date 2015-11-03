using UnityEngine;
using System.Collections;

public class CharacterInventory : MonoBehaviour
{

    public GameObject[] clockParts;
    public GameObject centerClock;

    public int clockpartsToCollect;

    CharacterMovement playerMovement;

    public int clockPartCounter = 0;
    bool deliver;
    bool doOnce;

    // Use this for initialization
    void Start()
    {
        deliver = false;
        doOnce = true;

        playerMovement = gameObject.GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (clockPartCounter >= clockpartsToCollect && doOnce)
        {
            playerMovement.GoToCenterClock(clockParts[clockPartCounter-1].GetComponent<Cockpart>().FloatWaypoints);
            doOnce = false;
        }

        if (clockPartCounter >= clockpartsToCollect && deliver)
        {
            for (int i = 0; i < clockpartsToCollect ; i++)
            {
                clockParts[i].GetComponent<Cockpart>().goToCenterClock(centerClock.transform.position);
                clockParts[i] = null;
                clockPartCounter--;
            }
            doOnce = true;
        }
    }

    public void AddClockPart(GameObject clockpart)
    {
        clockParts[clockPartCounter] = clockpart;
        clockPartCounter++;

        for (int i = 0; i < clockPartCounter; i++)
        {
            clockParts[i].transform.rotation = Quaternion.Euler(0, 360 / clockPartCounter * i, 0);
        }
    }

    public void Deliver(bool value)
    {
        deliver = value;
    }

}
