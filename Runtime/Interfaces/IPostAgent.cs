using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    public interface IPostAgent
    {
        public Vector3 Position { get; }
        /// <summary>
        /// Called when the zone manager assigns this agent to a zone
        /// </summary>
        public Action<Zone> OnAssignedZone { get; set; }
    }
}
