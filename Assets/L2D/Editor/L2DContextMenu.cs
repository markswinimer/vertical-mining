using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEditor;
using System.Reflection;

namespace L2D
{
    [ExecuteInEditMode]
    public class L2DContextMenu : MonoBehaviour
    {
        [MenuItem("GameObject/L2D/Area Light", false, -10)]
        static void SpawnAreaLight()
        {
            GameObject spawn = new GameObject();
            spawn.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = spawn.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = L2DAssets.Instance.L2DLightMat;
            spawn.AddComponent<AreaLight>();
            Debug.Log("Please choose a mesh for this area light.");
            spawn.name = "Area Light";
            int layerNumber = -1;
            int layer = L2DGlobalSettings.Instance.LightingLayers;
            while (layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }
            if (layerNumber < 0)
                layerNumber = 0;
            spawn.layer = layerNumber;

            Undo.RegisterCreatedObjectUndo(spawn, "Create " + spawn.name);
            Selection.activeObject = spawn;
        }
        
        [MenuItem("GameObject/L2D/Cone Light", false, -10)]
        public static void SpawnConeLight()
        {
            GameObject spawn = new GameObject();
            spawn.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = spawn.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = L2DAssets.Instance.L2DLightMat;
            spawn.AddComponent<ConeLight>();
            spawn.name = "Cone Light";
            int layerNumber = -1;
            int layer = L2DGlobalSettings.Instance.LightingLayers;
            while (layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }
            if (layerNumber < 0)
                layerNumber = 0;
            spawn.layer = layerNumber;

            Undo.RegisterCreatedObjectUndo(spawn, "Create " + spawn.name);
            Selection.activeObject = spawn;
        }

        [MenuItem("GameObject/L2D/Fire Light", false, -10)]
        public static void SpawnFireLight()
        {
            GameObject spawn = new GameObject();
            spawn.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = spawn.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = L2DAssets.Instance.L2DLightMat;
            spawn.AddComponent<FireLight>();
            spawn.name = "Fire Light";
            int layerNumber = -1;
            int layer = L2DGlobalSettings.Instance.LightingLayers;
            while (layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }
            if (layerNumber < 0)
                layerNumber = 0;
            spawn.layer = layerNumber;

            Undo.RegisterCreatedObjectUndo(spawn, "Create " + spawn.name);
            Selection.activeObject = spawn;
        }

        [MenuItem("GameObject/L2D/Point Light", false, -10)]
        public static void SpawnPointLight()
        {
            GameObject spawn = new GameObject();
            spawn.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = spawn.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = L2DAssets.Instance.L2DLightMat;
            spawn.AddComponent<PointLight>();
            spawn.name = "Point Light";
            int layerNumber = -1;
            int layer = L2DGlobalSettings.Instance.LightingLayers;
            while (layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }
            if (layerNumber < 0)
                layerNumber = 0;
            spawn.layer = layerNumber;

            Undo.RegisterCreatedObjectUndo(spawn, "Create " + spawn.name);
            Selection.activeObject = spawn;
        }

        [MenuItem("GameObject/L2D/Spot Light", false, -10)]
        public static void SpawnSpotLight()
        {
            GameObject spawn = new GameObject();
            spawn.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = spawn.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = L2DAssets.Instance.L2DLightMat;
            SpotLight spotLight = spawn.AddComponent<SpotLight>();
            spotLight.blendScale = 2;
            spawn.name = "Spot Light";
            int layerNumber = -1;
            int layer = L2DGlobalSettings.Instance.LightingLayers;
            while (layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }
            if (layerNumber < 0)
                layerNumber = 0;
            spawn.layer = layerNumber;

            Undo.RegisterCreatedObjectUndo(spawn, "Create " + spawn.name);
            Selection.activeObject = spawn;
        }

        [MenuItem("L2D/Run L2D Setup")]
        public static void RunSetup()
        {
            L2DGlobalSettings.Instance.LightingLayers = 536870912;
            L2DGlobalSettings.Instance.ShadowCastingLayers = 1073741824;
            L2DGlobalSettings.Instance.RequiresLightLayers = -2147483648;

            PlayerSettings.colorSpace = ColorSpace.Linear;

            QualitySettings.renderPipeline = L2DAssets.Instance.L2DRenderPipelineAsset;
            L2DSettingProvider.UpdateRendererLayers();

            RenderPipelineAsset rpa = L2DAssets.Instance.L2DRenderPipelineAsset;
            ScriptableRenderer sr0 = null;
            bool error = false;
            try
            {
                sr0 = ((UniversalRenderPipelineAsset)rpa).GetRenderer(0);
            }
            catch
            {
                error = true;
            }
            List<ScriptableRendererFeature> scriptableRendererFeatures;
            if (sr0 == null)
            {
                error = true;
            }
            else
            {
                scriptableRendererFeatures = (List<ScriptableRendererFeature>)sr0.GetType().GetProperty("rendererFeatures", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sr0);
                if (scriptableRendererFeatures.Count == 0 || scriptableRendererFeatures[0].name != "LightingStencil")
                {
                    error = true;
                }
            }
            
            if (error)
            {
                Debug.LogError("L2D setup: ACTION REQUIRED!! Please go to L2D->Rendering->UniversalRP-L2D.asset and set the renderer in position 0 to the ForwardRendererL2D.asset file.", rpa);
                Selection.activeObject = rpa;
                EditorApplication.update += URPAssetChecker;


            }
            else
            {
                Debug.Log("L2D is fully imported and ready to use! You can now look at example scenes and/or create your own scene by clicking the \"Quick Import to Scene\" button.", AssetDatabase.LoadAssetAtPath<SceneAsset>("Packages/com.rgbstudios.l2d/Examples/Scenes/Soft Shadows.unity"));
            }
        }

        public static void URPAssetChecker()
        {
            RenderPipelineAsset rpa = L2DAssets.Instance.L2DRenderPipelineAsset;
            ScriptableRenderer sr0 = null;
            bool error = false;
            try
            {
                sr0 = ((UniversalRenderPipelineAsset)rpa).GetRenderer(0);
            }
            catch
            {
                error = true;
            }
            List<ScriptableRendererFeature> scriptableRendererFeatures;
            if (sr0 == null)
            {
                error = true;
            }
            else
            {
                scriptableRendererFeatures = (List<ScriptableRendererFeature>)sr0.GetType().GetProperty("rendererFeatures", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sr0);
                if (scriptableRendererFeatures.Count == 0 || scriptableRendererFeatures[0].name != "LightingStencil")
                {
                    error = true;
                }
            }

            if (error)
            {
                Debug.LogError("L2D setup: ACTION REQUIRED!! Please go to L2D->Rendering->UniversalRP-L2D.asset and set the renderer in position 0 to the ForwardRendererL2D.asset file.", rpa);
                return;
            }
            else
            {
                Debug.Log("L2D is fully imported and ready to use! You can now look at example scenes and/or create your own scene by clicking the \"Quick Import to Scene\" button.", AssetDatabase.LoadAssetAtPath<SceneAsset>("Packages/com.rgbstudios.l2d/Examples/Scenes/Soft Shadows.unity"));
            }


            EditorApplication.update -= URPAssetChecker;
        }
        

        [MenuItem("L2D/Quick Import to Scene")]
        public static void QuickImport()
        {
            try
            {
                L2DSettingProvider.UpdateRendererLayers();

                bool changed = false;

                if (Camera.main != null)
                {
                    UniversalAdditionalCameraData camData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
                    if (camData == null) camData = Camera.main.gameObject.AddComponent<UniversalAdditionalCameraData>();
                    camData.SetRenderer(0);
                    changed = true;
                }

                if (LightingManager.instance == null)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    GameObject spawn = new GameObject();
                    spawn.name = "Lighting Manager";
                    LightingManager.instance = spawn.AddComponent<LightingManager>();
                    changed = true;
                }
                if (FindObjectOfType<LightingCamera>() == null)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    GameObject spawn = new GameObject();
                    spawn.name = "Lighting Camera";
                    Camera cam = spawn.AddComponent<Camera>();
                    spawn.AddComponent<LightingCamera>();
                    spawn.transform.parent = Camera.main.transform;

                    cam.targetTexture = L2DAssets.Instance.LightMap;
                    UniversalAdditionalCameraData data = spawn.AddComponent<UniversalAdditionalCameraData>();
                    data.SetRenderer(1);
                    changed = true;
                }
                if (FindObjectOfType<WorldLightingCamera>() == null)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    GameObject spawn = new GameObject();
                    spawn.name = "World Lighting Camera";
                    Camera cam = spawn.AddComponent<Camera>();
                    spawn.AddComponent<WorldLightingCamera>();
                    spawn.transform.parent = Camera.main.transform;

                    cam.targetTexture = L2DAssets.Instance.WorldLightMap;
                    UniversalAdditionalCameraData data = spawn.AddComponent<UniversalAdditionalCameraData>();
                    data.SetRenderer(0);
                    changed = true;
                }
                if (FindObjectOfType<ColorLightingCamera>() == null)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    GameObject spawn = new GameObject();
                    spawn.name = "Color Lighting Camera";
                    Camera cam = spawn.AddComponent<Camera>();
                    spawn.AddComponent<ColorLightingCamera>();
                    spawn.transform.parent = Camera.main.transform;

                    cam.targetTexture = L2DAssets.Instance.ColorLightMap;
                    UniversalAdditionalCameraData data = spawn.AddComponent<UniversalAdditionalCameraData>();
                    data.SetRenderer(1);
                    changed = true;
                }

                Volume[] volumes = FindObjectsOfType<Volume>();
                bool volumeExits = false;
                for (int i = 0; i < volumes.Length; i++)
                {
                    if (((1 << volumes[i].gameObject.layer) & L2DGlobalSettings.Instance.LightingLayers) > 0)
                    {
                        volumeExits = true;
                    }
                }
                if (!volumeExits)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    GameObject spawn = new GameObject();
                    spawn.name = "Lighting Post Processing";
                    Volume vol = spawn.AddComponent<Volume>();
                    spawn.transform.parent = Camera.main.transform;
                    int layerNumber = -1;
                    int layer = L2DGlobalSettings.Instance.LightingLayers;
                    while (layer > 0)
                    {
                        layer = layer >> 1;
                        layerNumber++;
                    }
                    if (layerNumber < 0)
                        layerNumber = 0;
                    spawn.layer = layerNumber;
                    changed = true;
                }

                if (changed)
                {
                    LightBase[] lights = Resources.FindObjectsOfTypeAll<LightBase>();
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].gameObject.SetActive(true);
                    }

                    Debug.Log("L2D import successful! Add lights using the GameObject->L2D menu.");
                }
                else
                {
                    Debug.LogWarning("L2D import found nothing to import. Your scene should already be good to go.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("L2D Import failed!!  " + e.ToString());
                RemoveLighting();
            }
        }

        [MenuItem("L2D/Remove Lighting from Scene")]
        public static void RemoveLighting()
        {
            if (LightingManager.instance != null)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                while (LightingManager.instance.lights.Count > 0)
                {
                    LightingManager.instance.lights[0].gameObject.SetActive(false);
                }
                DestroyImmediate(LightingManager.instance.lightingOverlay.gameObject);
                DestroyImmediate(LightingManager.instance.gameObject);
            }
            LightingCamera light = FindObjectOfType<LightingCamera>();
            if (light != null)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                DestroyImmediate(light.gameObject);
            }
            WorldLightingCamera world = FindObjectOfType<WorldLightingCamera>();
            if (world != null)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                DestroyImmediate(world.gameObject);
            }
            ColorLightingCamera color = FindObjectOfType<ColorLightingCamera>();
            if (color != null)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                DestroyImmediate(color.gameObject);
            }
        }

        [MenuItem("L2D/Open Settings")]
        public static void OpenSettings()
        {
            SettingsService.OpenProjectSettings("Project/L2D");
        }
    }
}