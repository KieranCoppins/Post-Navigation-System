using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A destructive rule that removes any posts that are not of the given type
    /// </summary>
    public class IsOfPostType : IPostRule
    {
        /// <summary>
        /// The type of post that we want to make sure we have
        /// </summary>
        private readonly Type postType;

        float IPostRule.Weight { get => weight; set => weight = value; }
        private float weight;
        bool IPostRule.NormaliseScore { get => normaliseScore; set => normaliseScore = value; }
        private bool normaliseScore;

        /// <summary>
        /// Create a new IsOfPostType rule
        /// </summary>
        /// <param name="postType">The type of post that we want to make sure we have</param>
        public IsOfPostType(Type postType)
        {
            this.postType = postType;
        }

        Dictionary<IPost, float> IPostRule.Run(Dictionary<IPost, float> scores)
        {
            Dictionary<IPost, float> baseScores = new Dictionary<IPost, float>();
            foreach (KeyValuePair<IPost, float> score in scores)
            {
                if (score.Key.GetType() == postType)
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
