using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace L2D
{
    /// <summary>
    /// A light the projects out in the shape of a cone.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ConeLight : LightBase
    {
        /// <summary>
        /// If true, the light will always point towards the mouse pointer.
        /// </summary>
        public bool followMouse = false;
        /// <summary>
        /// Field of view in degrees for the cone.
        /// </summary>
        [Range(0.001f, 360f)] public float fov = 90;
        /// <summary>
        /// Direction in degrees the cone is facing.
        /// </summary>
        public float aimDirection = 0;
        float startingAngle;
        private Vector3 origin;

        private void Start()
        {
            GetComponent<MeshFilter>().mesh = mesh;
            origin = Vector3.zero;
            if (Application.isPlaying)
            {
                generateLight = true;
            }
        }


        Vector3[] vertices;
        Vector2[] uv;
        int[] triangles;
        int oldRayCount = -2;

        /// <summary>
        /// Generate the light.
        /// </summary>
        public override void Bake()
        {
            base.Bake();

            origin = transform.position;

            if (followMouse && Application.isPlaying)
            {
                Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouse -= transform.position;
                SetAimDirection(mouse);
            }

            float angle = Mathf.Deg2Rad * aimDirection + (Mathf.Deg2Rad * fov) / 2;
            angle += transform.eulerAngles.z * Mathf.Deg2Rad;
            float angleIncrease = (Mathf.Deg2Rad * fov) / rayCount;

            if (oldRayCount != rayCount)
            {
                vertices = new Vector3[rayCount + 2];
                uv = new Vector2[vertices.Length];
                triangles = new int[rayCount * 3];
            }

            vertices[0] = Vector3.zero;

            int vertexIndex = 1;
            int triangleIndex = 0;

            for (int i = 0; i <= rayCount; i++)
            {
                Vector3 vertex;
                RaycastHit2D raycastHit = Physics2D.Raycast(origin, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0), radius, layerMask);
                if (raycastHit.collider == null)
                {
                    vertex = origin + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                }
                else
                {
                    vertex = raycastHit.point;
                }
                vertex = RotateAroundPoint(vertex, origin, new Quaternion(-transform.rotation.x, -transform.rotation.y, -transform.rotation.z, transform.rotation.w));

                vertices[vertexIndex] = vertex - origin;

                if (i > 0)
                {
                    triangles[triangleIndex + 0] = 0;
                    triangles[triangleIndex + 1] = vertexIndex - 1;
                    triangles[triangleIndex + 2] = vertexIndex;

                    triangleIndex += 3;
                }

                vertexIndex++;
                angle -= angleIncrease;
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.bounds = new Bounds(Vector3.zero, Vector3.one * (radius + radius));

        }

        /// <summary>
        /// Destroy this light's data.
        /// </summary>
        public override void UnBake()
        {
            base.UnBake();
        }

        /// <summary>
        /// Set the aim direction to look at a directional vector.
        /// </summary>
        /// <param name="aimDirection"></param>
        public void SetAimDirection(Vector3 aimDirection)
        {
            this.aimDirection = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        }
        /// <summary>
        /// Set the aim direction to degrees.
        /// </summary>
        /// <param name="degrees"></param>
        public void SetAimDirection(float degrees)
        {
            aimDirection = degrees;
        }
    }
}