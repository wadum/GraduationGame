using UnityEngine;
using System.Collections;

public class LightningGenerator : MonoBehaviour {

    public int lengthOfLineRenderer;
    public float frecuency;
    public float maxConductorDistance;

    public GameObject[] LightningConductors;
    LineRenderer lineRenderer;

    float LengthForEachSection;
    float timePassed = 0.0f;

    GameObject lightningConductor;
    float conductorDistance;

    // Use this for initialization
    void Start () {

        // add linerenderer
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(lengthOfLineRenderer);

        // Find lightningConductors
        LightningConductors = GameObject.FindGameObjectsWithTag("LightningConductor");

    }

    // Update is called once per frame
    void Update() {
        // Add time
        timePassed += Time.deltaTime;

        // reset conductor max distance
        conductorDistance = maxConductorDistance;
            

        //Find the closest lightning conducter
        for (int i = 0; i < LightningConductors.Length; i++)
        {
            //LightningConductors[i].GetComponent<Electrified>().Deactivate();

            if (LightningConductors[i].GetComponentInParent<RiftScript>() && !LightningConductors[i].GetComponentInParent<RiftScript>().Partner) continue;

            if (Vector3.Distance(transform.position, LightningConductors[i].transform.position) < conductorDistance && Vector3.Distance(transform.position, LightningConductors[i].transform.position) > 1) {
                conductorDistance = Vector3.Distance(transform.position, LightningConductors[i].transform.position);
                if(lightningConductor && lightningConductor != LightningConductors[i])
                    lightningConductor.GetComponent<Electrified>().Deactivate();
                lightningConductor = LightningConductors[i];
                lightningConductor.GetComponent<Electrified>().Activate();
                LengthForEachSection = conductorDistance / lengthOfLineRenderer;
                lineRenderer.enabled = true;
            }
        }

        //If the conductors are to far away dont shoot lightning
        if (conductorDistance >= maxConductorDistance)
        {
            if(lightningConductor)
                lightningConductor.GetComponent<Electrified>().Deactivate();
            lightningConductor = null;
            lineRenderer.enabled = false;
            return;
        }

        //if enough ime has passed update the line
        if (timePassed > frecuency)
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
        }
    }
    void OnDisable()
    {
        lineRenderer.enabled = false;
        if (lightningConductor)
            lightningConductor.GetComponent<Electrified>().Deactivate();
    }
}
