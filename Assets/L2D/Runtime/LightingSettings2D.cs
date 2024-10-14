using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L2D
{
    /// <summary>
    /// All per-scene settings regaurding lighting.
    /// </summary>
    [CreateAssetMenu(fileName = "LightingSettings2D", menuName = "L2D/LightingSettings2D")]
    public class LightingSettings2D : ScriptableObject
    {
        /// <summary>
        /// Color of the sunlight thoughout the scene. Only used if doDayNightCycle is false.
        /// </summary>
        public Color ambientColor = new Color(0, 0, 0, 0.95f);
        /// <summary>
        /// Position of the sun to control direction of shadows. Only used if doDayNightCycle is false.
        /// </summary>
        public Vector2 sunPosition = new Vector2(1, 1);
        /// <summary>
        /// If true, the scene will progress through time and update sun color, intesity and position automatically.
        /// </summary>
        public bool doDayNightCycle = false;
        /// <summary>
        /// A gradient that controls the ambient light color over time, this would be anywhere there is a lack of scene lights. Only used if doDayNightCycle is true.
        /// </summary>
        public Gradient ambientLightColorOverCycle = new Gradient()
        {
            colorKeys = new GradientColorKey[7] {
                // Add your colour and specify the stop point
                new GradientColorKey(new Color(0.75f, 0.55f, 0.45f), 0),
                new GradientColorKey(new Color(1, 0.95f, 0.55f), 0.08f),
                new GradientColorKey(new Color(1, 0.95f, 0.55f), 0.48f),
                new GradientColorKey(new Color(0.55f, 0.18f, 0.1f), 0.54f),
                new GradientColorKey(new Color(0, 0, 0), 0.6f),
                new GradientColorKey(new Color(0, 0, 0), 0.94f),
                new GradientColorKey(new Color(0.75f, 0.55f, 0.45f), 1)
                },
            alphaKeys = new GradientAlphaKey[7] {
                new GradientAlphaKey(0.43f, 0),
                new GradientAlphaKey(0.2f, 0.08f),
                new GradientAlphaKey(0.2f, 0.48f),
                new GradientAlphaKey(0.53f, 0.54f),
                new GradientAlphaKey(1, 0.6f),
                new GradientAlphaKey(1, 0.94f),
                new GradientAlphaKey(0.43f, 1)
            }
        };
        /// <summary>
        /// A curve that controls the position of the sun. Only used if doDayNightCycle is true.
        /// </summary>
        public AnimationCurve SunPositionOverDay = new AnimationCurve(new Keyframe(-1, 0), new Keyframe(0, 2), new Keyframe(1, 0));
        /// <summary>
        /// A curve that controls the position of the moon. Only used if doDayNightCycle is true.
        /// </summary>
        public AnimationCurve MoonPositionOverNight = new AnimationCurve(new Keyframe(-0.5f, 0), new Keyframe(0, 1), new Keyframe(0.5f, 0));
        /// <summary>
        /// How long a day last from dusk to dawn in seconds. Only used if doDayNightCycle is true.
        /// </summary>
        public float lengthOfDay = 60f;
        /// <summary>
        /// How long a night last from dawn to dusk in seconds. Only used if doDayNightCycle is true.
        /// </summary>
        public float lengthOfNight = 60f;
        /// <summary>
        /// Total length of lengthOfNight + lengthOfDay in seconds.
        /// </summary>
        public float lengthOfDNCycle { get { return lengthOfNight + lengthOfDay; } }
        /// <summary>
        /// How close the lighting system can get to pitch black.
        /// </summary>
        [Range(0f, 1f)] public float pitchBlackValue = 0.95f;
        /// <summary>
        /// If the sunlightcolor alpha value drops below this value then all requires light object will be able to be seen.
        /// </summary>
        [Range(0, 255)] public int requiresLightClipping = 100;
        /// <summary>
        /// How many seconds the ambient shadows should take to fade out when switching from day to night and night to day.
        /// </summary>
        public float shadowFadeTime = 3;

        /// <summary>
        /// Flag if set true will force all lighting objects to validate themselves.
        /// </summary>
        [HideInInspector] public bool validate = false;
        public void OnValidate()
        {
            validate = true;
        }
        
        /// <summary>
        /// Returns the sunlight color at a given time of day. Time must be less than lengthOfDNCycle.
        /// </summary>
        /// <param name="time">Time of day.</param>
        /// <returns></returns>
        public Color GetAmbientColor(float time)
        {
            float t;
            if (time < lengthOfDay)
            {
                t = time / lengthOfDay / 2;
                return ambientLightColorOverCycle.Evaluate(t);
            }
            else
            {
                t = (time - lengthOfDay) / lengthOfNight / 2 + 0.5f;
                return ambientLightColorOverCycle.Evaluate(t);
            }
        }

        /// <summary>
        /// Returns the sun/moon position at a given time of day. Time must be less than lengthOfDNCycle.
        /// </summary>
        /// <param name="time">Time of day.</param>
        /// <returns></returns>
        public Vector2 GetSunMoonPosition(float time)
        {
            float t;
            if (time < lengthOfDay)
            {
                if (SunPositionOverDay.keys.Length == 0)
                {
                    Debug.LogError("You must add points to the SunPosOverDay curve for doDayNightCycle to work. Look at the lighting manager.");
                    return Vector2.zero;
                }
                t = time / lengthOfDay;
                float width = SunPositionOverDay.keys[SunPositionOverDay.keys.Length - 1].time - SunPositionOverDay.keys[0].time;
                t = -(t * width + SunPositionOverDay.keys[0].time);
                return new Vector2 (t, SunPositionOverDay.Evaluate(t));
            }
            else
            {
                if (MoonPositionOverNight.keys.Length == 0)
                {
                    Debug.LogError("You must add points to the MoonPosOverNight curve for doDayNightCycle to work. Look at the lighting manager.");
                    return Vector2.zero;
                }
                t = (time - lengthOfDay) / lengthOfNight;
                float width = MoonPositionOverNight.keys[MoonPositionOverNight.keys.Length - 1].time - MoonPositionOverNight.keys[0].time;
                t = -(t * width + MoonPositionOverNight.keys[0].time);
                return new Vector2 (t, MoonPositionOverNight.Evaluate(t));
            }
        }

        /// <summary>
        /// Returns a percentage of shadow fade based on the time in DNCycle.
        /// </summary>
        /// <param name="time">Time of day.</param>
        /// <returns></returns>
        public float GetShadowFade(float time)
        {
            if (time < shadowFadeTime)
                return 1 - time / shadowFadeTime;
            if (time < lengthOfDay && time > lengthOfDay - shadowFadeTime)
                return 1 - (time - lengthOfDay) / -shadowFadeTime;

            if (time > lengthOfDay)
            {
                float t = time - lengthOfDay;

                if (t < shadowFadeTime)
                    return 1 - t / shadowFadeTime;
                if (t < lengthOfNight && t > lengthOfNight - shadowFadeTime)
                    return 1 - (t - lengthOfNight) / -shadowFadeTime;
            }

            return 0;
        }
    }
}