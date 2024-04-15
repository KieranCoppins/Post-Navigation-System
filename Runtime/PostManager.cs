using System;
using System.Collections;
using System.Collections.Generic;
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

        private PostManager()
        {
            // Get posts from the scene data
            Posts = PostGenerators.GetPostFromSceneData();
        }


    }
}
