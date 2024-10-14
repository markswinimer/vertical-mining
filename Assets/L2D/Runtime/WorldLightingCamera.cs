using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace L2D
{
    /// <summary>
    /// A camera that follows the main camera and captures all objects that would cast shadows from the sun.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class WorldLightingCamera : MonoBehaviour
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

            Cam.backgroundColor = new Color(0, 0, 0, 0);
            Cam.clearFlags = CameraClearFlags.SolidColor;

#if UNITY_EDITOR
            if (Cam.cullingMask != L2DGlobalSettings.Instance.ShadowCastingLayers)
            {
                Cam.cullingMask = L2DGlobalSettings.Instance.ShadowCastingLayers;
                EditorUtility.SetDirty(this);
            }

            if (Cam.targetTexture != L2DAssets.Instance.WorldLightMap)
            {
                Cam.targetTexture = L2DAssets.Instance.WorldLightMap;
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