using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{

    /// <summary>
    /// An interface for custom monobehaviour posts, this allows for game designers to place custom posts in the scene
    /// </summary>
    public interface ICustomPost
    {
        /// <summary>
        /// Converts a custom monobehaviour post to an object IPost that can be serialized
        /// 
        /// Due to how Unity handles serialization, we need to convert the monobehaviour object to a serializable object.
        /// 
        /// For example, the CoverPost ICustomPost will convert to an InternalCoverPost ICoverPost object since the InternalCoverPost is serializable
        /// </summary>
        /// <returns></returns>
        public IPost ToSerializableObject();
    }
}
