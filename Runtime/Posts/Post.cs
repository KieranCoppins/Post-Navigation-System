using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    [System.Serializable]
    public class Post
    {
        /// <summary>
        /// The position of the post
        /// </summary>
        public Vector3 Position { get => position; }
        [SerializeField] private Vector3 position;

        public Post(Vector3 position)
        {
            this.position = position;
        }

        public static implicit operator Vector3(Post post) => post.position;
    }
}
