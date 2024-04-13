using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    using PostSelectorScores = Dictionary<IPost, float>;

    /// <summary>
    /// A rule that can be applied to a set of posts to score them
    /// </summary>
    public interface IPostRule
    {
        /// <summary>
        /// The weight of this rule, used to scale the score
        /// </summary>
        protected float Weight { get; set; }

        /// <summary>
        /// Whether to normalise the score of this rule
        /// </summary>
        protected bool NormaliseScore { get; set; }

        public PostSelectorScores Run(PostSelectorScores scores);
    }
}
