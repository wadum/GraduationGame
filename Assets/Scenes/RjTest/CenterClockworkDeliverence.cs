using UnityEngine;
using System.Collections;

public class CenterClockworkDeliverence : MonoBehaviour
{
    public int TurnedIn;
    public string NextLevel = "Main Menu";

    private Clockpart[] _clockParts;

    void Start()
    {
        _clockParts = FindObjectsOfType<Clockpart>();
    }

    void Update()
    {
        if (_clockParts.Length == TurnedIn)
        {
            // Fire winning animation
            if (SaveLoad.saveLoad)
            {
                SaveLoad.saveLoad.Reset();
            }
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
        Application.LoadLevel(NextLevel);
    }
}
