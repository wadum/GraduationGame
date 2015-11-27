using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FogControl))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        {
            if (target.GetType() == typeof(FogControl))
            {
                FogControl getterSetter = (FogControl)target;
                getterSetter.colorProperty = getterSetter.color;
                getterSetter.densityProperty = getterSetter.density;
            }
        }
    }
}
