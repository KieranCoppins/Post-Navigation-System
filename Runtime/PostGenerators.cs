using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// Configuration class for post generation using Unity's navmesh
    /// </summary>
    public class NavMeshPostGenerationConfig
    {
        /// <summary>
        /// The height of the agent that will be using the generated posts
        /// </summary>
        public float AgentHeight { get; set; }

        /// <summary>
        /// The distance from the point to check for cover
        /// </summary>
        public float CoverDistance { get; set; }

        /// <summary>
        /// The distance from the point to the left or right to check if the agent can peak around the corner
        /// </summary>
        public float CoverPeakDistance { get; set; }

        /// <summary>
        /// The distance between each cover post when walking around the edge of a navmesh triangle
        /// </summary>
        public float CoverPostStepSize { get; set; }

        public NavMeshPostGenerationConfig()
        {
            this.AgentHeight = 2f;
            this.CoverDistance = 0.7f;
            this.CoverPeakDistance = 0.8f;
            this.CoverPostStepSize = 0.6f;
        }

        /// <summary>
        /// Create a new NavMeshPostGenerationConfig with the given parameters
        /// </summary>
        /// <param name="agentHeight">The height of the agent that will be using the generated posts</param>
        /// <param name="coverDistance">The distance from the point to check for cover</param>
        /// <param name="coverPeakDistance">The distance from the point to the left or right to check if the agent can peak around the corner</param>
        /// <param name="coverPostStepSize">The distance between each cover post when walking around the edge of a navmesh triangle</param>
        public NavMeshPostGenerationConfig(float agentHeight = 2f, float coverDistance = 0.7f, float coverPeakDistance = 0.7f, float coverPostStepSize = 0.6f)
        {
            this.AgentHeight = agentHeight;
            this.CoverDistance = coverDistance;
            this.CoverPeakDistance = coverPeakDistance;
            this.CoverPostStepSize = coverPostStepSize;
        }
    }

    /// <summary>
    /// A static class with different post generation algorithms
    /// </summary>
    public static class PostGenerators
    {
        /// <summary>
        /// Generates a grid of posts around the origin on the ground. Multiple floors are supported
        /// </summary>
        /// <param name="origin">The origin for post generation</param>
        /// <param name="widthCount">The amount of posts to generate in the X</param>
        /// <param name="heightCount">The amount of posts to generate in the Z</param>
        /// <param name="spacing">The spacing between each post</param>
        /// <returns></returns>
        public static Post[] GenerateGrid(Vector3 origin, float widthCount, float heightCount, float spacing)
        {
            // Create a grid of points around the position
            List<Post> points = new();

            for (float x = (-widthCount / 2f) + .5f; x < (widthCount / 2f) + .5f; x++)
            {
                for (float y = (-heightCount / 2f) + .5f; y < (heightCount / 2f) + .5f; y++)
                {
                    Vector3 point = new Vector3(x * spacing, 0, y * spacing) + origin;
                    points.AddRange(FindFloors(point).Select(floor => new Post(floor)));
                }
            }

            points.Add(new Post(origin));

            return points.ToArray();
        }


        /// <summary>
        /// Generates a grid of posts around the origin on the ground in a radius. Multiple floors are supported
        /// </summary>
        /// <param name="origin">The origin for post generation</param>
        /// <param name="radius">The radius from the origin for post generation</param>
        /// <param name="postSpacing">The spacing between each post within the radius</param>
        /// <returns></returns>
        public static Post[] GenerateCircle(Vector3 origin, float radius, int postSpacing)
        {
            List<Post> posts = new();

            // Generate a filled grid of points within the radius of the origin
            for (float x = -radius; x < radius; x++)
            {
                for (float z = -radius; z < radius; z++)
                {
                    Vector3 point = new Vector3(x * postSpacing, 0, z * postSpacing) + origin;
                    if (Vector3.Distance(origin, point) < radius)
                    {
                        posts.AddRange(FindFloors(point).Select(floor => new Post(floor)));
                    }
                }
            }

            return posts.ToArray();
        }

        /// <summary>
        /// Generates posts on the navmesh using the given configuration, this should not be done at runtime!
        /// </summary>
        /// <param name="config">The configuration for this post generation</param>
        /// <returns></returns>
        internal static Post[] GenerateFromNavMesh(NavMeshPostGenerationConfig config)
        {
            NavMeshTriangulation navmeshData = NavMesh.CalculateTriangulation();
            Mesh mesh = new Mesh();
            mesh.SetVertices(navmeshData.vertices.ToList());
            mesh.SetIndices(navmeshData.indices, MeshTopology.Triangles, 0);

            List<Post> posts = new();

            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                // Triangle points A, B, C
                Vector3 A = mesh.vertices[mesh.triangles[i]];
                Vector3 B = mesh.vertices[mesh.triangles[i + 1]];
                Vector3 C = mesh.vertices[mesh.triangles[i + 2]];

                // Center of the triangle
                Vector3 center = (A + B + C) / 3f;

                // Create open posts at the center of each triangle in the navmesh
                posts.Add(new OpenPost(center));

                // Walk around the edge of the triangle and use raycasts to determine if there is a wall
                Vector3[] edgePoints = { A, B, C, A };
                for (int j = 0; j < edgePoints.Length - 1; j++)
                {
                    Vector3 interpPointA = edgePoints[j];
                    Vector3 interpPointB = edgePoints[j + 1];
                    Vector3 edgeVector = (interpPointB - interpPointA).normalized;

                    Vector3 interpPoint = interpPointA;

                    // Counter for saftey
                    int counter = 0;

                    // Interpolate between the two points at a given step size
                    while (Vector3.Dot(edgeVector, (interpPointB - interpPoint).normalized) > 0 && counter < 100)
                    {
                        Vector3 rayDirection = Vector3.Cross(edgeVector, Vector3.up);
                        CalculateCoverRaycast(in posts, interpPoint, rayDirection, config.CoverDistance, config.CoverPeakDistance, config.AgentHeight);
                        CalculateCoverRaycast(in posts, interpPoint, -rayDirection, config.CoverDistance, config.CoverPeakDistance, config.AgentHeight);
                        interpPoint += edgeVector * config.CoverPostStepSize;
                        counter++;
                    }
                }

            }

            return posts.ToArray();
        }


        /// <summary>
        /// Gets the post data from the asset saved in the active scenes folder directory
        /// </summary>
        /// <returns>The posts that have been generated previously</returns>
        public static Post[] GetPostFromSceneData()
        {
            PostData data = AssetDatabase.LoadAssetAtPath<PostData>($"{SceneManager.GetActiveScene().path.Replace(".unity", "")}/Posts.asset");
            if (data == null)
            {
                Debug.LogWarning("No post data found in scene");
            }
            return data.Posts;
        }

        /// <summary>
        /// Uses a raycast to find all hit points through multiple floors and returns a list of them
        /// </summary>
        /// <param name="point">The point to search floors for</param>
        /// <returns>A list of points</returns>
        private static Vector3[] FindFloors(Vector3 point)
        {
            var hits = Physics.RaycastAll(point + Vector3.up * 1000f, Vector3.down);
            return hits.Select(hit => hit.point).ToArray();
        }

        /// <summary>
        /// Calculate if the given point is within the triangle defined by A, B, C. This method does not care about height of the point
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <param name="A">Point A of the triangle</param>
        /// <param name="B">Point B of the triangle</param>
        /// <param name="C">Point C of the triangle</param>
        /// <returns>True if the point is within the triangle</returns>
        private static bool PointInTriangle(Vector3 point, Vector3 A, Vector3 B, Vector3 C)
        {
            float s = A.z * C.x - A.x * C.z + (C.z - A.z) * point.x + (A.x - C.x) * point.z;
            float t = A.x * B.z - A.z * B.x + (A.z - B.z) * point.x + (B.x - A.x) * point.z;

            if ((s < 0) != (t < 0))
                return false;

            float A1 = -B.z * C.x + A.z * (C.x - B.x) + A.x * (B.z - C.z) + B.x * C.z;
            if (A1 < 0.0)
            {
                s = -s;
                t = -t;
                A1 = -A1;
            }

            return s > 0 && t > 0 && (s + t) <= A1;
        }


        /// <summary>
        /// Calculate the height of a point within a triangle using barycentric coordinates
        /// </summary>
        /// <param name="point">The point on the triangle excluding height data (y)</param>
        /// <param name="A">Point A of the triangle</param>
        /// <param name="B">Point B of the triangle</param>
        /// <param name="C">Point C of the triangle</param>
        /// <returns>The height using barycentric coordinates</returns>
        private static float CalculateTriangleInterpolatedHeight(Vector3 point, Vector3 A, Vector3 B, Vector3 C)
        {
            // Find the height of the point within the triangle ABC using barycentric coordinates
            float areaABC = Vector3.Cross(B - A, C - A).magnitude;
            float areaPBC = Vector3.Cross(B - point, C - point).magnitude / areaABC;
            float areaPCA = Vector3.Cross(C - point, A - point).magnitude / areaABC;
            float areaPAB = Vector3.Cross(A - point, B - point).magnitude / areaABC;

            return areaPBC * A.y + areaPCA * B.y + areaPAB * C.y;
        }

        private static void CalculateCoverRaycast(in List<Post> currentPosts, Vector3 point, Vector3 rayDirection, float length, float peakWidth, float agentHeight)
        {
            Vector3 highCoverOffset = agentHeight * 0.75f * Vector3.up;
            Vector3 lowCoverOffset = agentHeight * 0.25f * Vector3.up;
            if (Physics.Raycast(point + lowCoverOffset, rayDirection, length))
            {
                Vector3 coverDirection = Vector3.Cross(rayDirection, Vector3.up).normalized;
                if (Physics.Raycast(point + highCoverOffset, rayDirection, length))
                {
                    bool canPeakLeft = !Physics.Raycast(point + highCoverOffset + (coverDirection * peakWidth), rayDirection, 5);
                    bool canPeakRight = !Physics.Raycast(point + highCoverOffset + (-coverDirection * peakWidth), rayDirection, 5);
                    if (canPeakLeft || canPeakRight)
                    {
                        currentPosts.Add(new CoverPost(point, CoverType.High, rayDirection, canPeakLeft, canPeakRight));
                    }
                }
                else
                {
                    bool canPeakLeft = !Physics.Raycast(point + lowCoverOffset + (coverDirection * peakWidth), rayDirection, 5);
                    bool canPeakRight = !Physics.Raycast(point + lowCoverOffset + (-coverDirection * peakWidth), rayDirection, 5);
                    bool canPeakOver = !Physics.Raycast(point + highCoverOffset, rayDirection, 5);
                    if (canPeakOver)
                    {
                        currentPosts.Add(new CoverPost(point, CoverType.Low, rayDirection, canPeakLeft, canPeakRight));
                    }
                }
            }
        }
    }
}
