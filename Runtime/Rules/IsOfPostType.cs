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


            Dictionary<IPost, float> newScores = new Dictionary<IPost, float>();
            foreach (KeyValuePair<IPost, float> score in scores)
            {
                if (score.Key.GetType() == postType)
                {
                    newScores[score.Key] = scores[score.Key];
                }
            }
            return newScores;
        }
    }
}
