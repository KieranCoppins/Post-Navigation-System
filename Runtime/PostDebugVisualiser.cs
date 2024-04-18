using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A debug tool to allow visualising posts in the scene view, only shows serialized posts in post data. Does not show
    /// monobehaviour posts placed in the scene
    /// </summary>
    [ExecuteInEditMode]
    public class PostDebugVisualiser : MonoBehaviour
    {
        private IPost[] generatedPosts;
        void OnEnable()
        {
            generatedPosts = PostGenerators.GetPostFromSceneData();
        }

        void OnDrawGizmos()
        {
            if (!enabled) return;

            foreach (IPost post in generatedPosts)
            {
                if (post is IOpenPost openPost)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(openPost.ToVector3(), 0.1f);
                }
                else if (post is ICoverPost coverPost)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(post.ToVector3(), new Vector3(.2f, 2f, .2f));
                    Gizmos.color = Color.magenta;
                    Vector3 coverRayOrigin = post.ToVector3() + Vector3.up;
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
                    Gizmos.DrawSphere(post.ToVector3(), 0.1f);
                }
            }
        }
    }
}
