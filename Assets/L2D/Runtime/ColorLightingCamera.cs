using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEditor;

namespace L2D
{
    /// <summary>
    /// A lighting camera that follows the main camera and renders color for lights.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class ColorLightingCamera : MonoBehaviour
    {
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

            if (Cam.targetTexture != L2DAssets.Instance.ColorLightMap)
            {
                Cam.targetTexture = L2DAssets.Instance.ColorLightMap;
                EditorUtility.SetDirty(this);
            }
#endif

            if (Cam.targetTexture.height != Screen.height || Cam.targetTexture.width != Screen.width)
            {
                RenderTexture renderTexture = Cam.targetTexture;
                renderTexture.Release();
                renderTexture.width = Screen.width;
                renderTexture.height = Screen.height;
                renderTexture.format = RenderTextureFormat.ARGB32;
            }

        }
    }
}