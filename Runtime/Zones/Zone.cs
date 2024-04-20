using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A component to define a zone within the scene. This is used to assign agents to zones
    /// </summary>
    public class Zone : MonoBehaviour
    {
        /// <summary>
        /// The points that define the zone. The zone is defined by a closed loop of points
        /// </summary>
        public List<Vector3> ZonePoints { get => zonePoints; set => zonePoints = value; }

        /// <summary>
        /// The height of the zone, any posts that are above or below the zones height are not included in the zone
        /// </summary>
        [SerializeField, Tooltip("The height of the zone, any points that are above or below the zones height are not included in the zone")] private float height = 3f;

        /// <summary>
        /// The posts that are in this zone
        /// </summary>
        public List<IPost> Posts { get; private set; } = new();

        /// <summary>
        /// The minimum number of agents that should be in this zone
        /// </summary>
        public int MinAgents { get => minAgents; }
        [SerializeField, Tooltip("The minimum number of agents that should be in this zone")] private int minAgents = 0;

        /// <summary>
        /// The maximum number of agents that should be in this zone
        /// </summary>
        public int MaxAgents { get => maxAgents; }
        [SerializeField, Tooltip("The maximum number of agents that should be in this zone")] private int maxAgents = 0;

        [SerializeField, Tooltip("The points that define the zone. The zone is defined by a closed loop of points")]
        private List<Vector3> zonePoints = new List<Vector3>()
        {
            // Define a square zone as default
            new Vector3(-1, 0, -1),
            new Vector3(1, 0, -1),
            new Vector3(1, 0, 1),
            new Vector3(-1, 0, 1)
        };

        /// <summary>
        /// Checks if a point is inside the zone
        /// </summary>
        /// <param name="point">The point to check if its in the zone</param>
        /// <returns>True if the point is in the zone</returns>
        public bool IsPointInZone(Vector3 point)
        {
            // Height check is fast so do this first before moving onto the ray-casting algorithm
            float currentY = transform.position.y;
            if (point.y < currentY || point.y > currentY + height) return false;

            // Transform the points to global space and create a closed loop of points
            List<Vector3> loop = new(zonePoints);
            {
                loop.Add(zonePoints[0]);
            }
            Vector3[] transformedPoints = loop.Select(p => transform.TransformPoint(p)).ToArray();


            // Check if the point is inside the zone using the ray-casting algorithm
            int intersectCount = 0;
            for (int i = 0; i < transformedPoints.Length - 1; i++)
            {
                Vector3 p1 = transformedPoints[i];
                Vector3 p2 = transformedPoints[i + 1];

                // Check if the ray intersects with this edge
                if ((p1.z > point.z) != (p2.z > point.z) &&
                    (point.x < (p2.x - p1.x) * (point.z - p1.z) / (p2.z - p1.z) + p1.x))
                {
                    intersectCount++;
                }
            }

            // If the number of intersections is odd, the point is inside the polygon
            return intersectCount % 2 == 1;
        }

        void OnDrawGizmos()
        {
            // Create a mesh for the zone
            Mesh zoneMesh = new Mesh();

            // Define vertices for the base of the polygon
            Vector3[] vertices = new Vector3[zonePoints.Count * 2];
            for (int i = 0; i < zonePoints.Count; i++)
            {
                vertices[i] = zonePoints[i];
                vertices[i + zonePoints.Count] = zonePoints[i] + Vector3.up * height;
            }

            // Define triangles
            int[] triangles = new int[zonePoints.Count * 6];
            for (int i = 0; i < zonePoints.Count - 1; i++)
            {
                int j = i * 6;
                triangles[j] = i;
                triangles[j + 1] = i + zonePoints.Count;
                triangles[j + 2] = i + 1;

                triangles[j + 3] = i + 1;
                triangles[j + 4] = i + zonePoints.Count;
                triangles[j + 5] = i + zonePoints.Count + 1;
            }

            // Handle last triangle
            triangles[triangles.Length - 3] = zonePoints.Count - 1;
            triangles[triangles.Length - 2] = 2 * zonePoints.Count - 1;
            triangles[triangles.Length - 1] = 0;

            // Assign vertices and triangles to the mesh
            zoneMesh.vertices = vertices;
            zoneMesh.triangles = triangles;

            // Recalculate normals to ensure proper lighting
            zoneMesh.RecalculateNormals();


            Gizmos.color = Color.magenta;
            Gizmos.DrawWireMesh(zoneMesh, transform.position, Quaternion.identity);

            // Draw a sphere at the last point so we know which one is the end
            if (ZonePoints.Count > 0)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.TransformPoint(ZonePoints[ZonePoints.Count - 1]), .2f);
            }
        }
    }
}
