using System;

namespace GonadFateSim.Simulation
{
    [Serializable]
    public struct PathwayState
    {
        public string PathwayName;
        public float ActivityScore;
        public bool IsDominant;

        public PathwayState(string pathwayName, float activityScore, bool isDominant)
        {
            PathwayName = pathwayName;
            ActivityScore = activityScore;
            IsDominant = isDominant;
        }
    }
}
