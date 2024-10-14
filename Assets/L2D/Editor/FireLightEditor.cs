using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace L2D
{
    [CustomEditor(typeof(FireLight))]
    public class FireLightEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            FireLight script = (FireLight)target;


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

                float newradius = EditorGUILayout.FloatField(new GUIContent("Radius", "Directly controls the size of the light."), script.radius);
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

                int newrayCount = (int)EditorGUILayout.Slider(new GUIContent("Raycast Count", "The higher this number the better the light will look but will be more expensive."), script.rayCount, 0, 1000);
                if (newrayCount != script.rayCount)
                {
                    Undo.RecordObject(script, "Change Raycast Count");
                    script.rayCount = newrayCount;
                    script.OnValidate();
                }
            }
            EditorGUILayout.Space(5);

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
                EditorGUILayout.Space(5);

                float newfireMovement = EditorGUILayout.Slider(new GUIContent("Fire Move Step", "How much the fire light can jump in one frame."), script.fireMovement, 0, 1);
                if (newfireMovement != script.fireMovement)
                {
                    Undo.RecordObject(script, "Change Fire Move Step");
                    script.fireMovement = newfireMovement;
                    script.OnValidate();
                }

                float newfireMaxMovement = EditorGUILayout.Slider(new GUIContent("Fire Max Distance", "Limit of how far the fire can move from center of the light."), script.fireMaxMovement, 0, 1);
                if (newfireMaxMovement != script.fireMaxMovement)
                {
                    Undo.RecordObject(script, "Change Fire Max Distance");
                    script.fireMaxMovement = newfireMaxMovement;
                    script.OnValidate();
                }
            }
            EditorGUILayout.Space(5);

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Performance", EditorStyles.boldLabel);
                EditorGUILayout.Space(5);

                bool newdynamicLight = EditorGUILayout.Toggle(new GUIContent("Dynamic Light", "If true this light will shadow cast every frame."), script.dynamicLight);
                if (newdynamicLight != script.dynamicLight)
                {
                    Undo.RecordObject(script, "Toggle Dynamic Light");
                    script.dynamicLight = newdynamicLight;
                    script.OnValidate();
                }

                bool newrunInEditor = EditorGUILayout.Toggle(new GUIContent("Show In Edit Mode", "If true this light will update and be displayed while in Edit Mode."), script.runInEditor);
                if (newrunInEditor != script.runInEditor)
                {
                    Undo.RecordObject(script, "Toggle Show In Edit Mode");
                    script.runInEditor = newrunInEditor;
                    script.OnValidate();
                }


                if (!script.dynamicLight)
                {
                    if (GUILayout.Button(new GUIContent("Bake Light", "Will instantly cause the light to update and shadow cast.")))
                    {
                        script.generateLight = true;
                    }
                }
            }
        }
    }
}