using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A custom post that represents a cover object, contains extra data for a cover
    /// 
    /// Allows for a game designer to manually place a cover post
    /// </summary>
    public class CoverPost : MonoBehaviour, ICoverPost
    {
        Vector3 IPost.Position { get => transform.position; set => transform.position = value; }
        CoverType ICoverPost.CoverType { get => coverType; set => coverType = value; }
        Vector3 ICoverPost.CoverDirection { get => transform.forward; set => transform.forward = value; }
        bool ICoverPost.CanPeakLeft { get => canPeakLeft; set => canPeakLeft = value; }
        bool ICoverPost.CanPeakRight { get => canPeakRight; set => canPeakRight = value; }

        [SerializeField] private CoverType coverType;
        [SerializeField] private bool canPeakLeft;
        [SerializeField] private bool canPeakRight;
    }
}
