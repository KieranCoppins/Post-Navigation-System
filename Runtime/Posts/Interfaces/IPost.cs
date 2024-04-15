using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPost
    {
        /// <summary>
        /// The position of the post
        /// </summary>
        public Vector3 Position { get; protected set; }

        /// <summary>
        /// The object that is currently occupying the post
        /// </summary>
        public object OccupiedBy { get; set; }

        public Vector3 ToVector3() => Position;
    }
}
