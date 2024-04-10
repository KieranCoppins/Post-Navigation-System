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

    /// <summary>
    /// A post that represents a cover object, contains extra data for a cover
    /// </summary>
    [System.Serializable]
    public class CoverPost : Post
    {
        /// <summary>
        /// The type of cover this post is
        /// </summary>
        public CoverType CoverType { get => coverType; }
        [SerializeField] private CoverType coverType;

        /// <summary>
        /// The direction the cover is facing, i.e. the direction the target is in relative to the cover
        /// </summary>
        public Vector3 CoverDirection { get => coverDirection; }
        [SerializeField] private Vector3 coverDirection;

        /// <summary>
        /// Whether the agent can peak left from this cover
        /// </summary>
        public bool CanPeakLeft { get => canPeakLeft; }
        [SerializeField] private bool canPeakLeft;

        /// <summary>
        /// Whether the agent can peak right from this cover
        /// </summary>
        public bool CanPeakRight { get => canPeakRight; }
        [SerializeField] private bool canPeakRight;

        public CoverPost(Vector3 position, CoverType coverType, Vector3 coverDirection, bool canPeakLeft, bool canPeakRight) : base(position)
        {
            this.coverType = coverType;
            this.coverDirection = coverDirection;
            this.canPeakLeft = canPeakLeft;
            this.canPeakRight = canPeakRight;
        }
    }
}
