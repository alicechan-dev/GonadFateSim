using UnityEngine;

namespace GonadFateSim.Simulation
{
    public sealed class SexDeterminationSimulator
    {
        private const float InhibitionFactor = 0.45f;
        private const float TestisThreshold = 0.7f;
        private const float OvaryThreshold = 0.7f;
        private const float MixedThreshold = 0.4f;

        public GonadState Simulate(SimulationParameters parameters, int? noiseSeed = null)
        {
            SimulationParameters input = parameters.Normalized();
            float timingFactor = CalculateTimingFactor(input.SryTiming);
            float backgroundFactor = Mathf.Lerp(0.85f, 1.15f, input.GeneticBackgroundModifier);
            float effectiveSry = Mathf.Clamp01(input.SryStrength * timingFactor);
            float sox9Score = Mathf.Clamp01(effectiveSry * input.Sox9Sensitivity * backgroundFactor);
            float wntScore = Mathf.Clamp01(input.WntBetaCateninStrength * Mathf.Lerp(1.1f, 0.9f, input.GeneticBackgroundModifier));

            ApplyNoise(input.Noise, noiseSeed, ref sox9Score, ref wntScore);

            float testisScore = Mathf.Clamp01(sox9Score - wntScore * InhibitionFactor);
            float ovaryScore = Mathf.Clamp01(wntScore - sox9Score * InhibitionFactor);
            float closeness = 1f - Mathf.Clamp01(Mathf.Abs(testisScore - ovaryScore));
            float weakness = 1f - Mathf.Clamp01(Mathf.Max(testisScore, ovaryScore));
            float instabilityScore = Mathf.Clamp01(closeness * 0.6f + weakness * 0.4f);
            float ovotestisScore = Mathf.Clamp01(Mathf.Min(testisScore, ovaryScore) * 1.5f);
            GonadFate fate = Classify(testisScore, ovaryScore);

            return new GonadState
            {
                Fate = fate,
                TestisScore = testisScore,
                OvaryScore = ovaryScore,
                OvotestisScore = ovotestisScore,
                InstabilityScore = instabilityScore,
                EffectiveSryScore = effectiveSry,
                Sox9Score = sox9Score,
                WntBetaCateninScore = wntScore,
                Explanation = BuildExplanation(fate, input, timingFactor, sox9Score, wntScore)
            };
        }

        public float CalculateTimingFactor(float sryTiming)
        {
            float normalizedTiming = Mathf.Clamp01(sryTiming);
            return Mathf.Clamp01(1f - Mathf.Abs(normalizedTiming - 0.5f) * 2f);
        }

        private static void ApplyNoise(float noise, int? seed, ref float sox9Score, ref float wntScore)
        {
            if (noise <= 0f)
            {
                return;
            }

            System.Random random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
            float amplitude = Mathf.Clamp01(noise) * 0.12f;
            sox9Score = Mathf.Clamp01(sox9Score + NextCentered(random) * amplitude);
            wntScore = Mathf.Clamp01(wntScore + NextCentered(random) * amplitude);
        }

        private static float NextCentered(System.Random random)
        {
            return (float)(random.NextDouble() * 2.0 - 1.0);
        }

        private static GonadFate Classify(float testisScore, float ovaryScore)
        {
            if (testisScore > TestisThreshold && testisScore > ovaryScore)
            {
                return GonadFate.TestisLike;
            }

            if (ovaryScore > OvaryThreshold && ovaryScore > testisScore)
            {
                return GonadFate.OvaryLike;
            }

            if (testisScore > MixedThreshold && ovaryScore > MixedThreshold)
            {
                return GonadFate.Ovotestis;
            }

            return GonadFate.Unstable;
        }

        private static string BuildExplanation(
            GonadFate fate,
            SimulationParameters input,
            float timingFactor,
            float sox9Score,
            float wntScore)
        {
            bool sryInWindow = timingFactor >= 0.65f;
            bool sryStrong = input.SryStrength >= 0.65f && input.Sox9Sensitivity >= 0.65f;

            return fate switch
            {
                GonadFate.TestisLike when sryStrong && sryInWindow =>
                    "SRY signal was strong and inside the simplified timing window, allowing SOX9 activity to dominate over WNT/beta-catenin.",
                GonadFate.TestisLike =>
                    "SOX9/testis pathway activity dominated in this educational model despite some limiting timing or sensitivity inputs.",
                GonadFate.OvaryLike when sox9Score < 0.25f =>
                    "SRY/SOX9 activity was weak in the simplified model, so the WNT/beta-catenin ovarian-supporting pathway dominated.",
                GonadFate.OvaryLike =>
                    "WNT/beta-catenin activity dominated the pathway competition and biased the model toward an ovary-like output.",
                GonadFate.Ovotestis =>
                    "Both pathway scores remained active after mutual inhibition, producing a mixed educational outcome.",
                _ when Mathf.Abs(sox9Score - wntScore) < 0.15f =>
                    "The pathway scores were close, so the model reports an unstable bias rather than a clear fate.",
                _ =>
                    "Neither pathway reached a strong threshold, so the model reports an unstable educational outcome."
            };
        }
    }
}
