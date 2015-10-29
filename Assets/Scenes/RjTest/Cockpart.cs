using UnityEngine;
using System.Collections;

public class Cockpart : MonoBehaviour {

    public GameObject[] FloatWaypoints;

    GameObject player;
    public GameObject clockPart;
    public float speed = 1.0f;
    float startTime;
    float journeyLength;

    bool pickedUp = false;
    bool flyToCenterClock = false;

    Vector3 lerpStartingPos;
    Vector3 lerpEndPos;

    // Use this for initialization
    void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        transform.Rotate(Vector3.up, Random.Range(0.0f,360.0f));

    }
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(Vector3.up,100*Time.deltaTime);        

        if (pickedUp)
        {
            clockPart.transform.localPosition = Vector3.right;
            transform.position = player.transform.position;

        }

        if (flyToCenterClock)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(lerpStartingPos, lerpEndPos, fracJourney);

            if (fracJourney > 1)
            {
                Destroy(gameObject);
            }
        }

    }

    void OnTriggerEnter(Collider player)
    {

        if (player.tag != "Player")
        {
            return;
        }

        pickedUp = true;
        this.GetComponent<Collider>().enabled = false;
        player.GetComponent<CharacterInventory>().AddClockPart(this.gameObject);
       
    }

    public void goToCenterClock(Vector3 CenterClockPos)
    {
        startTime = Time.time;
        lerpEndPos = CenterClockPos;
        lerpStartingPos = player.transform.position;
        journeyLength = Vector3.Distance(lerpStartingPos, lerpEndPos);
        pickedUp = false;
        flyToCenterClock = true;
    }
}
