using GonadFateSim.Simulation;
using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public sealed class VisualStateMapper
    {
        public VisualStateDescriptor Map(GonadState state)
        {
            float testis = Mathf.Clamp01(state.TestisScore);
            float ovary = Mathf.Clamp01(state.OvaryScore);
            float mixed = Mathf.Clamp01(state.OvotestisScore);
            float instability = Mathf.Clamp01(state.InstabilityScore);
            float dominance = Mathf.Clamp01(Mathf.Max(testis, ovary));

            VisualStateDescriptor descriptor = new VisualStateDescriptor
            {
                TestisBias = testis,
                OvaryBias = ovary,
                Mixedness = Mathf.Clamp01(mixed + Mathf.Min(testis, ovary) * 0.35f),
                Instability = instability,
                Sox9Signal = SignalStrength(testis, mixed, instability),
                Foxl2Signal = SignalStrength(ovary, mixed, instability),
                DapiDensity = Mathf.Lerp(0.52f, 0.92f, dominance) * Mathf.Lerp(1f, 0.72f, instability),
                Elongation = Mathf.Clamp01(0.18f + testis * 0.72f + mixed * 0.22f - ovary * 0.2f - instability * 0.18f),
                Roundness = Mathf.Clamp01(0.18f + ovary * 0.72f + mixed * 0.22f - testis * 0.18f),
                Connectedness = Mathf.Clamp01(0.28f + mixed * 0.38f + ovary * 0.22f + testis * 0.18f - instability * 0.3f),
                Fragmentation = Mathf.Clamp01(instability * 0.82f + (1f - dominance) * 0.22f),
                LumenSize = Mathf.Clamp01(0.12f + testis * 0.48f + mixed * 0.18f - ovary * 0.18f),
                LumenSoftness = Mathf.Clamp01(0.32f + testis * 0.36f + mixed * 0.18f + instability * 0.18f),
                CompartmentSpacing = Mathf.Clamp01(0.28f + testis * 0.44f + mixed * 0.18f - ovary * 0.16f),
                MarkerPatchiness = Mathf.Clamp01(0.12f + instability * 0.68f + mixed * 0.24f + (1f - dominance) * 0.16f)
            };

            descriptor.Clamp01();
            return descriptor;
        }

        private static float SignalStrength(float primaryScore, float mixedness, float instability)
        {
            float signal = primaryScore * 0.78f + mixedness * 0.28f;
            return Mathf.Clamp01(signal * Mathf.Lerp(1f, 0.58f, instability));
        }
    }
}
