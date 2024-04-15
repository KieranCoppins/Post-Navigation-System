using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    public class IsNotOccupied : IPostRule
    {
        private object self;

        public IsNotOccupied(object self)
        {
            this.self = self;
        }

        public Dictionary<IPost, float> Run(Dictionary<IPost, float> scores)
        {
            Dictionary<IPost, float> newScores = new Dictionary<IPost, float>();
            foreach (KeyValuePair<IPost, float> score in scores)
            {
                if (score.Key.OccupiedBy == null || score.Key.OccupiedBy == self)
                {
                    newScores[score.Key] = scores[score.Key];
                }
            }
            return newScores;
        }
    }
}
