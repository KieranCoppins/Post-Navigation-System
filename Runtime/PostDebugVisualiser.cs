using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A debug tool to allow visualising posts in the scene view
    /// </summary>
    [ExecuteInEditMode]
    public class PostDebugVisualiser : MonoBehaviour
    {
        private Post[] generatedPosts;
        void OnEnable()
        {
            generatedPosts = PostGenerators.GetPostFromSceneData();
        }

        void OnDrawGizmos()
        {
            if (!enabled) return;

            foreach (Post post in generatedPosts)
            {
                if (post.GetType() == typeof(OpenPost))
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(post, 0.1f);
                }
                else if (post.GetType() == typeof(CoverPost))
                {
                    CoverPost coverPost = (CoverPost)post;
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(post, new Vector3(.2f, 2f, .2f));
                    Gizmos.color = Color.magenta;
                    Vector3 coverRayOrigin = coverPost + Vector3.up;
                    Vector3 coverDirection = Vector3.Cross(coverPost.CoverDirection, Vector3.up);
                    Gizmos.DrawRay(coverRayOrigin, coverPost.CoverDirection);
                    if (coverPost.CanPeakLeft)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawRay(coverRayOrigin + (coverDirection * 0.3f), coverPost.CoverDirection);
                    }

                    if (coverPost.CanPeakRight)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawRay(coverRayOrigin + (-coverDirection * 0.3f), coverPost.CoverDirection);
                    }
                }
                else
                {
                    Gizmos.color = Color.white;
                }
            }
        }
    }
}
