using UnityEngine;
using System.Collections;

public class TimeForceGenerator : MonoBehaviour
{

    public int lengthOfLineRenderer;
    public float frecuency;
    public float maxConductorDistance;

    public GameObject[] TimeForceConductors;
    LineRenderer lineRenderer;

    float LengthForEachSection;
    float timePassed = 0.0f;

    GameObject lightningConductor;
    public GameObject PositionSpawner;
    float conductorDistance;

    public Vector3[] _positions;
    int _currentPosition;
    bool forward = true;

    // Use this for initialization
    void Start()
    {
        _currentPosition = 0;

        for (int i = 0; i < _positions.Length; i++)
        {
            _positions[i] = transform.position;
        }

        // add linerenderer
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(lengthOfLineRenderer);

        // Find lightningConductors
        TimeForceConductors = GameObject.FindGameObjectsWithTag("LightningConductor");

    }

    // Update is called once per frame
    void Update()
    {
        // Add time
        timePassed += Time.deltaTime;

        // reset conductor max distance
        conductorDistance = maxConductorDistance;


        //Find the closest lightning conducter
        for (int i = 0; i < TimeForceConductors.Length; i++)
        {
            //LightningConductors[i].GetComponent<Electrified>().Deactivate();

            if (TimeForceConductors[i].GetComponentInParent<RiftScript>() && !TimeForceConductors[i].GetComponentInParent<RiftScript>().Partner) continue;

            if (Vector3.Distance(transform.position, TimeForceConductors[i].transform.position) < conductorDistance && Vector3.Distance(transform.position, TimeForceConductors[i].transform.position) > 1)
            {
                conductorDistance = Vector3.Distance(transform.position, TimeForceConductors[i].transform.position);
                if (lightningConductor && lightningConductor != TimeForceConductors[i])
                    lightningConductor.GetComponent<Electrified>().Deactivate();
                lightningConductor = TimeForceConductors[i];
                lightningConductor.GetComponent<Electrified>().Activate();
                LengthForEachSection = conductorDistance / lengthOfLineRenderer;
                lineRenderer.enabled = true;
            }
        }

        //If the conductors are to far away dont shoot lightning
        if (conductorDistance >= maxConductorDistance)
        {
            if (lightningConductor)
                lightningConductor.GetComponent<Electrified>().Deactivate();
            lightningConductor = null;
            lineRenderer.enabled = false;
            return;
        }

        //if enough ime has passed update the line
        /*if (timePassed > frecuency)
        {
            lineRenderer.SetPosition(0, transform.position);
            for (int i = 1; i < lengthOfLineRenderer - 1; i++)
            {
                Vector3 originalPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Vector3 dirMoveDist = (lightningConductor.transform.position - originalPos).normalized * LengthForEachSection;
                Vector3 RandomDisplacement = (Vector3.Cross(dirMoveDist, Camera.main.transform.position - originalPos)).normalized * Random.Range(-0.5f, 0.5f);
                Vector3 pos = originalPos + dirMoveDist * i + RandomDisplacement;
                lineRenderer.SetPosition(i, pos);
            }
            Vector3 lastPos = lightningConductor.transform.position;
            lineRenderer.SetPosition(lengthOfLineRenderer - 1, lastPos);
            timePassed = 0.0f;
        }*/



        Vector3 dirMoveDir = (lightningConductor.transform.position - transform.position).normalized;

        if (timePassed > frecuency)
        {
            _positions[_currentPosition] = PositionSpawner.transform.position + dirMoveDir * 0.01f;
            _currentPosition++;
            timePassed = 0.0f;

            for (int i = 0; i < _positions.Length; i++)
            {
                if (_positions[i] != PositionSpawner.transform.position)
                {
                    _positions[i] = _positions[i] + dirMoveDir * 0.1f;
                    lineRenderer.SetPosition(i, _positions[i]);
                }
                if (Vector3.Distance(_positions[i], lightningConductor.transform.position) < 0.5 || Vector3.Distance(_positions[i], PositionSpawner.transform.position) > Vector3.Distance(PositionSpawner.transform.position, lightningConductor.transform.position))
                {
                    _positions[i] = PositionSpawner.transform.position;
                }
            }
        }

        


        if (_currentPosition == _positions.Length)
        {
            _currentPosition = 0;
        }




    }
    void OnDisable()
    {
        lineRenderer.enabled = false;
        if (lightningConductor)
            lightningConductor.GetComponent<Electrified>().Deactivate();
    }
}
