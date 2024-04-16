using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    public class Zone : MonoBehaviour
    {
        public List<Vector3> ZonePoints { get => zonePoints; set => zonePoints = value; }
        [SerializeField] private float height = 3f;

        public List<IPost> Posts { get; private set; } = new();

        [Header("Zone Properties")]
        [Tooltip("The zone manager will ensure that every zone's minimum agents is always met.")]
        public int MinAgents { get => minAgents; }
        [SerializeField] private int minAgents = 0;

        [Tooltip("The zone manager will ensure that every zone's maximum agents is never exceeded.")]
        public int MaxAgents { get => maxAgents; }
        [SerializeField] private int maxAgents = 0;

        public int CurrentAgents { get => agentsInZone.Count; }

        List<IPostAgent> agentsInZone = new List<IPostAgent>();

        [SerializeField]
        private List<Vector3> zonePoints = new List<Vector3>()
    {
        // Define a square zone as default
        new Vector3(-1, 0, -1),
        new Vector3(1, 0, -1),
        new Vector3(1, 0, 1),
        new Vector3(-1, 0, 1)
    };

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

        public IPostAgent GetAgentInZone()
        {
            if (agentsInZone.Count == 0) return null;

            return agentsInZone[0];
        }

        public void AssignAgent(IPostAgent agent)
        {
            if (CurrentAgents >= MaxAgents)
            {
                Debug.LogWarning("Zone is at maximum capacity, cannot assign agent.");
                return;
            }

            if (agent.AssignedZone != null)
            {
                agent.AssignedZone.RemoveAgent(agent);
            }

            agentsInZone.Add(agent);
            agent.AssignedZone = this;
            agent.OnAssignedZone(this);

            // Check if the agent is a monobehaviour
            if (agent is MonoBehaviour monoBehaviour)
            {
                // If it is then subscribe to the destroy event, so that we dont have to in the Zoneable agent impl
                monoBehaviour.destroyCancellationToken.Register(() => RemoveAgent(agent));
            }
        }

        public void RemoveAgent(IPostAgent agent)
        {
            agentsInZone.Remove(agent);
            agent.AssignedZone = null;
        }

        public IPost[] PostsInZone()
        {
            return Posts.ToArray();
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
