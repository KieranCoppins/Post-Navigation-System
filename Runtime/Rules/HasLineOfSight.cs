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

        /// <summary>
        /// The amount to add to the post if has line of sight
        /// </summary>
        private readonly float weight;

        /// <summary>
        /// If the rule should remove posts that do not have line of sight
        /// </summary>
        private readonly bool destructive;



        /// <summary>
        /// Create a new HasLineOfSight rule
        /// </summary>
        /// <param name="target">The target to check line of sight to</param>
        /// <param name="heightOffset">The height offset from the post position (usually on the ground) to apply to the raycast</param>
        /// <param name="weight">The weight of this rule, used to scale the score</param>
        public HasLineOfSight(Transform target, float heightOffset, float weight = 1f, bool destructive = true)
        {
            this.target = target;
            this.heightOffset = heightOffset;
            this.weight = weight;
            this.destructive = destructive;
        }


        Dictionary<IPost, float> IPostRule.Run(Dictionary<IPost, float> scores)
        {
            Dictionary<IPost, float> newScores = destructive ? new() : new(scores);
            foreach (KeyValuePair<IPost, float> score in scores)
            {
                Vector3 direction = target.position - score.Key.Position;
                Ray ray = new Ray(score.Key.Position + Vector3.up * heightOffset, direction);
                if (Physics.Raycast(ray, out RaycastHit hit, direction.magnitude))
                {
                    if (hit.transform == target)
                    {
                        newScores[score.Key] = scores[score.Key] + weight;
                    }
                }
                else
                {
                    newScores[score.Key] = scores[score.Key] + weight;
                }
            }
            return newScores;
        }
    }
}
