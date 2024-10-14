using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L2D
{
    /// <summary>
    /// A light in the shape of whatever mesh is provided to it.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class AreaLight : LightBase
    {
        /// <summary>
        /// Enable the light.
        /// </summary>
        public override void Bake()
        {
            mesh = MeshFilter.sharedMesh;

            if (validate)
            {
                validate = false;
                Mpb.SetColor("_color", color);
                Mpb.SetFloat("_viewDistance", radius);
                if (LightingManager.instance.lightingSettings != null)
                    Mpb.SetFloat("_pitchBlackValue", LightingManager.instance.lightingSettings.pitchBlackValue);

                MeshRenderer.SetPropertyBlock(Mpb);
            }
        }

        /// <summary>
        /// Does nothing on none generated lights.
        /// </summary>
        public override void UnBake()
        {
            // prevent base.UnBake() from being ran
        }
    }
}
