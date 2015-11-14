using UnityEngine;

public class Clockpart : MonoBehaviour
{
    public GameObject[] FloatWaypoints;

    public bool pickedUp = false;
    public GameObject clockPart;
    public float speed = 1.0f;
    public AudioSource PickupSound;

    private bool _flyToCenterClock = false;
    private float _startTime,
        _journeyLength;
    private GameObject _player;
    private Vector3 _lerpStartingPos,
        _lerpEndPos;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        transform.Rotate(Vector3.up, Random.Range(0.0f, 360.0f));
    }

    void Update()
    {
        transform.Rotate(Vector3.up, 100 * Time.deltaTime);

        if (pickedUp)
        {
            clockPart.transform.localPosition = Vector3.right + Vector3.up;
            transform.position = _player.transform.position;
        }

        if (_flyToCenterClock)
        {
            float distCovered = (Time.time - _startTime) * speed;
            float fracJourney = distCovered / _journeyLength;
            //Debug.Log(gameObject.name + " Start: " + _lerpStartingPos + " End: " + _lerpEndPos + " frac: " + fracJourney + " _journeyLength: " + _journeyLength + " distCovered: " + distCovered);

            if (distCovered == 0 || _journeyLength == 0)
            {
                fracJourney = 1.1f;
            }

            transform.position = Vector3.Lerp(_lerpStartingPos, _lerpEndPos, fracJourney);

            if (fracJourney > 1)
            {
                FindObjectOfType<CenterClockworkDeliverence>().TurnedIn += 1;
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

        transform.parent = player.transform;
        pickedUp = true;
        this.GetComponent<Collider>().enabled = false;
        player.GetComponent<CharacterInventory>().AddClockPart(this.gameObject);
        if (PickupSound)
            PickupSound.Play();
    }

    public void goToCenterClock(Vector3 CenterClockPos)
    {
        transform.parent = null;
        _startTime = Time.time;
        _lerpEndPos = CenterClockPos;
        _lerpStartingPos = _player.transform.position;
        _journeyLength = Vector3.Distance(_lerpStartingPos, _lerpEndPos);
        pickedUp = false;
        _flyToCenterClock = true;
    }
}
