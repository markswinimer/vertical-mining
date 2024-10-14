using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace L2D
{
    /// <summary>
    /// A cone light with a spot on the end, occilates back and forth in cycles.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class SpotLight : LightBase
    {
        /// <summary>
        /// If true, the spot light's direction will point towards the mouse.
        /// </summary>
        public bool followMouse = false;
        /// <summary>
        /// Field of view in degrees for the spotlight.
        /// </summary>
        [Range(0.001f, 360f)] public float fov = 30;
        /// <summary>
        /// Controls the roundness of the spot at the end of the light.
        /// </summary>
        [Range(0f, 1f)] public float spotSteepness = 0.2f;
        /// <summary>
        /// Direction in degrees the spot is facing, not including where the spotlight is in it's movement cycle.
        /// </summary>
        public float aimDirection = 0;
        float startingAngle;
        private Vector3 origin;

        /// <summary>
        /// Total degrees of the spotlight's rotation cycle.
        /// </summary>
        [Range(0, 360)] public float rotationRange = 10;
        /// <summary>
        /// How fast the spot light can complete one cycle.
        /// </summary>
        public float rotationSpeed = 0.5f;
        private float rotationOffset = 0;

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
            rotationOffset = Mathf.SmoothStep(0, rotationRange * Mathf.Deg2Rad, Mathf.PingPong(Time.time * rotationSpeed, 1)) - rotationRange * Mathf.Deg2Rad / 2;

            float steepness = spotSteepness * radius;

            origin = transform.position;

            if (followMouse && Application.isPlaying)
            {
                Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouse -= transform.position;
                SetAimDirection(mouse);
            }

            float angle = Mathf.Deg2Rad * aimDirection + (Mathf.Deg2Rad * fov) / 2;
            angle += transform.eulerAngles.z * Mathf.Deg2Rad;
            angle += rotationOffset;
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

            float rayHalf = rayCount / 2;
            for (int i = 0; i <= rayCount; i++)
            {
                Vector3 vertex;
                float rayDistance = radius + -steepness * Mathf.Pow((i - rayHalf) / rayHalf, 2);
                RaycastHit2D raycastHit = Physics2D.Raycast(origin, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0), rayDistance, layerMask);
                if (raycastHit.collider == null)
                {
                    vertex = origin + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * rayDistance;
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