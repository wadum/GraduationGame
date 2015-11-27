using System.Collections.Generic;
using UnityEngine;

public class FogControl : MonoBehaviour {

    public Color color;

    [Range(0,1)]
    public float density;

    [Range(0,1)]
    public float ambientIntensity;

    public Color colorProperty {
        get { return color; }
        set { RenderSettings.fogColor = color = value; }
    }

    public float densityProperty {
        get { return density; }
        set { RenderSettings.fogDensity = density = value; }
    }

    public float ambientIntensityProperty {
        get { return ambientIntensity; }
        set { RenderSettings.ambientIntensity = ambientIntensity = value; }
    }

    // To control the fog while the game is running. The editor properties above is not enough, as the inspector does not run in the actual build.
    void LateUpdate() {
        RenderSettings.fogColor = color;
        RenderSettings.fogDensity = density;
        RenderSettings.ambientIntensity = ambientIntensity;
    }
}