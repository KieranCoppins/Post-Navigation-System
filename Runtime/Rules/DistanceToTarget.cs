using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    using PostSelectorScores = Dictionary<IPost, float>;

    /// <summary>
    /// A rule that scores posts based on their distance to a target
    /// </summary>
    public class DistanceToTarget : IPostRule
    {
        float IPostRule.Weight { get => weight; set => weight = value; }
        private float weight;

        bool IPostRule.NormaliseScore { get => normaliseScore; set => normaliseScore = value; }
        private bool normaliseScore;
        private readonly Transform target;

        public DistanceToTarget(Transform target, float weight, bool normaliseScore)
        {
            this.target = target;
            this.weight = weight;
            this.normaliseScore = normaliseScore;
        }


        PostSelectorScores IPostRule.Run(PostSelectorScores scores)
        {
            // Determine our range of distances for normalisation
            PostSelectorScores baseScores = new();
            float furthestDistance = 0;
            float closestDistance = Mathf.Infinity;
            foreach (KeyValuePair<IPost, float> score in scores)
            {
                float dist = Vector3.Distance(target.position, score.Key.ToVector3());
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
            foreach (KeyValuePair<IPost, float> score in baseScores)
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
