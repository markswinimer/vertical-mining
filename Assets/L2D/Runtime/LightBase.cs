using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace L2D
{
    /// <summary>
    /// Parent class for all lights
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class LightBase : MonoBehaviour
    {

        /// <summary>
        /// Color of the light given off by the light.
        /// </summary>
        [ColorUsageAttribute(true, true)] public Color color = new Color(1, 1, 1, 1f);
        /// <summary>
        /// Controls how much darkness is removed by the light.
        /// </summary>
        [Range(0f, 2f)] public float lightIntensity = 0.7f;
        /// <summary>
        /// Controls how much color is added to the area of the light.
        /// </summary>
        [Range(0f, 2f)] public float colorIntensity = 0.3f;
        /// <summary>
        /// Amount of raycasts used to generate the mesh for the  light.
        /// </summary>
        [Range(3, 1000)] public int rayCount = 50;
        /// <summary>
        /// How far the light can shine.
        /// </summary>
        public float radius = 3;

        public float blendScale = 0.7f;

        [HideInInspector]
        public LayerMask layer;
        /// <summary>
        /// Layer that the raycast will collide with, resulting in a shadow.
        /// </summary>
        public LayerMask layerMask {
            get
            {
#if UNITY_EDITOR
                if (layer != L2DGlobalSettings.Instance.ShadowCastingLayers)
                {
                    layer = L2DGlobalSettings.Instance.ShadowCastingLayers;
                    EditorUtility.SetDirty(this);
                }
#endif
                return layer;
            }
        }
        /// <summary>
        /// If true, the light will continue to generate every frame.
        /// </summary>
        public bool dynamicLight = true;
        /// <summary>
        /// When flipped true the light will generate and then flip this varible back to false.
        /// </summary>
        public bool generateLight = false;
        /// <summary>
        /// If true, the light can generate while in EditMode.
        /// </summary>
        public bool runInEditor = true;

        MaterialPropertyBlock mpb;
        /// <summary>
        /// MaterialPropertyBlock for this light.
        /// </summary>
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

        MeshFilter meshFilter;
        /// <summary>
        /// MeshFilter attached to this light.
        /// </summary>
        public MeshFilter MeshFilter
        {
            get
            {
                if (meshFilter == null)
                    meshFilter = GetComponent<MeshFilter>();
                return meshFilter;
            }
        }

        protected Mesh mesh;
        /// <summary>
        /// Lights outside of camera view are deactivated.
        /// </summary>
        public bool inView { get; protected set; } = true;
        /// <summary>
        /// Only active lights can be baked. This is toggled by the scene lighting toggled and only affects lights in editmode.
        /// </summary>
        public bool active { get; protected set; } = true;
        

        protected bool validate = true;
        public void OnValidate()
        {
            validate = true;
        }

        private bool needToEnable;
        private void OnEnable()
        {
            validate = true;
            if (LightingManager.instance == null)
            {
                needToEnable = true;
                return;
            }

            if (!LightingManager.instance.lights.Contains(this))
                LightingManager.instance.lights.Add(this);

            PrepareColorPass();
            Bake();
        }

        
        private void OnDisable()
        {
            LightingManager.instance.lights.Remove(this);
        }

        public void Update()
        {
#if UNITY_EDITOR

            if (LightingManager.instance == null && gameObject.activeSelf)
            {
                Debug.LogError("No instance of Lighting Manager Found! Try using quick import under the L2D tab.");
                return;
            }

            if (!runInEditor && !Application.isPlaying)
            {
                UnBake();
                return;
            }

            if (!SceneVisibilityManager.instance.IsHidden(gameObject, false))
                SceneVisibilityManager.instance.Hide(gameObject, false);
#endif
            if (needToEnable)
            {
                needToEnable = false;
                if (!LightingManager.instance.lights.Contains(this))
                    LightingManager.instance.lights.Add(this);
            }

            if ((generateLight && active) || (dynamicLight && inView && active))
            {
                generateLight = false;
                Bake();
            }
        }

        /// <summary>
        /// Generate the light.
        /// </summary>
        public virtual void Bake()
        {
            
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(1 / transform.lossyScale.x, 1 / transform.lossyScale.y, 1 / transform.lossyScale.z);

            if (mesh == null)
            {
                mesh = new Mesh();
                mesh.name = "Generated Mesh";
                MeshFilter.mesh = mesh;
            }
            else if (MeshFilter.sharedMesh != mesh)
            {
                MeshFilter.sharedMesh = mesh;
            }

            if (validate)
            {
                validate = false;
                Mpb.SetColor("_color", color);
                Mpb.SetFloat("_viewDistance", radius * blendScale);
                if (LightingManager.instance.lightingSettings != null)
                    Mpb.SetFloat("_pitchBlackValue", LightingManager.instance.lightingSettings.pitchBlackValue);
                
                MeshRenderer.SetPropertyBlock(Mpb);
            }
        }

        /// <summary>
        /// Destroy this light's data.
        /// </summary>
        public virtual void UnBake()
        {
            mesh = null;
            MeshFilter.mesh = null;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Called when the user enables scene lighting.
        /// </summary>
        public virtual void ActivateSceneLighting()
        {
            active = true;
            Bake();
        }

        /// <summary>
        /// Called when the user disables scene lighting.
        /// </summary>
        public virtual void DeActivateSceneLighting()
        {
            active = false;
            UnBake();
        }
#endif

        private void OnBecameVisible()
        {
            inView = true;
        }

        private void OnBecameInvisible()
        {
            inView = false;
        }

        /// <summary>
        /// Called before darkness removal render pass.
        /// </summary>
        public void PrepareLightPass()
        {
            Mpb.SetFloat("_intensity", lightIntensity > 1? lightIntensity+(lightIntensity-1)*3 : lightIntensity);
            MeshRenderer.SetPropertyBlock(Mpb);
        }

        /// <summary>
        /// Called before Color addition render pass.
        /// </summary>
        public void PrepareColorPass()
        {
            Mpb.SetFloat("_intensity", colorIntensity);
            MeshRenderer.SetPropertyBlock(Mpb);
        }

        /// <summary>
        /// Used to rotate a vertex of the mesh around the origin of a light in order to prevent problems caused by transform rotation.
        /// </summary>
        /// <param name="point">The vertex to be rotated.</param>
        /// <param name="pivot">The origin to rotate around.</param>
        /// <param name="angle">How much to rotate.</param>
        /// <returns></returns>
        public Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle)
        {
            return angle * (point - pivot) + pivot;
        }
    }
}