using UnityEngine;
using System.Collections;

public class CreditsController : MonoBehaviour {

    public float ScrollSpeed = 50;
    public float WaitTime = 1.5f;
    public float FinalScrollDistance;

    public GameObject FadeScreen;

    Transform OriginalPos;
    public GameObject CombinedCredits;

    float _scrollDistance;

    void OnEnable()
    {
        _scrollDistance = 0;
        WaitTime += Time.time;
        //CombinedCredits.transform.position = OriginalPos.position;
    }

    void OnDisable()
    {
        WaitTime = 1.5f;
        CombinedCredits.transform.Translate(Vector3.down * _scrollDistance);
    }
	// Update is called once per frame
	void Update () {
       
        if (WaitTime < Time.time)
        {
            CombinedCredits.transform.Translate(Vector3.up * ScrollSpeed * Time.deltaTime);
            _scrollDistance += ScrollSpeed * Time.deltaTime;
        }

        //Debug.Log(_scrollDistance);
        //Debug.Log(FinalScrollDistance);
        if (FinalScrollDistance < _scrollDistance)
        {
            transform.parent.gameObject.GetComponent<SettingsMenu>().ReturnFromCredits();
        }
    }

    
}
