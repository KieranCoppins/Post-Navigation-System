using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    [System.Serializable]
    public class PostData : ScriptableObject
    {
        /// <summary>
        /// The posts in the data
        /// </summary>
        public Post[] Posts { get => posts.ToArray(); }

        [SerializeReference] private List<Post> posts;

        private PostData() { }

        public static PostData CreateInstance(Post[] posts)
        {
            PostData postData = ScriptableObject.CreateInstance<PostData>();
            postData.posts = new(posts);
            return postData;
        }
    }
}
