using UnityEngine;
using System.Collections;

public class RandomRotation : MonoBehaviour
{
    bool _doOnce = true;
    float RandomValue;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_doOnce)
        {
            RandomValue = Random.Range(0.0f, 360f);

            transform.Rotate(Vector3.up, Random.Range(0.0f, 360f));
            transform.Rotate(Vector3.right, Random.Range(0.0f, 360f));
            transform.Rotate(Vector3.forward, Random.Range(0.0f, 360f));

            _doOnce = false;
        }
        RandomValue = Random.Range(0.0f, 1f);
    
        transform.Rotate(Vector3.up, 100 * Time.deltaTime * RandomValue);
        transform.Rotate(Vector3.right, 100 * Time.deltaTime * RandomValue);           
        transform.Rotate(Vector3.forward, 100 * Time.deltaTime * RandomValue);
            
    }
}
