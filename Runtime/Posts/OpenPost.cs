using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    /// <summary>
    /// A custom post that represents an open post
    /// 
    /// Allows for a game designer to manually place an open post
    /// </summary>
    public class OpenPost : MonoBehaviour, IOpenPost, ICustomPost
    {
        Vector3 IPost.Position { get => transform.position; set => transform.position = value; }

        public IPost ToSerializableObject() => new InternalOpenPost(transform.position);
    }
}