using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A destructive rule that removes any posts that are not of the given type
    /// </summary>
    public class IsOfPostType : PostRule
    {
        /// <summary>
        /// The type of post that we want to make sure we have
        /// </summary>
        private readonly Type postType;

        /// <summary>
        /// Create a new IsOfPostType rule
        /// </summary>
        /// <param name="postType">The type of post that we want to make sure we have</param>
        public IsOfPostType(Type postType) : base(0, true)
        {
            this.postType = postType;
        }

        public override Dictionary<Post, float> Run(Dictionary<Post, float> scores)
        {
            Dictionary<Post, float> baseScores = new Dictionary<Post, float>();
            foreach (KeyValuePair<Post, float> score in scores)
            {
                if (score.Key.GetType() == postType)
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
