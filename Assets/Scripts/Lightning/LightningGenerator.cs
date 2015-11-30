using UnityEngine;
using System.Linq;

public class LightningGenerator : MonoBehaviour {

    public int lengthOfLineRenderer;
    public float frecuency;
    public float maxConductorDistance;
    public float JitterDistance = 0.1f;
    public float SinScale = 0.6f;

    public AudioSource Audio;

    public GameObject[] LightningConductorsToIgnore;
    public GameObject[] LightningConductors;
    LineRenderer lineRenderer;

    float LengthForEachSection;
    public float timePassed { get; set; }
    Camera cam = null;
    public GameObject lightningConductor { get; set; }
    float conductorDistance;

    // Use this for initialization
    void Awake () {

        // add linerenderer
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(lengthOfLineRenderer);

        // Find lightningConductors
        LightningConductors = GameObject.FindGameObjectsWithTag("LightningConductor");

        for (int i = 0; i < LightningConductorsToIgnore.Length; i++)
        {
            LightningConductors = LightningConductors.Where(con => con != LightningConductorsToIgnore[i]).ToArray();
        }
    }

    // Update is called once per frame
    void Update() {
        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null) return;
        }

        if (LightningConductors == null || LightningConductors.Length < 1)
        {
            lineRenderer.enabled = false;
            if(Audio)
                Audio.Stop();
            return;
        }
        // Add time
        timePassed += Time.deltaTime;

        // reset conductor max distance
        conductorDistance = (lightningConductor != null) ? Vector3.Distance(transform.position, lightningConductor.transform.position) : maxConductorDistance;
        
        //Remove the lightningconductor if the target conductor is not working
        if (lightningConductor && lightningConductor.gameObject.activeInHierarchy == false)
        {
            lightningConductor = null;
        }
        //Find the closest lightning conducter
        for (int i = 0; i < LightningConductors.Length; i++)
        {

            if (LightningConductors[i].gameObject.activeInHierarchy == false) continue;

            if (Vector3.Distance(transform.position, LightningConductors[i].transform.position) < conductorDistance && Vector3.Distance(transform.position, LightningConductors[i].transform.position) > 0.05) {
                conductorDistance = Vector3.Distance(transform.position, LightningConductors[i].transform.position);
                if (lightningConductor && lightningConductor != LightningConductors[i])
                {
                    lightningConductor.GetComponent<Electrified>().Deactivate();
                }
                lightningConductor = LightningConductors[i];
            }
        }

        //If the conductors are to far away dont shoot lightning
        if (conductorDistance >= maxConductorDistance)
        {
            if (lightningConductor)
            {
                lightningConductor.GetComponent<Electrified>().Deactivate();
            }
            lightningConductor = null;
            lineRenderer.enabled = false;
            if (Audio && Audio.enabled)
                Audio.Stop();
            return;
        }

        if (lightningConductor)
        {
            lightningConductor.GetComponent<Electrified>().Activate();
            lineRenderer.enabled = true;
			if (Audio && Audio.isActiveAndEnabled && (!Audio.isPlaying) )
                Audio.Play();
                

            //if enough time has passed update the line
            if (timePassed > frecuency)
            {
                LengthForEachSection = conductorDistance / lengthOfLineRenderer;
                lineRenderer.SetPosition(0, transform.position);
                for (int i = 1; i < lengthOfLineRenderer - 1; i++)
                {
                    Vector3 originalPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    Vector3 dirMoveDist = (lightningConductor.transform.position - originalPos).normalized * LengthForEachSection;
                    Vector3 RandomDisplacement = (Vector3.Cross(dirMoveDist, cam.transform.position - originalPos)).normalized * Random.Range(-JitterDistance, JitterDistance);
                    Vector3 SinusCurver = new Vector3(0, SinScale, 0);

                    if ( i < lengthOfLineRenderer / 2) { 
                        SinusCurver = (SinusCurver * i / lengthOfLineRenderer) * (i - (lengthOfLineRenderer / 2));
                    }
                    else
                    {
                        SinusCurver = (SinusCurver * (lengthOfLineRenderer - i) / lengthOfLineRenderer) * (i - (lengthOfLineRenderer/2));
                    }

                    Vector3 pos = originalPos + dirMoveDist * i + RandomDisplacement + SinusCurver * SinScale;
                    lineRenderer.SetPosition(i, pos);
                }
                Vector3 lastPos = lightningConductor.transform.position;
                lineRenderer.SetPosition(lengthOfLineRenderer - 1, lastPos);
                timePassed = 0.0f;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, maxConductorDistance);
    }

    void OnDisable()
    {
        if (lightningConductor)
        {
            lightningConductor.GetComponent<Electrified>().Deactivate();
        }
    }
}
