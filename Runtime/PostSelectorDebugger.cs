using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace KieranCoppins.PostNavigation
{
    [ExecuteInEditMode]
    public class PostSelectorDebugger : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private PostSelector postSelector;


        void OnEnable()
        {
            postSelector = new PostSelector(
                new List<PostRule>
                {
                    new IsInCover(target, 0.95f),
                    new HasLineOfSight(target, 1.2f, 1),
                    new DistanceToTarget(target, 1, true),
                    new DistanceToTarget(transform, .5f, true),
                },
                (origin) => PostGenerators.GetPostFromSceneData()
            );
        }

        void OnDrawGizmos()
        {
            if (!enabled) return;
            Dictionary<Post, float> scores = postSelector.Run(transform.position);

            float minScore = scores.Values.Min();
            float maxScore = scores.Values.Max();
            Gizmos.color = Color.white;

            foreach (KeyValuePair<Post, float> score in scores)
            {
                // Debug.Log(score.Key + " " + score.Value);
                float normalisedScore = Mathf.InverseLerp(minScore, maxScore, score.Value);
                Gizmos.color = Color.Lerp(Color.red, Color.green, normalisedScore);
                Gizmos.DrawSphere(score.Key, 0.1f);
            }

            Vector3 bestPoint = scores.GetBestPost();
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(bestPoint, 0.3f);
        }
    }
}
