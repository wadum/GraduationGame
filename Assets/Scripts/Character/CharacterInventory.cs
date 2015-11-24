using UnityEngine;
using System.Collections;
using System.Linq;

public class CharacterInventory : MonoBehaviour
{

    public GameObject[] clockParts;
    public GameObject WholePiecePos;

    public GameObject Whole_Piece;

    public int clockpartsToCollect;
    public int clockPartCounter = 0;

	public ZoneAudio AmbientAddition;
	public AudioSource CompletedAudio;
	public AudioSource PickUpSound;
    public float AnimateSpeed = 1.5f;

    private bool _doOnce;

    
    void Start()
    {
        _doOnce = false;
        foreach (Clockpart clockpart in GameObject.FindObjectsOfType<Clockpart>())
            clockpart.AnimateSpeed = AnimateSpeed;
    }
    
    void Update()
    {
        if (clockPartCounter >= clockpartsToCollect && !_doOnce)
        {
            for (int i = 0; i < clockpartsToCollect; i++)
            {
                clockParts[i].GetComponent<Clockpart>().CollectToFinalPiece(transform.position + Vector3.up * 2);
            }
            _doOnce = true;
        }

        if (clockPartCounter >= clockpartsToCollect && _doOnce)
        {
			if(!CompletedAudio.isPlaying)
				CompletedAudio.Play();
            if(!(clockParts[0].GetComponent<Clockpart>().finishedReassembling && clockParts[1].GetComponent<Clockpart>().finishedReassembling))
            {
                return;
            }

            for (int i = 0; i < clockpartsToCollect; i++)
            {
                clockParts[i].SetActive(false);
            }
            if (!Whole_Piece)
            {
                Debug.Log("where the hell is the diamond!?!?!");
            }

            Whole_Piece.SetActive(true);
            Whole_Piece.transform.parent = null;
            Whole_Piece.transform.position = clockParts[0].transform.position;
            StartCoroutine(Whole_Piece.GetComponent<FinalPieceAI>().PositionFinalPiece());
            clockPartCounter = 0;
            _doOnce = false;
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
		if(clockPartCounter == 0)
		{
			AmbientAddition.enabled = true;
			GameObject.Find("Addition").GetComponent<AudioSource>().time = GameObject.Find("Ambience").GetComponent<AudioSource>().time;
		}
        clockParts[clockPartCounter] = clockpart;
        clockPartCounter++;
		if(clockPartCounter < clockpartsToCollect)
			PickUpSound.Play();
        for (int i = 0; i < clockPartCounter; i++)
        {
            clockParts[i].transform.rotation = Quaternion.Euler(0, 360 / clockPartCounter * i, 0);
        }
    }
}
