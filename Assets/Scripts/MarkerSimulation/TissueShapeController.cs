using System.Collections.Generic;
using GonadFateSim.Simulation;
using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public sealed class TissueShapeController
    {
        private const int RegionCount = 4;
        private const int GapCount = 3;

        private readonly TissueRegionDescriptor[] regions = new TissueRegionDescriptor[RegionCount];
        private readonly TissueRegionDescriptor[] gaps = new TissueRegionDescriptor[GapCount];
        private readonly TissueCompartmentDescriptor[] compartments =
        {
            new TissueCompartmentDescriptor(0, new Vector2(0.24f, 0.64f), new Vector2(0.16f, 0.09f), -0.34f, 3.1f),
            new TissueCompartmentDescriptor(1, new Vector2(0.42f, 0.42f), new Vector2(0.17f, 0.09f), 0.26f, 7.2f),
            new TissueCompartmentDescriptor(2, new Vector2(0.62f, 0.62f), new Vector2(0.16f, 0.09f), -0.22f, 11.3f),
            new TissueCompartmentDescriptor(3, new Vector2(0.78f, 0.4f), new Vector2(0.15f, 0.085f), 0.32f, 17.4f)
        };
        private GonadFate cachedFate = (GonadFate)(-1);

        public IReadOnlyList<TissueRegionDescriptor> Regions => regions;
        public IReadOnlyList<TissueRegionDescriptor> Gaps => gaps;

        public void UpdateForVisualState(VisualStateDescriptor visualState)
        {
            visualState.Clamp01();

            for (int index = 0; index < RegionCount; index++)
            {
                TissueCompartmentDescriptor compartment = compartments[index];
                float spacing = Mathf.Lerp(0.76f, 1.18f, visualState.CompartmentSpacing);
                Vector2 center = new Vector2(
                    0.5f + (compartment.Center.x - 0.5f) * spacing,
                    0.5f + (compartment.Center.y - 0.5f) * Mathf.Lerp(0.86f, 1.08f, visualState.CompartmentSpacing));

                float elongation = Mathf.Lerp(0.75f, 1.32f, visualState.Elongation);
                float roundness = Mathf.Lerp(0.72f, 1.28f, visualState.Roundness);
                Vector2 radius = new Vector2(
                    Mathf.Lerp(compartment.Radius.x * elongation, 0.145f * roundness, visualState.Roundness),
                    Mathf.Lerp(compartment.Radius.y * (1.08f - visualState.Elongation * 0.22f), 0.145f * roundness, visualState.Roundness));

                float instabilityJitter = (Noise(compartment.Seed + 1.7f) - 0.5f) * 0.05f * visualState.Fragmentation;
                center.x = Mathf.Clamp01(center.x + instabilityJitter);
                center.y = Mathf.Clamp01(center.y + (Noise(compartment.Seed + 5.9f) - 0.5f) * 0.05f * visualState.Fragmentation);
                regions[index] = new TissueRegionDescriptor(center, radius, Mathf.Lerp(0.85f, 1f, Noise(compartment.Seed)));
            }

            for (int index = 0; index < GapCount; index++)
            {
                TissueRegionDescriptor source = regions[index];
                float lumen = Mathf.Lerp(0.14f, 0.34f, visualState.LumenSize);
                gaps[index] = new TissueRegionDescriptor(
                    source.Center,
                    new Vector2(
                        Mathf.Max(0.024f, source.Radius.x * lumen),
                        Mathf.Max(0.02f, source.Radius.y * Mathf.Lerp(0.32f, 0.52f, visualState.LumenSize))),
                    Mathf.Lerp(0.6f, 1f, visualState.LumenSoftness));
            }
        }

        public void UpdateForState(GonadState state)
        {
            if (state.Fate == cachedFate)
            {
                return;
            }

            cachedFate = state.Fate;
            System.Random random = new System.Random(4519 + (int)state.Fate * 977);

            for (int index = 0; index < RegionCount; index++)
            {
                float band = index / (float)(RegionCount - 1);
                regions[index] = new TissueRegionDescriptor(
                    CenterFor(state.Fate, band, random),
                    RadiusFor(state.Fate, random),
                    Lerp(0.75f, 1f, Next(random)));
            }

            for (int index = 0; index < GapCount; index++)
            {
                TissueRegionDescriptor source = regions[GapSourceIndex(state.Fate, index)];
                float jitter = state.Fate == GonadFate.Unstable ? 0.055f : 0.025f;
                gaps[index] = new TissueRegionDescriptor(
                    new Vector2(
                        Mathf.Clamp01(source.Center.x + Lerp(-jitter, jitter, Next(random))),
                        Mathf.Clamp01(source.Center.y + Lerp(-jitter, jitter, Next(random)))),
                    GapRadiusFor(state.Fate, source, random),
                    Lerp(0.7f, 1f, Next(random)));
            }
        }

        private static Vector2 CenterFor(GonadFate fate, float band, System.Random random)
        {
            return fate switch
            {
                GonadFate.TestisLike => TestisCenter(band, random),
                GonadFate.OvaryLike => OvaryCenter(band, random),
                GonadFate.Ovotestis => band < 0.5f ? TestisCenter(band * 1.4f, random) : OvaryCenter((band - 0.5f) * 1.8f, random),
                GonadFate.Unstable => UnstableCenter(band, random),
                _ => new Vector2(Lerp(0.24f, 0.76f, band), Lerp(0.3f, 0.7f, Next(random)))
            };
        }

        private static Vector2 RadiusFor(GonadFate fate, System.Random random)
        {
            return fate switch
            {
                GonadFate.TestisLike => new Vector2(Lerp(0.15f, 0.21f, Next(random)), Lerp(0.065f, 0.095f, Next(random))),
                GonadFate.OvaryLike => new Vector2(Lerp(0.105f, 0.17f, Next(random)), Lerp(0.12f, 0.19f, Next(random))),
                GonadFate.Ovotestis => Next(random) < 0.5f
                    ? new Vector2(Lerp(0.15f, 0.2f, Next(random)), Lerp(0.065f, 0.095f, Next(random)))
                    : new Vector2(Lerp(0.105f, 0.16f, Next(random)), Lerp(0.12f, 0.18f, Next(random))),
                GonadFate.Unstable => new Vector2(Lerp(0.1f, 0.19f, Next(random)), Lerp(0.075f, 0.17f, Next(random))),
                _ => new Vector2(Lerp(0.13f, 0.2f, Next(random)), Lerp(0.1f, 0.17f, Next(random)))
            };
        }

        private static Vector2 TestisCenter(float band, System.Random random)
        {
            Vector2[] anchors =
            {
                new Vector2(0.24f, 0.65f),
                new Vector2(0.42f, 0.42f),
                new Vector2(0.62f, 0.63f),
                new Vector2(0.78f, 0.4f)
            };
            int index = Mathf.Clamp((int)(band * anchors.Length), 0, anchors.Length - 1);
            return Jitter(anchors[index], 0.018f, random);
        }

        private static Vector2 OvaryCenter(float band, System.Random random)
        {
            Vector2[] anchors =
            {
                new Vector2(0.32f, 0.62f),
                new Vector2(0.5f, 0.44f),
                new Vector2(0.67f, 0.64f),
                new Vector2(0.62f, 0.28f)
            };
            int index = Mathf.Clamp((int)(band * anchors.Length), 0, anchors.Length - 1);
            return Jitter(anchors[index], 0.035f, random);
        }

        private static Vector2 UnstableCenter(float band, System.Random random)
        {
            Vector2 basePoint = band < 0.34f
                ? new Vector2(0.28f, 0.58f)
                : band < 0.67f
                    ? new Vector2(0.56f, 0.52f)
                    : new Vector2(0.72f, 0.34f);
            return Jitter(basePoint, 0.075f, random);
        }

        private static int GapSourceIndex(GonadFate fate, int gapIndex)
        {
            return fate switch
            {
                GonadFate.TestisLike => Mathf.Clamp(gapIndex, 0, 2),
                GonadFate.Ovotestis => gapIndex < 2 ? gapIndex : 0,
                GonadFate.OvaryLike => Mathf.Clamp(gapIndex + 1, 1, 3),
                _ => gapIndex == 2 ? 3 : gapIndex
            };
        }

        private static Vector2 GapRadiusFor(GonadFate fate, TissueRegionDescriptor source, System.Random random)
        {
            return fate switch
            {
                GonadFate.TestisLike => new Vector2(
                    Mathf.Max(0.038f, source.Radius.x * Lerp(0.2f, 0.3f, Next(random))),
                    Mathf.Max(0.026f, source.Radius.y * Lerp(0.34f, 0.5f, Next(random)))),
                GonadFate.Ovotestis => new Vector2(
                    Mathf.Max(0.038f, source.Radius.x * Lerp(0.2f, 0.32f, Next(random))),
                    Mathf.Max(0.026f, source.Radius.y * Lerp(0.34f, 0.52f, Next(random)))),
                GonadFate.OvaryLike => new Vector2(
                    Mathf.Max(0.032f, source.Radius.x * Lerp(0.18f, 0.28f, Next(random))),
                    Mathf.Max(0.03f, source.Radius.y * Lerp(0.18f, 0.3f, Next(random)))),
                _ => new Vector2(
                    Mathf.Max(0.035f, source.Radius.x * Lerp(0.25f, 0.42f, Next(random))),
                    Mathf.Max(0.03f, source.Radius.y * Lerp(0.3f, 0.5f, Next(random))))
            };
        }

        private static Vector2 Jitter(Vector2 point, float amount, System.Random random)
        {
            return new Vector2(
                Mathf.Clamp01(point.x + Lerp(-amount, amount, Next(random))),
                Mathf.Clamp01(point.y + Lerp(-amount, amount, Next(random))));
        }

        private static float Next(System.Random random)
        {
            return (float)random.NextDouble();
        }

        private static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        private static float Noise(float value)
        {
            float raw = Mathf.Sin(value * 12.9898f) * 43758.5453f;
            return raw - (float)System.Math.Floor(raw);
        }
    }
}
