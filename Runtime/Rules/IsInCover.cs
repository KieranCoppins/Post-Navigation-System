using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A destructive rule that will remove any posts that are not in cover to the target
    /// </summary>
    public class IsInCover : IPostRule
    {
        /// <summary>
        /// The transform of the target we are taking cover from
        /// </summary>
        private readonly Transform target;

        /// <summary>
        /// The distance away an object can be concidered cover
        /// </summary>
        private readonly float distance;

        /// <summary>
        /// The amount to add to the post if it is in cover
        /// </summary>
        private readonly float weight;

        /// <summary>
        /// If the rule should remove posts that are not in cover
        /// </summary>
        private readonly bool destructive;

        /// <summary>
        /// Create a new IsInCover rule
        /// </summary>
        /// <param name="target">The target to check cover against</param>
        /// <param name="dot">The dot product threshold to check against</param>
        public IsInCover(Transform target, float distance = 2f, float weight = 1f, bool destructive = true)
        {
            this.target = target;
            this.distance = distance;
            this.weight = weight;
            this.destructive = destructive;
        }

        Dictionary<IPost, float> IPostRule.Run(Dictionary<IPost, float> scores)
        {
            Dictionary<IPost, float> newScores = destructive ? new() : new(scores);
            foreach (KeyValuePair<IPost, float> score in scores)
            {
                Vector3 direction = (target.position - score.Key.ToVector3()).normalized;
                if (score.Key is ICoverPost && Physics.Raycast(score.Key.ToVector3() + (Vector3.up * 0.5f), direction, distance))
                {
                    Debug.DrawRay(score.Key.ToVector3() + (Vector3.up * 0.5f), direction * distance, Color.red, 5f);
                    newScores[score.Key] = scores[score.Key] + weight;
                }
            }
            return newScores;
        }
    }
}
