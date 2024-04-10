using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    using PostSelectorScores = Dictionary<Post, float>;

    /// <summary>
    /// A rule that can be applied to a set of posts to score them
    /// </summary>
    public abstract class PostRule
    {
        /// <summary>
        /// The weight of this rule, used to scale the score
        /// </summary>
        protected float weight = 1;

        /// <summary>
        /// Whether to normalise the score of this rule
        /// </summary>
        protected bool normaliseScore = true;

        // Dont allow a default construction
        private PostRule() { }

        protected PostRule(float weight, bool normaliseScore)
        {
            this.weight = weight;
            this.normaliseScore = normaliseScore;
        }

        public abstract PostSelectorScores Run(PostSelectorScores scores);
    }
}
