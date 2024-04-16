using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KieranCoppins.PostNavigation
{
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

        public bool IsPostOccupied(IPost post)
        {
            // This does have a time complexity of o(n agents) but usually you'd have a small amount of agents for the use case
            return occupiedPosts.ContainsValue(post);
        }

        public bool IsPostOccupiedBy(IPost post, IPostAgent agent)
        {
            return occupiedPosts.ContainsKey(agent) && occupiedPosts[agent] == post;
        }
    }
}
