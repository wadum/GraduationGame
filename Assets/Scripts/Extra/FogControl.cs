using UnityEngine;

public class FogControl : MonoBehaviour {

    public Color color;

    [Range(0,1)]
    public float density;

    [Range(0,1)]
    public float ambientIntensity;

    void LateUpdate() {
        RenderSettings.fogColor = color;
        RenderSettings.fogDensity = density;
        RenderSettings.ambientIntensity = ambientIntensity;
    }
}