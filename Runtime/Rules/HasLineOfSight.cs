using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A destructive rule that will remove any posts that do not have line of sight to the target
    /// </summary>
    public class HasLineOfSight : IPostRule
    {
        /// <summary>
        /// The target to check line of sight to
        /// </summary>
        private readonly Transform target;

        /// <summary>
        /// The height offset from the post position (usually on the ground) to apply to the raycast
        /// </summary>
        private readonly float heightOffset;

        float IPostRule.Weight { get => weight; set => weight = value; }
        private float weight;
        bool IPostRule.NormaliseScore { get => normaliseScore; set => normaliseScore = value; }
        private bool normaliseScore;

        /// <summary>
        /// Create a new HasLineOfSight rule
        /// </summary>
        /// <param name="target">The target to check line of sight to</param>
        /// <param name="heightOffset">The height offset from the post position (usually on the ground) to apply to the raycast</param>
        /// <param name="weight">The weight of this rule, used to scale the score</param>
        public HasLineOfSight(Transform target, float heightOffset, float weight)
        {
            this.target = target;
            this.heightOffset = heightOffset;
            this.weight = weight;
            this.normaliseScore = false;
        }


        Dictionary<IPost, float> IPostRule.Run(Dictionary<IPost, float> scores)
        {
            Dictionary<IPost, float> baseScores = new Dictionary<IPost, float>();
            foreach (KeyValuePair<IPost, float> score in scores)
            {
                Vector3 direction = target.position - score.Key.ToVector3();
                Ray ray = new Ray(score.Key.ToVector3() + Vector3.up * heightOffset, direction);
                if (Physics.Raycast(ray, out RaycastHit hit, direction.magnitude))
                {
                    if (hit.transform == target)
                    {
                        baseScores[score.Key] = weight;
                    }
                }
                else
                {
                    baseScores[score.Key] = weight;
                }
            }

            Dictionary<IPost, float> newScores = new Dictionary<IPost, float>();
            foreach (KeyValuePair<IPost, float> score in baseScores)
            {
                newScores[score.Key] = score.Value + scores[score.Key];
            }
            return newScores;
        }
    }
}
