using System.Collections.Generic;
using GonadFateSim.Simulation;
using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public readonly struct TissueBlob
    {
        public readonly Vector2 Center;
        public readonly Vector2 Radius;
        public readonly float Weight;

        public TissueBlob(Vector2 center, Vector2 radius, float weight)
        {
            Center = center;
            Radius = radius;
            Weight = weight;
        }
    }

    public sealed class TissueMask
    {
        private readonly IReadOnlyList<TissueBlob> positiveBlobs;
        private readonly IReadOnlyList<TissueBlob> negativeBlobs;

        public TissueMask(IReadOnlyList<TissueBlob> positiveBlobs, IReadOnlyList<TissueBlob> negativeBlobs)
        {
            this.positiveBlobs = positiveBlobs;
            this.negativeBlobs = negativeBlobs;
        }

        public bool Contains(Vector2 point)
        {
            return DensityAt(point) > 0.38f;
        }

        public float DensityAt(Vector2 point)
        {
            float density = 0f;
            for (int index = 0; index < positiveBlobs.Count; index++)
            {
                density += BlobContribution(positiveBlobs[index], point);
            }

            for (int index = 0; index < negativeBlobs.Count; index++)
            {
                density -= BlobContribution(negativeBlobs[index], point) * 1.15f;
            }

            return Mathf.Clamp01(density);
        }

        private static float BlobContribution(TissueBlob blob, Vector2 point)
        {
            float dx = (point.x - blob.Center.x) / Mathf.Max(blob.Radius.x, 0.001f);
            float dy = (point.y - blob.Center.y) / Mathf.Max(blob.Radius.y, 0.001f);
            float distanceSquared = dx * dx + dy * dy;
            if (distanceSquared >= 1f)
            {
                return 0f;
            }

            float falloff = 1f - distanceSquared;
            return falloff * falloff * blob.Weight;
        }
    }

    public sealed class TissueMaskGenerator
    {
        public TissueMask Generate(GonadState state, System.Random random)
        {
            List<TissueBlob> tissue = new List<TissueBlob>();
            List<TissueBlob> gaps = new List<TissueBlob>();

            int tissueBlobCount = state.Fate switch
            {
                GonadFate.OvaryLike => 10,
                GonadFate.Ovotestis => 11,
                GonadFate.Unstable => 7,
                _ => 9
            };

            for (int index = 0; index < tissueBlobCount; index++)
            {
                float band = tissueBlobCount <= 1 ? 0.5f : index / (float)(tissueBlobCount - 1);
                Vector2 center = CenterFor(state.Fate, band, random);
                Vector2 radius = RadiusFor(state.Fate, random);
                float weight = Lerp(0.72f, 1.05f, Next(random));
                tissue.Add(new TissueBlob(center, radius, weight));
            }

            int gapCount = state.Fate == GonadFate.Unstable ? 7 : 5;
            for (int index = 0; index < gapCount; index++)
            {
                gaps.Add(new TissueBlob(
                    new Vector2(Lerp(0.15f, 0.86f, Next(random)), Lerp(0.14f, 0.86f, Next(random))),
                    new Vector2(Lerp(0.055f, 0.15f, Next(random)), Lerp(0.05f, 0.18f, Next(random))),
                    Lerp(0.55f, 0.95f, Next(random))));
            }

            return new TissueMask(tissue, gaps);
        }

        private static Vector2 CenterFor(GonadFate fate, float band, System.Random random)
        {
            return fate switch
            {
                GonadFate.TestisLike => new Vector2(
                    Lerp(0.18f, 0.82f, band) + Lerp(-0.05f, 0.05f, Next(random)),
                    0.5f + 0.18f * Mathf.Sin(band * 6.28f) + Lerp(-0.08f, 0.08f, Next(random))),
                GonadFate.OvaryLike => new Vector2(
                    Lerp(0.2f, 0.8f, Next(random)),
                    Lerp(0.2f, 0.82f, Next(random))),
                GonadFate.Ovotestis => new Vector2(
                    Lerp(0.18f, 0.82f, band),
                    Lerp(0.22f, 0.78f, Next(random))),
                GonadFate.Unstable => new Vector2(
                    Lerp(0.18f, 0.82f, Next(random)),
                    Lerp(0.18f, 0.82f, Next(random))),
                _ => new Vector2(
                    Lerp(0.22f, 0.78f, band),
                    Lerp(0.28f, 0.72f, Next(random)))
            };
        }

        private static Vector2 RadiusFor(GonadFate fate, System.Random random)
        {
            return fate switch
            {
                GonadFate.TestisLike => new Vector2(Lerp(0.16f, 0.24f, Next(random)), Lerp(0.11f, 0.18f, Next(random))),
                GonadFate.OvaryLike => new Vector2(Lerp(0.12f, 0.2f, Next(random)), Lerp(0.12f, 0.22f, Next(random))),
                GonadFate.Unstable => new Vector2(Lerp(0.1f, 0.2f, Next(random)), Lerp(0.08f, 0.18f, Next(random))),
                _ => new Vector2(Lerp(0.13f, 0.22f, Next(random)), Lerp(0.1f, 0.2f, Next(random)))
            };
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
