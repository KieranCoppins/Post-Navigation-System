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
        public PostSelectorScores Run(PostSelectorScores scores);
    }
}
