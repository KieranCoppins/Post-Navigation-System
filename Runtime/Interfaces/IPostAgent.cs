using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// An interface that all agents that use posts or zones should implement
    /// </summary>
    public interface IPostAgent
    {
        /// <summary>
        /// The position of the agent
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// Called when the zone manager assigns this agent to a zone
        /// </summary>
        public Action<Zone> OnAssignedZone { get; set; }
    }
}
