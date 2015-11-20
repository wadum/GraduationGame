using UnityEngine;
using System.Collections;

public class Clockpart : MonoBehaviour
{
    public GameObject[] FloatWaypoints;

    public bool pickedUp = false;
    public GameObject clockPart;
    public float speed = 1.0f;
    public float RotationSpeed = 1.0f;

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
        _clockPieceInternalPos, 
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
            return;
        }

        if (pickedUp)
        {
            clockPart.transform.localPosition = Vector3.right;
            transform.position = _player.transform.position + Vector3.up;
        }

        transform.Rotate(Vector3.up, (50 * Time.deltaTime) * RotationSpeed);
    }

    void OnTriggerEnter(Collider player)
    {
        if (player.tag != "Player")
        {
            return;
        }

        transform.parent = player.transform;
        transform.position += Vector3.up;
        pickedUp = true;
        this.GetComponent<Collider>().enabled = false;
        player.GetComponent<CharacterInventory>().AddClockPart(this.gameObject);
    }

    public void CollectToFinalPiece(Vector3 ReassemblePos)
    {
        transform.parent = null;
        _startTime = Time.time;
        _lerpEndPos = ReassemblePos;
        _lerpStartingPos = _player.transform.position + Vector3.up;
        _journeyLength = Vector3.Distance(_lerpStartingPos, _lerpEndPos);
        _reassemble = true;
        StartCoroutine(reassemble());
    }

    public IEnumerator reassemble()
    {
        clockPart.transform.localPosition = Vector3.right;
        pickedUp = false;



        float distCovered = (Time.time - _startTime);
        float fracJourney = distCovered / _journeyLength;

      

        while (fracJourney < 1)
        {
            RotationSpeed += speed * Time.deltaTime;
            distCovered = (Time.time - _startTime);
            fracJourney = distCovered / _journeyLength;
            //rotation
            transform.Rotate(Vector3.up, (50 * Time.deltaTime) * RotationSpeed);
            //decreasing radius
            clockPart.transform.localPosition = Vector3.right - Vector3.right * fracJourney;
            //moving up
            transform.position = Vector3.Lerp(_lerpStartingPos, _lerpEndPos, fracJourney);
            yield return null;
        }

        float time = Time.time + 1.5f;

        while(time > Time.time)
        {
            RotationSpeed += speed * Time.deltaTime;
            transform.Rotate(Vector3.up, (50 * Time.deltaTime) * RotationSpeed);
            yield return null;
        }

        finishedReassembling = true;
        _reassemble = false;

        yield return new WaitForSeconds(1.5f);
        yield return null;
    }
}
