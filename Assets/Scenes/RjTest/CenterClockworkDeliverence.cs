using UnityEngine;
using System.Collections;

public class CenterClockworkDeliverence : MonoBehaviour
{
    public int TurnedIn;
    public string NextLevel = "Main Menu";
    bool done = false;

    private Clockpart[] _clockParts;

    void Start()
    {
        _clockParts = FindObjectsOfType<Clockpart>();
    }

    void Update()
    {
        if (done)
            return;

        if (_clockParts.Length == TurnedIn)
        {
            done = true;
            // Fire winning animation
            if (SaveLoad.saveLoad)
            {
                SaveLoad.saveLoad.Reset();
            }
            //I have removed it from here since the loading of the next level should happen once the final part has been positioned in the cener clock go to the FinalPieceAI script and its StartCoroutine to see the new place for loading next lvl
            //StartCoroutine(LoadNewLevelAfterXTime());
        }
    }

    /*void OnTriggerStay(Collider player)
    {
        if (player.tag != "Player")
        {
            return;
        }
        player.GetComponent<CharacterInventory>().Deliver(true);
    }

    void OnTriggerExit(Collider player)
    {
        if (player.tag != "Player")
        {
            return;
        }
        player.GetComponent<CharacterInventory>().Deliver(false);
    }*/

    IEnumerator LoadNewLevelAfterXTime()
    {
        yield return new WaitForSeconds(1.5f);
        Application.LoadLevel(NextLevel);
    }
}
