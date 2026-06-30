using System.Collections.Generic;
using GonadFateSim.Simulation;
using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public readonly struct SyntheticNucleus
    {
        public readonly Vector2 Position;
        public readonly float Radius;
        public readonly float Brightness;

        public SyntheticNucleus(Vector2 position, float radius, float brightness)
        {
            Position = position;
            Radius = radius;
            Brightness = brightness;
        }
    }

    public sealed class NucleiFieldGenerator
    {
        public IReadOnlyList<SyntheticNucleus> Generate(TissueMask mask, GonadState state, System.Random random)
        {
            int targetCount = state.Fate == GonadFate.Unstable ? 1400 : 2200;
            int clusterCount = state.Fate == GonadFate.TestisLike ? 22 : 18;
            List<Vector2> clusters = CreateClusters(mask, clusterCount, random);
            List<SyntheticNucleus> nuclei = new List<SyntheticNucleus>(targetCount);

            int attempts = 0;
            int maxAttempts = targetCount * 35;
            while (nuclei.Count < targetCount && attempts < maxAttempts)
            {
                attempts++;
                Vector2 position = SamplePosition(mask, clusters, random);
                float density = mask.DensityAt(position);
                if (density <= 0.38f)
                {
                    continue;
                }

                float acceptance = Mathf.Clamp01(0.25f + density * 0.95f);
                if (Next(random) > acceptance)
                {
                    continue;
                }

                float radius = Lerp(1.05f, 2.25f, Next(random));
                float brightness = Lerp(0.46f, 0.98f, Next(random)) * Lerp(0.82f, 1.08f, density);
                if (state.Fate == GonadFate.Unstable)
                {
                    brightness *= Lerp(0.55f, 0.9f, Next(random));
                }

                nuclei.Add(new SyntheticNucleus(position, radius, brightness));
            }

            return nuclei;
        }

        private static List<Vector2> CreateClusters(TissueMask mask, int count, System.Random random)
        {
            List<Vector2> clusters = new List<Vector2>(count);
            int attempts = 0;
            while (clusters.Count < count && attempts < count * 80)
            {
                attempts++;
                Vector2 candidate = new Vector2(Next(random), Next(random));
                if (mask.Contains(candidate))
                {
                    clusters.Add(candidate);
                }
            }

            return clusters;
        }

        private static Vector2 SamplePosition(TissueMask mask, IReadOnlyList<Vector2> clusters, System.Random random)
        {
            if (clusters.Count > 0 && Next(random) < 0.72f)
            {
                Vector2 center = clusters[random.Next(clusters.Count)];
                float spread = Lerp(0.035f, 0.11f, Next(random));
                Vector2 candidate = new Vector2(
                    Mathf.Clamp01(center.x + Lerp(-spread, spread, Next(random))),
                    Mathf.Clamp01(center.y + Lerp(-spread, spread, Next(random))));
                if (mask.DensityAt(candidate) > 0.22f)
                {
                    return candidate;
                }
            }

            return new Vector2(Next(random), Next(random));
        }

        private static float Next(System.Random random)
        {
            return (float)random.NextDouble();
        }

        private static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
    }
}
