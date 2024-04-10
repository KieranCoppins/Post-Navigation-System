using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A monodevelop class to generate posts from a navmesh, this is so we can call this function inside the unity editor and save it to a file. This is so we dont have to generate at runtime
    /// </summary>
    public class NavMeshPostGenerationMonodevelop : MonoBehaviour
    {
        [MenuItem("Tools/Navigation/Generate Posts For NavMesh")]
        private static void GeneratePostsForNavMesh()
        {
            Post[] posts = PostGenerators.GenerateFromNavMesh(new NavMeshPostGenerationConfig());
            PostData postData = PostData.CreateInstance(posts);
            AssetDatabase.CreateAsset(postData, $"{SceneManager.GetActiveScene().path.Replace(".unity", "")}/Posts.asset");
        }
    }
}

