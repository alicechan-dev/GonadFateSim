using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public readonly struct TissueCompartmentDescriptor
    {
        public readonly int StableId;
        public readonly Vector2 Center;
        public readonly Vector2 Radius;
        public readonly float Orientation;
        public readonly float Seed;

        public TissueCompartmentDescriptor(int stableId, Vector2 center, Vector2 radius, float orientation, float seed)
        {
            StableId = stableId;
            Center = center;
            Radius = radius;
            Orientation = orientation;
            Seed = seed;
        }
    }
}
