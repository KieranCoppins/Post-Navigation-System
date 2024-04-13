using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    using PostSelectorScores = Dictionary<IPost, float>;

    /// <summary>
    /// A basic post selector implementation, allows for post selection based on a set of rules
    /// </summary>
    public class PostSelector
    {
        /// <summary>
        /// The rules for this post selector to run
        /// </summary>
        private readonly List<IPostRule> rules = new List<IPostRule>();

        /// <summary>
        /// A function to get an array of all the posts around an origin
        /// </summary>
        private readonly Func<Vector3, IPost[]> getPosts;

        /// <summary>
        /// Create a new Post Selector
        /// </summary>
        /// <param name="rules">The rules for this post selector to run</param>
        /// <param name="getPosts">A function to get an array of all the posts around an origin</param>
        public PostSelector(List<IPostRule> rules, Func<Vector3, IPost[]> getPosts)
        {
            this.rules = rules;
            this.getPosts = getPosts;
        }

        /// <summary>
        /// Run the Post Selector
        /// </summary>
        /// <param name="origin">The position to run the post selector from</param>
        /// <returns>A list of key value pairs for each post and their score</returns>
        public PostSelectorScores Run(Vector3 origin)
        {
            IPost[] points = getPosts(origin);

            // Stores the score for each point in the EQS
            PostSelectorScores scores = new PostSelectorScores();

            // Initialise the dictionary
            foreach (IPost point in points)
            {
                scores.TryAdd(point, 0);
            }

            // Run each rule, every rule should take the dictionary of scores and modify it
            foreach (IPostRule rule in rules)
            {
                scores = rule.Run(scores);
            }

            return scores;
        }
    }

    /// <summary>
    /// An extension class on PostSelectorScores to get the best post from the scores
    /// </summary>
    public static class PostSelectorScoresExtensions
    {

        /// <summary>
        /// Gets the best post from the scores
        /// </summary>
        /// <param name="scores">The list of key value pairs to select from</param>
        /// <returns>The best post</returns>
        public static IPost GetBestPost(this PostSelectorScores scores)
        {
            return scores.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }
    }
}
