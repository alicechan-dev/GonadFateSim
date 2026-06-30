using System;
using UnityEngine;

namespace GonadFateSim.Simulation
{
    [Serializable]
    public struct SimulationParameters
    {
        [Range(0f, 1f)] public float SryStrength;
        [Range(0f, 1f)] public float SryTiming;
        [Range(0f, 1f)] public float Sox9Sensitivity;
        [Range(0f, 1f)] public float WntBetaCateninStrength;
        [Range(0f, 1f)] public float GeneticBackgroundModifier;
        [Range(0f, 1f)] public float Noise;

        public static SimulationParameters Default => new SimulationParameters
        {
            SryStrength = 0.95f,
            SryTiming = 0.5f,
            Sox9Sensitivity = 0.95f,
            WntBetaCateninStrength = 0.2f,
            GeneticBackgroundModifier = 0.5f,
            Noise = 0f
        };

        public SimulationParameters Normalized()
        {
            return new SimulationParameters
            {
                SryStrength = Mathf.Clamp01(SryStrength),
                SryTiming = Mathf.Clamp01(SryTiming),
                Sox9Sensitivity = Mathf.Clamp01(Sox9Sensitivity),
                WntBetaCateninStrength = Mathf.Clamp01(WntBetaCateninStrength),
                GeneticBackgroundModifier = Mathf.Clamp01(GeneticBackgroundModifier),
                Noise = Mathf.Clamp01(Noise)
            };
        }
    }
}
