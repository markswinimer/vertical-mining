using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace L2D
{
    [CustomEditor(typeof(AreaLight))]
    public class AreaLightEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            AreaLight script = (AreaLight)target;


            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Light", EditorStyles.boldLabel);
                EditorGUILayout.Space(5);

                Color newcolor = EditorGUILayout.ColorField(new GUIContent("Light Color", "The color that this light will add to the scene."), script.color, true, false, true);
                if (newcolor != script.color)
                {
                    Undo.RecordObject(script, "Change Light Color");
                    script.color = newcolor;
                    script.OnValidate();
                }

                float newlightIntensity = EditorGUILayout.Slider(new GUIContent("Light Intensity", "How much darkness is removed by this light."), script.lightIntensity, 0, 2);
                if (newlightIntensity != script.lightIntensity)
                {
                    Undo.RecordObject(script, "Change Light Intensity");
                    script.lightIntensity = newlightIntensity;
                    script.OnValidate();
                }

                float newcolorIntensity = EditorGUILayout.Slider(new GUIContent("Color Intensity", "How much color is added by this light."), script.colorIntensity, 0, 2);
                if (newcolorIntensity != script.colorIntensity)
                {
                    Undo.RecordObject(script, "Change Light Intensity");
                    script.colorIntensity = newcolorIntensity;
                    script.OnValidate();
                }

            }
            EditorGUILayout.Space(5);

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Shape", EditorStyles.boldLabel);
                EditorGUILayout.Space(5);

                EditorGUILayout.LabelField("An Area Light's shape is completly determined by whatever mesh it has in the MeshFilter and it's transform data.", EditorStyles.textArea);

                float newradius = EditorGUILayout.FloatField(new GUIContent("Radius", "In an Area Light the radius is just used for shader effects and will still affect the look of the light."), script.radius);
                if (newradius != script.radius)
                {
                    Undo.RecordObject(script, "Change Radius");
                    script.radius = newradius;
                    script.OnValidate();
                }

                float newblendScale = EditorGUILayout.Slider(new GUIContent("Blend Scale", "Controls how the outside of the light blends."), script.blendScale, 0, 2);
                if (newblendScale != script.blendScale)
                {
                    Undo.RecordObject(script, "Change Blend Scale");
                    script.blendScale = newblendScale;
                    script.OnValidate();
                }
            }
            EditorGUILayout.Space(5);

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Performance", EditorStyles.boldLabel);
                EditorGUILayout.Space(5);

                EditorGUILayout.LabelField("Area Lights don't make use of shadow casting so they are super cheap compared to the other lights, but are far more limited. All Area Lights are considered dynamic lights.", EditorStyles.textArea);
                
            }
        }
    }
}