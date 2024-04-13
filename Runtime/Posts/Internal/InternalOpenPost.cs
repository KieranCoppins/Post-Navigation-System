using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    [System.Serializable]
    internal class InternalOpenPost : IOpenPost
    {
        Vector3 IPost.Position { get => position; set => position = value; }
        [SerializeField] private Vector3 position;

        public InternalOpenPost(Vector3 position)
        {
            this.position = position;
        }
    }
}
