using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KieranCoppins.PostNavigation
{
    public class IsNotOccupied : IPostRule
    {
        private IPostAgent self;

        public IsNotOccupied(IPostAgent self)
        {
            this.self = self;
        }

        public Dictionary<IPost, float> Run(Dictionary<IPost, float> scores)
        {
            Dictionary<IPost, float> newScores = new Dictionary<IPost, float>();
            foreach (KeyValuePair<IPost, float> score in scores)
            {
                if (PostManager.Instance.IsPostOccupiedBy(score.Key, self) || !PostManager.Instance.IsPostOccupied(score.Key))
                {
                    newScores[score.Key] = scores[score.Key];
                }
            }
            return newScores;
        }
    }
}
