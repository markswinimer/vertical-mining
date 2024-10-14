using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L2D
{
    /// <summary>
    /// Object that overlays all lighting in the game.
    /// </summary>
    [ExecuteAlways]
    public class LightingOverlay : MonoBehaviour
    {
        MaterialPropertyBlock mpb;
        public MaterialPropertyBlock Mpb
        {
            get
            {
                if (mpb == null)
                    mpb = new MaterialPropertyBlock();
                return mpb;
            }
        }

        MeshRenderer meshRenderer;
        /// <summary>
        /// MeshRanderer attached to this light.
        /// </summary>
        public MeshRenderer MeshRenderer
        {
            get
            {
                if (meshRenderer == null)
                    meshRenderer = GetComponent<MeshRenderer>();
                return meshRenderer;
            }
        }

        public void OnValidate()
        {
#if UNITY_EDITOR
            if (LightingManager.instance == null) return;
            if (LightingManager.instance.lightingSettings == null) LightingManager.instance.LoadLightingSettings();
#endif

            Mpb.SetFloat("_pitchBlackValue", LightingManager.instance.lightingSettings.pitchBlackValue);
            Mpb.SetVector("_sunDirection", LightingManager.instance.currentSunPosition);
            Mpb.SetColor("_sunColor", LightingManager.instance.currentSunColor);
            Mpb.SetFloat("_sunShadowFade", LightingManager.instance.currentSunShadowFade);

            MeshRenderer.SetPropertyBlock(Mpb);
        }

        private void Update()
        {
            transform.position = Camera.main.transform.position + new Vector3(0, 0, 2);
            Vector3 targetScale = new Vector3(2 * Camera.main.orthographicSize * Camera.main.aspect, 2 * Camera.main.orthographicSize, 1);
            SetGlobalScale(targetScale);
            transform.rotation = Camera.main.transform.rotation;
        }

        public void SetGlobalScale(Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }
    }
}