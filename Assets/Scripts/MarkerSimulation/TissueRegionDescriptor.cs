using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public readonly struct TissueRegionDescriptor
    {
        public readonly Vector2 Center;
        public readonly Vector2 Radius;
        public readonly float Weight;

        public TissueRegionDescriptor(Vector2 center, Vector2 radius, float weight)
        {
            Center = center;
            Radius = radius;
            Weight = weight;
        }

        public Vector4 ToVector()
        {
            return new Vector4(Center.x, Center.y, Radius.x, Radius.y);
        }
    }
}
