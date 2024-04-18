using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A singleton class that manages all the posts in the scene, it is responsable for keeping a reference
    /// to all posts and which posts are occupied by IPostAgents 
    /// </summary>
    public class PostManager
    {
        public static PostManager Instance
        {
            get
            {
                instance ??= new PostManager();
                return instance;
            }
        }
        private static PostManager instance = null;

        public IPost[] Posts { get; private set; }

        private Dictionary<IPostAgent, IPost> occupiedPosts = new();

        private PostManager()
        {
            // Get posts from the scene data
            Posts = PostGenerators.GetPostFromSceneData();
        }

        /// <summary>
        /// Occupies the given post with the given agent
        /// </summary>
        /// <param name="post">The post to occupy</param>
        /// <param name="agent">The agent to occupy with</param>
        public void OccupyPost(IPost post, IPostAgent agent)
        {
            // only subscribe if we never occupied a post before
            if (!occupiedPosts.ContainsKey(agent) && agent is MonoBehaviour monoBehaviour)
            {
                // When we occupy a post, check if it is a monobehavour, if it is then subscribe to the destroy event
                monoBehaviour.destroyCancellationToken.Register(() =>
                {
                    occupiedPosts.Remove(agent);
                });
            }

            occupiedPosts[agent] = post;
        }

        /// <summary>
        /// Checks if a post is occupied by an agent
        /// </summary>
        /// <param name="post">The post to check</param>
        /// <returns>True if the post is occupied</returns>
        public bool IsPostOccupied(IPost post)
        {
            // This does have a time complexity of o(n agents) but usually you'd have a small amount of agents for the use case
            return occupiedPosts.ContainsValue(post);
        }

        /// <summary>
        /// Checks if the given post is occupied by the given agent
        /// </summary>
        /// <param name="post">The post to check</param>
        /// <param name="agent">The agent to check</param>
        /// <returns>True if the post is occupied by the provided agent</returns>
        public bool IsPostOccupiedBy(IPost post, IPostAgent agent)
        {
            return occupiedPosts.ContainsKey(agent) && occupiedPosts[agent] == post;
        }
    }
}
