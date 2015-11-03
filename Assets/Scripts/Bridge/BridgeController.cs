using UnityEngine;
using System.Collections;

public class BridgeController : MonoBehaviour {

    public GameObject Blocker1;
    public GameObject Blocker2;

    public void open()
    {
        Blocker1.SetActive(false);
        Blocker2.SetActive(false);
    }

    public void close()
    {
        Blocker1.SetActive(true);
        Blocker2.SetActive(true);
    }
}
