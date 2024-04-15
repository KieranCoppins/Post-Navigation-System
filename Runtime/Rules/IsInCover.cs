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
        public IsInCover(Transform target, float dot)
        {
            this.target = target;
            this.dot = dot;
        }

        Dictionary<IPost, float> IPostRule.Run(Dictionary<IPost, float> scores)
        {
            Dictionary<IPost, float> newScores = new Dictionary<IPost, float>();
            foreach (KeyValuePair<IPost, float> score in scores)
            {
                Vector3 direction = target.position - score.Key.ToVector3();
                if (score.Key is ICoverPost coverPost && Vector3.Dot(coverPost.CoverDirection.normalized, direction.normalized) > dot)
                {
                    newScores[score.Key] = scores[score.Key];
                }
            }
            return newScores;
        }
    }
}
