using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterInventory : MonoBehaviour
{
    public List<GameObject> clockParts;
    public GameObject centerClock;

    public int clockpartsToCollect;

    CharacterMovement playerMovement;

//    public int clockPartCounter = 0;
    bool deliver;
    bool doOnce;

    void Awake()
    {
        clockParts = new List<GameObject>();
    }

    // Use this for initialization
    void Start()
    {
        deliver = false;
        doOnce = true;
        if (!centerClock) centerClock = gameObject;
        playerMovement = gameObject.GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (clockParts.Count >= clockpartsToCollect && doOnce)
        {
            playerMovement.GoToCenterClock(clockParts[clockParts.Count - 1].GetComponent<Cockpart>().FloatWaypoints);
            doOnce = false;
        }

        if (clockParts.Count >= clockpartsToCollect && deliver)
        {
            foreach (GameObject clockpart in clockParts)
                clockpart.GetComponent<Cockpart>().goToCenterClock(centerClock.transform.position);
/*            for (int i = 0; i < clockpartsToCollect ; i++)
            {
                clockParts[i].GetComponent<Cockpart>().goToCenterClock(centerClock.transform.position);
                clockParts[i] = null;
                clockPartCounter--;
            }*/
            doOnce = true;
        }
    }

    public void AddClockPart(GameObject clockpart)
    {
        clockParts.Add(clockpart);

        for (int i = 0; i < clockParts.Count; i++)
        {
            clockParts[i].transform.rotation = Quaternion.Euler(0, 360 / clockParts.Count * i, 0);
        }
    }

    public void Deliver(bool value)
    {
        deliver = value;
    }
}
