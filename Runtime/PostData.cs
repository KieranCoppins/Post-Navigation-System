using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    [System.Serializable]
    internal class PostData : ScriptableObject
    {
        /// <summary>
        /// The posts in the data
        /// </summary>
        public IPost[] Posts { get => posts.ToArray(); }

        [SerializeReference] private List<IPost> posts;

        private PostData() { }

        public static PostData CreateInstance(IPost[] posts)
        {
            PostData postData = ScriptableObject.CreateInstance<PostData>();
            postData.posts = new(posts);
            return postData;
        }
    }
}
