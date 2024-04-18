using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{

    public enum CoverType
    {
        /// <summary>
        /// Low cover describes a cover that agents should crouch behind
        /// </summary>
        Low,

        /// <summary>
        /// High cover describes a cover that agents should stand behind, note that agents would have to peak around high cover
        /// </summary>
        High
    }

    public interface ICoverPost : IPost
    {
        /// <summary>
        /// The type of cover this post is
        /// </summary>
        public CoverType CoverType { get; protected set; }

        /// <summary>
        /// The direction the cover is facing, i.e. the direction the target is in relative to the cover
        /// </summary>
        public Vector3 CoverDirection { get; protected set; }

        /// <summary>
        /// Whether the agent can peak left from this cover
        /// </summary>
        public bool CanPeakLeft { get; protected set; }

        /// <summary>
        /// Whether the agent can peak right from this cover
        /// </summary>
        public bool CanPeakRight { get; protected set; }

    }
}
