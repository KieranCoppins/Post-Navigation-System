using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    using PostSelectorScores = Dictionary<Post, float>;

    /// <summary>
    /// A rule that scores posts based on their distance to a target
    /// </summary>
    public class DistanceToTarget : PostRule
    {
        private readonly Transform target;

        public DistanceToTarget(Transform target, float weight, bool normaliseScore) : base(weight, normaliseScore)
        {
            this.target = target;
        }

        public override PostSelectorScores Run(PostSelectorScores scores)
        {
            // Determine our range of distances for normalisation
            PostSelectorScores baseScores = new();
            float furthestDistance = 0;
            float closestDistance = Mathf.Infinity;
            foreach (KeyValuePair<Post, float> score in scores)
            {
                float dist = Vector3.Distance(target.position, score.Key);
                if (dist > furthestDistance)
                {
                    furthestDistance = dist;
                }
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                }

                baseScores[score.Key] = dist;
            }

            // Apply normnalisation and weighting
            PostSelectorScores newScores = new(baseScores);
            foreach (KeyValuePair<Post, float> score in baseScores)
            {
                newScores[score.Key] = (
                    normaliseScore ?
                        -Mathf.InverseLerp(closestDistance, furthestDistance, score.Value)
                        : -score.Value
                    ) * weight;
            }
            return newScores;
        }
    }
}
