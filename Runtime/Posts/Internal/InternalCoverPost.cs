using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A post that represents a cover object, contains extra data for a cover
    /// </summary>
    [System.Serializable]
    internal class InternalCoverPost : ICoverPost
    {
        Vector3 IPost.Position { get => position; set => position = value; }
        CoverType ICoverPost.CoverType { get => coverType; set => coverType = value; }
        Vector3 ICoverPost.CoverDirection { get => coverDirection; set => coverDirection = value; }
        bool ICoverPost.CanPeakLeft { get => canPeakLeft; set => canPeakLeft = value; }
        bool ICoverPost.CanPeakRight { get => canPeakRight; set => canPeakRight = value; }

        [SerializeField] private Vector3 position;
        [SerializeField] private CoverType coverType;
        [SerializeField] private Vector3 coverDirection;
        [SerializeField] private bool canPeakLeft;
        [SerializeField] private bool canPeakRight;


        public InternalCoverPost(Vector3 position, CoverType coverType, Vector3 coverDirection, bool canPeakLeft, bool canPeakRight)
        {
            this.position = position;
            this.coverType = coverType;
            this.coverDirection = coverDirection;
            this.canPeakLeft = canPeakLeft;
            this.canPeakRight = canPeakRight;
        }
    }
}
