using UnityEngine;

public class Clockpart : MonoBehaviour
{
    public GameObject[] FloatWaypoints;

    public bool pickedUp = false;
    public GameObject clockPart;
    public float speed = 1.0f;
    public AudioSource PickupSound;

    public bool
        finishedReassembling = false;
    private bool
        _reassemble = false;
    private float 
        _startTime,
        _journeyLength;
    private GameObject 
        _player;
    private Vector3 
        _lerpStartingPos,
        _lerpEndPos;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        transform.Rotate(Vector3.up, Random.Range(0.0f, 360.0f));
    }

    void Update()
    {
        if (_reassemble)
        {
            speed += 1;

            transform.Rotate(Vector3.up, (50 * Time.deltaTime) * speed);

            float distCovered = (Time.time - _startTime) * speed;
            float fracJourney = distCovered / _journeyLength;

            transform.position = Vector3.Lerp(_lerpStartingPos, _lerpEndPos, fracJourney);

            if (fracJourney > 1)
            {
                FindObjectOfType<CenterClockworkDeliverence>().TurnedIn += 1;
                finishedReassembling = true;
            }
            return;
        }

        transform.Rotate(Vector3.up, (50 * Time.deltaTime) * speed);

        if (pickedUp)
        {
            clockPart.transform.localPosition = Vector3.right + Vector3.up;
            transform.position = _player.transform.position;
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

    public void CollectToFinalPiece(Vector3 ReassemblePos)
    {
        transform.parent = null;
        _startTime = Time.time;
        _lerpEndPos = ReassemblePos;
        _lerpStartingPos = _player.transform.position;
        _journeyLength = Vector3.Distance(_lerpStartingPos, _lerpEndPos);
        pickedUp = false;
        _reassemble = true;
    }
}
