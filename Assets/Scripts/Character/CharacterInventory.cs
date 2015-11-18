using UnityEngine;
using System.Collections;
using System.Linq;

public class CharacterInventory : MonoBehaviour
{

    public GameObject[] clockParts;
    public GameObject centerClock;

    public GameObject Whole_Piece;

    public int clockpartsToCollect;
    public int clockPartCounter = 0;

    private bool _doOnce;

    private CharacterMovement _playerMovement;
    
    void Start()
    {
        _doOnce = false;
        if (!centerClock) centerClock = gameObject;
        _playerMovement = gameObject.GetComponent<CharacterMovement>();
    }
    
    void Update()
    {
        if (clockPartCounter >= clockpartsToCollect && !_doOnce)
        {
            for (int i = 0; i < clockpartsToCollect; i++)
            {
                clockParts[i].GetComponent<Clockpart>().CollectToFinalPiece(transform.position + Vector3.up * 2);
                //clockParts[i] = null;
                //clockPartCounter--;
            }
            _doOnce = true;
        }

        if (clockPartCounter >= clockpartsToCollect && _doOnce)
        {
            if(!(clockParts[0].GetComponent<Clockpart>().finishedReassembling && clockParts[1].GetComponent<Clockpart>().finishedReassembling))
            {
                return;
            }

            Whole_Piece.SetActive(true);
            Whole_Piece.transform.parent = null;
            StartCoroutine(Whole_Piece.GetComponent<FinalPieceAI>().PositionFinalPiece());
            _doOnce = false;
        }

        if (Whole_Piece.activeSelf == true)
        {
            _playerMovement.GoToCenterClock(clockParts[clockPartCounter - 1].GetComponent<Clockpart>().FloatWaypoints);
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
}
