using GonadFateSim.Simulation;
using UnityEngine;

namespace GonadFateSim.UI
{
    public sealed class SimulationDashboardView : MonoBehaviour
    {
        [SerializeField] private SimulationController controller;
        [SerializeField] private ParameterSliderView sryStrengthSlider;
        [SerializeField] private ParameterSliderView sryTimingSlider;
        [SerializeField] private ParameterSliderView sox9SensitivitySlider;
        [SerializeField] private ParameterSliderView wntBetaCateninStrengthSlider;
        [SerializeField] private ParameterSliderView geneticBackgroundModifierSlider;
        [SerializeField] private ParameterSliderView noiseSlider;

        private void OnEnable()
        {
            BindSliders();

            if (controller != null)
            {
                controller.StateChanged += HandleStateChanged;
                SyncSliders(controller.Parameters);
            }
        }

        private void OnDisable()
        {
            UnbindSliders();

            if (controller != null)
            {
                controller.StateChanged -= HandleStateChanged;
            }
        }

        private void BindSliders()
        {
            if (sryStrengthSlider != null)
            {
                sryStrengthSlider.ValueChanged += HandleSryStrengthChanged;
            }

            if (sryTimingSlider != null)
            {
                sryTimingSlider.ValueChanged += HandleSryTimingChanged;
            }

            if (sox9SensitivitySlider != null)
            {
                sox9SensitivitySlider.ValueChanged += HandleSox9SensitivityChanged;
            }

            if (wntBetaCateninStrengthSlider != null)
            {
                wntBetaCateninStrengthSlider.ValueChanged += HandleWntBetaCateninStrengthChanged;
            }

            if (geneticBackgroundModifierSlider != null)
            {
                geneticBackgroundModifierSlider.ValueChanged += HandleGeneticBackgroundModifierChanged;
            }

            if (noiseSlider != null)
            {
                noiseSlider.ValueChanged += HandleNoiseChanged;
            }
        }

        private void UnbindSliders()
        {
            if (sryStrengthSlider != null)
            {
                sryStrengthSlider.ValueChanged -= HandleSryStrengthChanged;
            }

            if (sryTimingSlider != null)
            {
                sryTimingSlider.ValueChanged -= HandleSryTimingChanged;
            }

            if (sox9SensitivitySlider != null)
            {
                sox9SensitivitySlider.ValueChanged -= HandleSox9SensitivityChanged;
            }

            if (wntBetaCateninStrengthSlider != null)
            {
                wntBetaCateninStrengthSlider.ValueChanged -= HandleWntBetaCateninStrengthChanged;
            }

            if (geneticBackgroundModifierSlider != null)
            {
                geneticBackgroundModifierSlider.ValueChanged -= HandleGeneticBackgroundModifierChanged;
            }

            if (noiseSlider != null)
            {
                noiseSlider.ValueChanged -= HandleNoiseChanged;
            }
        }

        private void HandleStateChanged(GonadState state)
        {
            if (controller != null)
            {
                SyncSliders(controller.Parameters);
            }
        }

        private void SyncSliders(SimulationParameters parameters)
        {
            sryStrengthSlider?.SetValueWithoutNotify(parameters.SryStrength);
            sryTimingSlider?.SetValueWithoutNotify(parameters.SryTiming);
            sox9SensitivitySlider?.SetValueWithoutNotify(parameters.Sox9Sensitivity);
            wntBetaCateninStrengthSlider?.SetValueWithoutNotify(parameters.WntBetaCateninStrength);
            geneticBackgroundModifierSlider?.SetValueWithoutNotify(parameters.GeneticBackgroundModifier);
            noiseSlider?.SetValueWithoutNotify(parameters.Noise);
        }

        private void HandleSryStrengthChanged(float value) => controller?.SetSryStrength(value);
        private void HandleSryTimingChanged(float value) => controller?.SetSryTiming(value);
        private void HandleSox9SensitivityChanged(float value) => controller?.SetSox9Sensitivity(value);
        private void HandleWntBetaCateninStrengthChanged(float value) => controller?.SetWntBetaCateninStrength(value);
        private void HandleGeneticBackgroundModifierChanged(float value) => controller?.SetGeneticBackgroundModifier(value);
        private void HandleNoiseChanged(float value) => controller?.SetNoise(value);
    }
}
