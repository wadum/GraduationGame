using UnityEngine;
using System.Collections;

public class FogControl : MonoBehaviour {

    public Color color;

    public Color colorProperty
    {
        get { return RenderSettings.fogColor; } set { RenderSettings.fogColor = value; }
    }

    [Range(0,1)]
    public float density;

    public float densityProperty
    {
        get { return RenderSettings.fogDensity; }
        set { RenderSettings.fogDensity = value; }
    }
}
