using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A destructive rule that will remove any posts that are not in cover to the target
    /// </summary>
    public class IsInCover : PostRule
    {
        /// <summary>
        /// The target to check cover against
        /// </summary>
        private readonly Transform target;

        /// <summary>
        /// The dot product threshold to check against
        /// </summary>
        private readonly float dot;

        /// <summary>
        /// Create a new IsInCover rule
        /// </summary>
        /// <param name="target">The target to check cover against</param>
        /// <param name="dot">The dot product threshold to check against</param>
        public IsInCover(Transform target, float dot) : base(0, true)
        {
            this.target = target;
            this.dot = dot;
        }

        public override Dictionary<Post, float> Run(Dictionary<Post, float> scores)
        {
            Dictionary<Post, float> baseScores = new Dictionary<Post, float>();
            foreach (KeyValuePair<Post, float> score in scores)
            {
                Vector3 direction = target.position - score.Key;
                if (score.Key is CoverPost coverPost && Vector3.Dot(coverPost.CoverDirection.normalized, direction.normalized) > dot)
                {
                    baseScores[score.Key] = weight;
                }
            }

            Dictionary<Post, float> newScores = new Dictionary<Post, float>();
            foreach (KeyValuePair<Post, float> score in baseScores)
            {
                newScores[score.Key] = score.Value + scores[score.Key];
            }
            return newScores;
        }
    }
}
