using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEditor;

namespace L2D
{
    /// <summary>
    /// A lighting camera that follows the main camera and captures all lights in the render view.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class LightingCamera : MonoBehaviour
    {
        public bool debug = true;

        Camera cam;
        Camera Cam
        {
            get
            {
                if (cam == null)
                    cam = GetComponent<Camera>();
                return cam;
            }
        }

        UniversalAdditionalCameraData camData;
        UniversalAdditionalCameraData CamData
        {
            get
            {
                if (camData == null)
                    camData = GetComponent<UniversalAdditionalCameraData>();
                return camData;
            }
        }

        private void Update()
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;

            Cam.orthographic = Camera.main.orthographic;
            Cam.orthographicSize = Camera.main.orthographicSize;
            Cam.aspect = Camera.main.aspect;
            Cam.farClipPlane = Camera.main.farClipPlane;
            Cam.nearClipPlane = Camera.main.nearClipPlane;
            Cam.fieldOfView = Camera.main.fieldOfView;

            CamData.renderPostProcessing = true;
            Cam.backgroundColor = new Color(0, 0, 0, 0);
            Cam.clearFlags = CameraClearFlags.SolidColor;
            CamData.volumeLayerMask = ~0;

#if UNITY_EDITOR
            if (Cam.cullingMask != L2DGlobalSettings.Instance.LightingLayers)
            {
                Cam.cullingMask = L2DGlobalSettings.Instance.LightingLayers;
                EditorUtility.SetDirty(this);
            }

            if (Cam.targetTexture != L2DAssets.Instance.LightMap)
            {
                Cam.targetTexture = L2DAssets.Instance.LightMap;
                EditorUtility.SetDirty(this);
            }
#endif

            if (Cam.targetTexture.height != Screen.height || Cam.targetTexture.width != Screen.width)
            {
                RenderTexture renderTexture = Cam.targetTexture;
                renderTexture.Release();
                renderTexture.width = Screen.width;
                renderTexture.height = Screen.height;
            }
        }

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }

        private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (camera == Cam && debug)
            {
                LightingManager LM = FindObjectOfType<LightingManager>();

                for (int i = 0; i < LM.lights.Count; i++)
                {
                    LM.lights[i].PrepareLightPass();
                }
            }
        }

        private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (camera == Cam)
            {
                LightingManager LM = FindObjectOfType<LightingManager>();

                for (int i = 0; i < LM.lights.Count; i++)
                {
                    LM.lights[i].PrepareColorPass();
                }
            }
        }
    }
}