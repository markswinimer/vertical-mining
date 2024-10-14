using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace L2D
{
    /// <summary>
    /// Singleton that manages all lights in the scene and generates sunlight for the scene.
    /// </summary>
    [ExecuteAlways]
    public class LightingManager : MonoBehaviour
    {
        //[HideInInspector]
        //public Texture2D texture2D;

        /// <summary>
        /// Current time of day/night in seconds according to the lighting system.
        /// </summary>
        public float timeOfDay = 0;
        /// <summary>
        /// Current imagined sun/moon position. Used to create shadows under all shadow casting objects.
        /// </summary>
        public Vector2 currentSunPosition { get; private set; }
        /// <summary>
        /// Current color of the sun/moon. Is used as ambient light color for the whole scene.
        /// </summary>
        public Color currentSunColor { get; private set; }
        /// <summary>
        /// Current alpha for the shadows in the scene.
        /// </summary>
        public float currentSunShadowFade { get; private set; } = 1;

        /// <summary>
        /// How quickly the timeOfDay passes.
        /// </summary>
        public float timeScale = 1;

        /// <summary>
        /// In scene instance of LightingManager.
        /// </summary>
        public static LightingManager instance;
        /// <summary>
        /// All lights placed in the scene.
        /// </summary>
        public List<LightBase> lights = new List<LightBase>();
        /// <summary>
        /// Debug count of the total number of raycasts that will be fired every frame in this scene, given the current camera angles.
        /// </summary>
        public float totalRaycasts;

        /// <summary>
        /// Instance of the LightingImage for this scene.
        /// </summary>
        [HideInInspector]
        public LightingOverlay lightingOverlay;

#if UNITY_EDITOR
        private bool activeState = true;
        private bool activeStateRefresh = true;
#endif
        /// <summary>
        /// LightingSettings asset used for this scene.
        /// </summary>
        public LightingSettings2D lightingSettings;

        private void Start()
        {
            lightingOverlay.gameObject.SetActive(true);

        }

        private void OnEnable()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                DestroyImmediate(gameObject);

            lights.Clear();
            LightBase[] found = FindObjectsOfType<LightBase>();
            for (int i = 0; i < found.Length; i++)
            {
                lights.Add(found[i]);
            }
            
                
            if (lightingOverlay == null)
            {
                lightingOverlay = FindObjectOfType<LightingOverlay>();
                if (lightingOverlay == null)
                {
                    GameObject spawn = Instantiate(L2DAssets.Instance.LightingOverlayPrefab);
                    spawn.name = "LightingOverlay";
                    lightingOverlay = spawn.GetComponent<LightingOverlay>();
                }
            }

            OnValidate();
#if UNITY_EDITOR
            activeStateRefresh = true;
            EditorApplication.update += Update;
#endif
        }

        int layer;

        public void OnValidate()
        {
            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i] != null)
                    lights[i].OnValidate();
                else
                {
                    OnEnable();
                    return;
                }
            }
#if UNITY_EDITOR
            int layer = L2DGlobalSettings.Instance.LightingLayers;
#endif

        }

        private void Update()
        {
            if (instance != this)
                Destroy(gameObject);
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if ((activeStateRefresh || activeState) && !SceneView.lastActiveSceneView.sceneLighting)
                {
                    activeStateRefresh = false;
                    activeState = false;
                    for (int i = 0; i < lights.Count; i++)
                    {
                        lights[i].DeActivateSceneLighting();
                    }
                    lightingOverlay.gameObject.SetActive(false);
                }
                else if ((activeStateRefresh || !activeState) && SceneView.lastActiveSceneView.sceneLighting)
                {
                    activeStateRefresh = false;
                    activeState = true;
                    for (int i = 0; i < lights.Count; i++)
                    {
                        lights[i].ActivateSceneLighting();
                    }
                    lightingOverlay.gameObject.SetActive(true);
                }
            }

            if (lightingSettings == null)
            {
                LoadLightingSettings();
            }
#endif

            if (lightingSettings.validate)
            {
                lightingSettings.validate = false;
                OnValidate();
            }

            if (lightingSettings.doDayNightCycle)
            {
                timeOfDay += Time.deltaTime * timeScale;
                if (timeOfDay > lightingSettings.lengthOfDNCycle)
                    timeOfDay -= lightingSettings.lengthOfDNCycle;

                currentSunPosition = lightingSettings.GetSunMoonPosition(timeOfDay);
                currentSunColor = lightingSettings.GetAmbientColor(timeOfDay);
                currentSunShadowFade = lightingSettings.GetShadowFade(timeOfDay);
            }
            else
            {
                currentSunPosition = lightingSettings.sunPosition;
                currentSunColor = lightingSettings.ambientColor;
                currentSunShadowFade = 0;
            }

            if (lightingOverlay != null)
            {
                lightingOverlay.OnValidate();
                if (currentSunColor.a < lightingSettings.requiresLightClipping/255f)
                {
                    int layerNumber = -1;
                    while (layer > 0)
                    {
                        layer = layer >> 1;
                        layerNumber++;
                    }
                    if (layerNumber < 0)
                        layerNumber = 0;
                    lightingOverlay.transform.GetChild(0).gameObject.layer = layerNumber;
                }
                else
                {
                    lightingOverlay.transform.GetChild(0).gameObject.layer = 0;
                }
            }
            else
            {
                OnEnable();
                return;
            }

            



#if UNITY_EDITOR
            totalRaycasts = 0;
            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i].active && lights[i].dynamicLight)
                    totalRaycasts += lights[i].rayCount;
            }
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// Forces Lighting manager to have an get an instance of LightingSettings.
        /// </summary>
        public void LoadLightingSettings()
        {
            string path = SceneManager.GetActiveScene().path;
            path = path.Remove(path.Length - 6);
            System.IO.Directory.CreateDirectory(path);
            path = path.Insert(path.Length, "/LightingSettings2D " + SceneManager.GetActiveScene().name);
            path += ".asset";
            lightingSettings = AssetDatabase.LoadAssetAtPath<LightingSettings2D>(path);
            if (lightingSettings == null)
            {
                lightingSettings = (LightingSettings2D)ScriptableObject.CreateInstance(typeof(LightingSettings2D));
                AssetDatabase.CreateAsset(lightingSettings, path);
            }
        }

        private void OnDisable()
        {
            EditorApplication.update -= Update;

        }
#endif
    }
}