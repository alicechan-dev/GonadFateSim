using System;
using GonadFateSim.Data;
using GonadFateSim.Simulation;
using UnityEngine;

namespace GonadFateSim.UI
{
    public sealed class SimulationController : MonoBehaviour
    {
        [SerializeField] private SimulationParameters parameters = SimulationParameters.Default;
        [SerializeField] private bool useSeededNoise = true;
        [SerializeField] private int noiseSeed = 42;

        private readonly SexDeterminationSimulator simulator = new SexDeterminationSimulator();

        public event Action<GonadState> StateChanged;

        public SimulationParameters Parameters => parameters;
        public GonadState CurrentState { get; private set; }

        private void Awake()
        {
            Refresh();
        }

        private void OnValidate()
        {
            parameters = parameters.Normalized();
        }

        public void SetSryStrength(float value) => SetParameter(value, parameter => parameters.SryStrength = parameter);
        public void SetSryTiming(float value) => SetParameter(value, parameter => parameters.SryTiming = parameter);
        public void SetSox9Sensitivity(float value) => SetParameter(value, parameter => parameters.Sox9Sensitivity = parameter);
        public void SetWntBetaCateninStrength(float value) => SetParameter(value, parameter => parameters.WntBetaCateninStrength = parameter);
        public void SetGeneticBackgroundModifier(float value) => SetParameter(value, parameter => parameters.GeneticBackgroundModifier = parameter);
        public void SetNoise(float value) => SetParameter(value, parameter => parameters.Noise = parameter);

        public void ApplyScenario(ScenarioDefinition scenario)
        {
            if (scenario == null)
            {
                return;
            }

            parameters = scenario.Parameters.Normalized();
            Refresh();
        }

        public void ResetToDefault()
        {
            parameters = SimulationParameters.Default;
            Refresh();
        }

        public void Refresh()
        {
            CurrentState = simulator.Simulate(parameters, useSeededNoise ? noiseSeed : null);
            StateChanged?.Invoke(CurrentState);
        }

        private void SetParameter(float value, Action<float> assign)
        {
            assign(Mathf.Clamp01(value));
            Refresh();
        }
    }
}
