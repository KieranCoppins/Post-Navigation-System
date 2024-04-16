using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A generic post implementation that only contains the position of the post
    /// 
    /// These generic posts are used when generating grids of posts at runtime since they could be anywhere and do not have context to the level
    /// </summary>
    [System.Serializable]
    internal class InternalPost : IPost
    {
        Vector3 IPost.Position { get => position; set => position = value; }

        [SerializeField] private Vector3 position;

        public InternalPost(Vector3 position)
        {
            this.position = position;
        }
    }
}
