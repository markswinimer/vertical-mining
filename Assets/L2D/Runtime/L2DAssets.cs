using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace L2D
{
    public class L2DAssets : ScriptableObject
    {
        private static L2DAssets instance;
        public static L2DAssets Instance
        {
            get
            {
                if (instance == null)
                    instance = (L2DAssets)CreateInstance(typeof(L2DAssets));
                return instance;
            }
        }

        [SerializeField]
        private Material l2DLightMat;
        /// <summary>
        /// Lighting material used for all lights.
        /// </summary>
        public Material L2DLightMat
        {
            get
            {
#if UNITY_EDITOR
                if (l2DLightMat == null)
                {
                    List<string> paths = new List<string>();
                    string[] result = AssetDatabase.FindAssets("L2DLight");
                    foreach (string guid in result)
                    {
                        paths.Add(AssetDatabase.GUIDToAssetPath(guid));
                    }

                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (!paths[i].Contains(".mat"))
                            continue;

                        l2DLightMat = AssetDatabase.LoadAssetAtPath<Material>(paths[i]);
                    }
                }
#endif

                return l2DLightMat;
            }
        }

        [SerializeField]
        private Material combinedLightingMat;
        /// <summary>
        /// Material that goes on the LightingOverlay
        /// </summary>
        public Material CombinedLightingMat
        {
            get
            {
#if UNITY_EDITOR
                if (combinedLightingMat == null)
                {
                    List<string> paths = new List<string>();
                    string[] result = AssetDatabase.FindAssets("CombinedLighting");
                    foreach (string guid in result)
                    {
                        paths.Add(AssetDatabase.GUIDToAssetPath(guid));
                    }

                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (!paths[i].Contains(".mat"))
                            continue;

                        combinedLightingMat = AssetDatabase.LoadAssetAtPath<Material>(paths[i]);
                    }
                }
#endif

                return combinedLightingMat;
            }
        }

        [SerializeField]
        private GameObject lightingOverlayPrefab;
        /// <summary>
        /// Prefab of the image that overlays lighting for the scene.
        /// </summary>
        public GameObject LightingOverlayPrefab
        {
            get
            {
#if UNITY_EDITOR
                if (lightingOverlayPrefab == null)
                {
                    List<string> paths = new List<string>();
                    string[] result = AssetDatabase.FindAssets("LightingOverlay");
                    foreach (string guid in result)
                    {
                        paths.Add(AssetDatabase.GUIDToAssetPath(guid));
                    }

                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (!paths[i].Contains(".prefab"))
                            continue;

                        lightingOverlayPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(paths[i]);
                    }
                }
#endif

                return lightingOverlayPrefab;
            }
        }

        [SerializeField]
        private RenderTexture lightMap;
        /// <summary>
        /// Render Texture target for the lighitng camera and holds info for all visble lights in the scene.
        /// </summary>
        public RenderTexture LightMap
        {
            get
            {
#if UNITY_EDITOR
                if (lightMap == null)
                {
                    List<string> paths = new List<string>();
                    string[] result = AssetDatabase.FindAssets("LightMap");
                    foreach (string guid in result)
                    {
                        paths.Add(AssetDatabase.GUIDToAssetPath(guid));
                    }

                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (!paths[i].Contains(".renderTexture"))
                            continue;
                        if (paths[i].Contains("World"))
                            continue;
                        if (paths[i].Contains("Color"))
                            continue;

                        lightMap = AssetDatabase.LoadAssetAtPath<RenderTexture>(paths[i]);
                        break;
                    }
                }
#endif

                return lightMap;
            }
        }

        [SerializeField]
        private RenderTexture worldLightMap;
        /// <summary>
        /// Render Texture target for the world lighitng camera and holds info for all shadow casters in the scene.
        /// </summary>
        public RenderTexture WorldLightMap
        {
            get
            {
#if UNITY_EDITOR
                if (worldLightMap == null)
                {
                    List<string> paths = new List<string>();
                    string[] result = AssetDatabase.FindAssets("LightMap");
                    foreach (string guid in result)
                    {
                        paths.Add(AssetDatabase.GUIDToAssetPath(guid));
                    }

                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (!paths[i].Contains(".renderTexture"))
                            continue;
                        if (!paths[i].Contains("World"))
                            continue;

                        worldLightMap = AssetDatabase.LoadAssetAtPath<RenderTexture>(paths[i]);
                        break;
                    }
                }
#endif

                return worldLightMap;
            }
        }

        [SerializeField]
        private RenderTexture colorLightMap;
        /// <summary>
        /// Render Texture target for the world lighitng camera and holds info for all shadow casters in the scene.
        /// </summary>
        public RenderTexture ColorLightMap
        {
            get
            {
#if UNITY_EDITOR
                if (colorLightMap == null)
                {
                    List<string> paths = new List<string>();
                    string[] result = AssetDatabase.FindAssets("LightMap");
                    foreach (string guid in result)
                    {
                        paths.Add(AssetDatabase.GUIDToAssetPath(guid));
                    }

                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (!paths[i].Contains(".renderTexture"))
                            continue;
                        if (!paths[i].Contains("Color"))
                            continue;

                        colorLightMap = AssetDatabase.LoadAssetAtPath<RenderTexture>(paths[i]);
                        break;
                    }
                }
#endif

                return colorLightMap;
            }
        }


        [SerializeField]
        private UniversalRendererData forwardRendererInstance;
        /// <summary>
        /// The ForwardRenderer for the project.
        /// </summary>
        public UniversalRendererData ForwardRendererInstance
        {
            get
            {
#if UNITY_EDITOR
                if (forwardRendererInstance == null)
                {
                    List<string> paths = new List<string>();
                    string[] result = AssetDatabase.FindAssets("ForwardRendererL2D");
                    foreach (string guid in result)
                    {
                        paths.Add(AssetDatabase.GUIDToAssetPath(guid));
                    }

                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (!paths[i].Contains(".asset"))
                            continue;


                        forwardRendererInstance = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(paths[i]);
                        break;
                    }
                }
#endif

                return forwardRendererInstance;
            }
        }

        [SerializeField]
        private UniversalRendererData lightingRenderer;
        /// <summary>
        /// The custom L2D Lighting Renderer for the project.
        /// </summary>
        public UniversalRendererData LightingRenderer
        {
            get
            {
#if UNITY_EDITOR
                if (lightingRenderer == null)
                {
                    List<string> paths = new List<string>();
                    string[] result = AssetDatabase.FindAssets("LightingRendererL2D");
                    foreach (string guid in result)
                    {
                        paths.Add(AssetDatabase.GUIDToAssetPath(guid));
                    }

                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (!paths[i].Contains(".asset"))
                            continue;


                        lightingRenderer = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(paths[i]);
                        break;
                    }
                }
#endif

                return lightingRenderer;
            }
        }

        [SerializeField]
        private RenderPipelineAsset l2DRenderPipelineAsset;
        /// <summary>
        /// The ForwardRenderer for the project.
        /// </summary>
        public RenderPipelineAsset L2DRenderPipelineAsset
        {
            get
            {
#if UNITY_EDITOR
                if (l2DRenderPipelineAsset == null)
                {
                    List<string> paths = new List<string>();
                    string[] result = AssetDatabase.FindAssets("UniversalRP-L2D");
                    foreach (string guid in result)
                    {
                        paths.Add(AssetDatabase.GUIDToAssetPath(guid));
                    }

                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (!paths[i].Contains(".asset"))
                            continue;


                        l2DRenderPipelineAsset = AssetDatabase.LoadAssetAtPath<RenderPipelineAsset>(paths[i]);
                        break;
                    }
                }
#endif

                return l2DRenderPipelineAsset;
            }
        }
    }
}