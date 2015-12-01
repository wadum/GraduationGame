using UnityEngine;
using System.Collections;

public class BridgeDispController : MonoBehaviour {

    public GameObject BridgeBlockers;
    public GameObject ActiveMe;
    public Animator Ani;
    public float Speed = 0.15f;
	
	// Update is called once per frame
	void Update () {
        if (ActiveMe.activeSelf) Ani.SetFloat("Blend",Mathf.Min(Ani.GetFloat("Blend") + Time.deltaTime * Speed, 1.0f));
        else Ani.SetFloat("Blend", Mathf.Max(Ani.GetFloat("Blend") - Time.deltaTime * Speed, 0));

        if (ActiveMe.activeSelf)
        {
            BridgeBlockers.SetActive(false);
        }
        else
        {
            BridgeBlockers.SetActive(true);
        }

    }
}
