using UnityEngine;
using System.Collections;

public class CharacterInventory : MonoBehaviour
{

    public GameObject[] clockParts;
    public GameObject centerClock;

    public int clockpartsToCollect;
    public int clockPartCounter = 0;

    private bool _deliver,
        _doOnce;

    private CharacterMovement _playerMovement;
    
    void Start()
    {
        _deliver = false;
        _doOnce = true;
        if (!centerClock) centerClock = gameObject;
        _playerMovement = gameObject.GetComponent<CharacterMovement>();
    }
    
    void Update()
    {

        if (clockPartCounter >= clockpartsToCollect && _doOnce)
        {
            _playerMovement.GoToCenterClock(clockParts[clockPartCounter - 1].GetComponent<Clockpart>().FloatWaypoints);
            _doOnce = false;
        }

        if (clockPartCounter >= clockpartsToCollect && _deliver)
        {
            for (int i = 0; i < clockpartsToCollect; i++)
            {
                clockParts[i].GetComponent<Clockpart>().goToCenterClock(centerClock.transform.position);
                clockParts[i] = null;
                clockPartCounter--;
            }
            _doOnce = true;
        }
    }

    public void AddClockPart(GameObject clockpart)
    {
        // Sanity.. This is an ugly fix, but I dont know why my loading gives two cogs..
        foreach (GameObject cog in clockParts)
        {
            if (cog != null)
            {
                if (cog.name == clockpart.name)
                {
                    return;
                }
            }
        }
        clockParts[clockPartCounter] = clockpart;
        clockPartCounter++;
        for (int i = 0; i < clockPartCounter; i++)
        {
            clockParts[i].transform.rotation = Quaternion.Euler(0, 360 / clockPartCounter * i, 0);
        }
    }

    public void Deliver(bool value)
    {
        _deliver = value;
    }
}
