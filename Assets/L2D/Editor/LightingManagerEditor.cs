using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;

namespace L2D
{
    [CustomEditor(typeof(LightingManager))]
    public class LightingManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            LightingManager script = (LightingManager)target;

            if (script.lightingSettings == null) return;


            LightingSettings2D newlightingSettings = (LightingSettings2D)EditorGUILayout.ObjectField(new GUIContent("Lighting Settings","The LightingSettings2D instance for this scene."), script.lightingSettings, typeof(LightingSettings2D), false);
            if (newlightingSettings != script.lightingSettings)
            {
                Undo.RecordObject(script, "Change Lighting Settings Object");
                script.lightingSettings = newlightingSettings;
                script.OnValidate();
            }
            EditorGUILayout.Space(5);
            


            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Sunlight", EditorStyles.boldLabel);
                EditorGUILayout.Space(5);


                bool newdoDayNightCycle = EditorGUILayout.Toggle(new GUIContent("Do Day Night Cycle","If true, the time of day, current sun position, and ambient light color will update automatically."), script.lightingSettings.doDayNightCycle);
                if (newdoDayNightCycle != script.lightingSettings.doDayNightCycle)
                {
                    Undo.RecordObject(script.lightingSettings, "Toggle Do Day Night Cycle");
                    script.lightingSettings.doDayNightCycle = newdoDayNightCycle;
                    script.lightingSettings.OnValidate();
                    EditorUtility.SetDirty(script.lightingSettings);
                    if (Provider.onlineState == OnlineState.Online)
                        Provider.Checkout(script.lightingSettings, CheckoutMode.Both);
                }
                EditorGUILayout.Space(5);

                if (script.lightingSettings.doDayNightCycle)
                {
                    Gradient newambientLightColorOverCycle = EditorGUILayout.GradientField(new GUIContent("Ambient Light Color Cycle","The natural light in the scene where there is no other light sources. The first half (0.0 to 0.5) is daylight hours and the second half (0.5 to 1.0) is night time hours."), script.lightingSettings.ambientLightColorOverCycle);
                    if (newambientLightColorOverCycle != script.lightingSettings.ambientLightColorOverCycle)
                    {
                        Undo.RecordObject(script.lightingSettings, "Change Ambient Light Color Cycle");
                        script.lightingSettings.ambientLightColorOverCycle = newambientLightColorOverCycle;
                        script.lightingSettings.OnValidate();
                        EditorUtility.SetDirty(script.lightingSettings);
                        if (Provider.onlineState == OnlineState.Online)
                            Provider.Checkout(script.lightingSettings, CheckoutMode.Both);
                    }

                    AnimationCurve newSunPositionOverDay = EditorGUILayout.CurveField(new GUIContent("Sun Position Over Day","The position of the sun as the day goes by. Object will cast shadows in the opposite direction."), script.lightingSettings.SunPositionOverDay);
                    if (newSunPositionOverDay != script.lightingSettings.SunPositionOverDay)
                    {
                        Undo.RecordObject(script.lightingSettings, "Change Sun Position Over Day");
                        script.lightingSettings.SunPositionOverDay = newSunPositionOverDay;
                        script.lightingSettings.OnValidate();
                        EditorUtility.SetDirty(script.lightingSettings);
                        if (Provider.onlineState == OnlineState.Online)
                            Provider.Checkout(script.lightingSettings, CheckoutMode.Both);
                    }

                    AnimationCurve newMoonPositionOverNight = EditorGUILayout.CurveField(new GUIContent("Moon Position Over Night", "The position of the moon as the night goes by. Shadow Casting Objects will cast shadows in the opposite direction."), script.lightingSettings.MoonPositionOverNight);
                    if (newMoonPositionOverNight != script.lightingSettings.MoonPositionOverNight)
                    {
                        Undo.RecordObject(script.lightingSettings, "Change Moon Position Over Night");
                        script.lightingSettings.MoonPositionOverNight = newMoonPositionOverNight;
                        script.lightingSettings.OnValidate();
                        EditorUtility.SetDirty(script.lightingSettings);
                        if (Provider.onlineState == OnlineState.Online)
                            Provider.Checkout(script.lightingSettings, CheckoutMode.Both);
                    }

                    float newshadowFadeTime = EditorGUILayout.FloatField(new GUIContent("Shadow Fade Time", "How many seconds the ambient shadows should take to fade out when switching from day to night and night to day."), script.lightingSettings.shadowFadeTime);
                    if (newshadowFadeTime != script.lightingSettings.shadowFadeTime)
                    {
                        Undo.RecordObject(script.lightingSettings, "Change Shadow Fade Time");
                        script.lightingSettings.shadowFadeTime = newshadowFadeTime;
                        script.lightingSettings.OnValidate();
                        EditorUtility.SetDirty(script.lightingSettings);
                        if (Provider.onlineState == OnlineState.Online)
                            Provider.Checkout(script.lightingSettings, CheckoutMode.Both);
                    }
                }
                else
                {
                    Color newambientColor = EditorGUILayout.ColorField(new GUIContent("Static Ambient Light Color", "The natural light in the scene where there is no other light sources."), script.lightingSettings.ambientColor);
                    if (newambientColor != script.lightingSettings.ambientColor)
                    {
                        Undo.RecordObject(script.lightingSettings, "Change Static Light Color");
                        script.lightingSettings.ambientColor = newambientColor;
                        script.lightingSettings.OnValidate();
                        EditorUtility.SetDirty(script.lightingSettings);
                        if (Provider.onlineState == OnlineState.Online)
                            Provider.Checkout(script.lightingSettings, CheckoutMode.Both);
                    }

                    Vector2 newsunPosition = EditorGUILayout.Vector2Field(new GUIContent("Static Sun Position", "The position of the sun. Shadow Casting Objects will cast shadows in the opposite direction."), script.lightingSettings.sunPosition);
                    if (newsunPosition != script.lightingSettings.sunPosition)
                    {
                        Undo.RecordObject(script.lightingSettings, "Change Static Sun Position");
                        script.lightingSettings.sunPosition = newsunPosition;
                        script.lightingSettings.OnValidate();
                        EditorUtility.SetDirty(script.lightingSettings);
                        if (Provider.onlineState == OnlineState.Online)
                            Provider.Checkout(script.lightingSettings, CheckoutMode.Both);
                    }
                }

                int newrequiresLightClipping = (int)EditorGUILayout.Slider(new GUIContent("Requires Light Clipping", "If the sunlightcolor alpha value drops below this value then all requires light object will be able to be seen."), script.lightingSettings.requiresLightClipping, 0, 256);
                if (newrequiresLightClipping != script.lightingSettings.requiresLightClipping)
                {
                    Undo.RecordObject(script.lightingSettings, "Change Requires Light Clipping");
                    script.lightingSettings.requiresLightClipping = newrequiresLightClipping;
                    script.lightingSettings.OnValidate();
                    EditorUtility.SetDirty(script.lightingSettings);
                    if (Provider.onlineState == OnlineState.Online)
                        Provider.Checkout(script.lightingSettings, CheckoutMode.Both);
                }
            }
            EditorGUILayout.Space(5);

            if (script.lightingSettings.doDayNightCycle)
            {
                using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField("Time", EditorStyles.boldLabel);
                    EditorGUILayout.Space(5);

                    float newtimeOfDay = EditorGUILayout.FloatField(new GUIContent("Time of Day", "The current time of day in seconds."), script.timeOfDay);
                    if (newtimeOfDay != script.timeOfDay)
                    {
                        Undo.RecordObject(script, "Change Time of Day");
                        script.timeOfDay = newtimeOfDay;
                        script.OnValidate();
                    }


                    float newtimeScale = EditorGUILayout.FloatField(new GUIContent("Time Scale", "Multiplyer for how mast the time of day passes."), script.timeScale);
                    if (newtimeScale < 0) newtimeScale = 0;
                    if (newtimeScale != script.timeScale)
                    {
                        Undo.RecordObject(script, "Change Time Scale");
                        script.timeScale = newtimeScale;
                        script.OnValidate();
                    }

                    float newlengthOfDay = EditorGUILayout.FloatField(new GUIContent("Length of Day", "How long one day (day light hours only) is in seconds."), script.lightingSettings.lengthOfDay);
                    if (newlengthOfDay != script.lightingSettings.lengthOfDay)
                    {
                        Undo.RecordObject(script.lightingSettings, "Change Length of Day");
                        script.lightingSettings.lengthOfDay = newlengthOfDay;
                        script.lightingSettings.OnValidate();
                        EditorUtility.SetDirty(script.lightingSettings);
                        if (Provider.onlineState == OnlineState.Online)
                            Provider.Checkout(script.lightingSettings, CheckoutMode.Both);
                    }

                    float newlengthOfNight = EditorGUILayout.FloatField(new GUIContent("Length of Night", "How long one night is in seconds."), script.lightingSettings.lengthOfNight);
                    if (newlengthOfNight != script.lightingSettings.lengthOfNight)
                    {
                        Undo.RecordObject(script.lightingSettings, "Change Length of Night");
                        script.lightingSettings.lengthOfNight = newlengthOfNight;
                        script.lightingSettings.OnValidate();
                        EditorUtility.SetDirty(script.lightingSettings);
                        if (Provider.onlineState == OnlineState.Online)
                            Provider.Checkout(script.lightingSettings, CheckoutMode.Both);
                    }


                }
                EditorGUILayout.Space(5);
            }

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Utility", EditorStyles.boldLabel);
                EditorGUILayout.Space(5);

                float newpitchBlackValue = EditorGUILayout.Slider(new GUIContent("Pitch Black Value","The maximum darkness that can appear anywhere in the scene."), script.lightingSettings.pitchBlackValue, 0, 1);
                if (newpitchBlackValue != script.lightingSettings.pitchBlackValue)
                {
                    Undo.RecordObject(script.lightingSettings, "Change Pitch Black Value");
                    script.lightingSettings.pitchBlackValue = newpitchBlackValue;
                    script.lightingSettings.OnValidate();
                    EditorUtility.SetDirty(script.lightingSettings);
                    if (Provider.onlineState == OnlineState.Online)
                        Provider.Checkout(script.lightingSettings, CheckoutMode.Both);
                }

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.FloatField(new GUIContent("Total Raycasts Per Frame","A total number of every raycast from dynamic lights in this scene every frame."), script.totalRaycasts);
                EditorGUI.EndDisabledGroup();

            }
            EditorGUILayout.Space(5);


            if (GUILayout.Button("Bake All Scene Lights"))
            {
                for (int i = 0; i < script.lights.Count; i++)
                {
                    script.lights[i].generateLight = true;
                }
            }
        }
    }
}