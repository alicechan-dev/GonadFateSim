using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public struct CompartmentMorphState
    {
        public Vector2 Center;
        public Vector2 Radius;
        public float LumenScale;

        public CompartmentMorphState(Vector2 center, Vector2 radius, float lumenScale)
        {
            Center = center;
            Radius = radius;
            LumenScale = lumenScale;
        }
    }
}
