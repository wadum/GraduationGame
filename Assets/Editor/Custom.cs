using UnityEditor;

[CustomEditor(typeof(FogControl))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        {
            if (target.GetType() == typeof(FogControl))
            {
                var getterSetter = (FogControl)target;
                getterSetter.colorProperty = getterSetter.color;
                getterSetter.densityProperty = getterSetter.density;
                getterSetter.ambientIntensityProperty = getterSetter.ambientIntensity;
            }
        }
    }
}