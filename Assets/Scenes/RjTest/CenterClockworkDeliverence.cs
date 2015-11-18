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
            StartCoroutine(LoadNewLevelAfterXTime());
        }
    }

    void OnTriggerStay(Collider player)
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
    }

    IEnumerator LoadNewLevelAfterXTime()
    {
        yield return new WaitForSeconds(1.5f);
        if (SaveLoad.saveLoad)
        {
            SaveLoad.saveLoad.Reset();
        }
        Application.LoadLevel(NextLevel);
    }
}
