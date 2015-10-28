using UnityEngine;
using System.Collections;

public class ClockPartRandomotation : MonoBehaviour {

    bool up;
    bool down;
    float RandomValue;

    // Use this for initialization
    void Start () {
        up = true;
        down = false;
        Vector3 startPos = new Vector3(transform.localPosition.x, Random.Range(-0.2f, 0.2f), transform.localPosition.z);
        transform.localPosition = startPos;

        RandomValue = Random.Range(0.0f, 360f);

        transform.Rotate(Vector3.up, Random.Range(0.0f, 360f));
        transform.Rotate(Vector3.right, Random.Range(0.0f, 360f));
        transform.Rotate(Vector3.forward, Random.Range(0.0f, 360f));
    }
	
	// Update is called once per frame
	void Update () {
        
        transform.Rotate(Vector3.up, 100 * Time.deltaTime);
        transform.Rotate(Vector3.right, 100 * Time.deltaTime);
        transform.Rotate(Vector3.forward, 100 * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, transform.parent.position.y + Mathf.Sin(RandomValue + Time.time) * 0.4f, transform.position.z);

    }
}
