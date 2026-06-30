using GonadFateSim.Simulation;
using UnityEngine;
using UnityEngine.UI;

namespace GonadFateSim.UI
{
    public sealed class SimulationResultView : MonoBehaviour
    {
        [SerializeField] private SimulationController controller;
        [SerializeField] private Text fateLabel;
        [SerializeField] private Text explanationLabel;
        [SerializeField] private Slider testisScoreBar;
        [SerializeField] private Slider ovaryScoreBar;
        [SerializeField] private Slider ovotestisScoreBar;
        [SerializeField] private Slider instabilityScoreBar;
        [SerializeField] private Text testisScoreLabel;
        [SerializeField] private Text ovaryScoreLabel;
        [SerializeField] private Text ovotestisScoreLabel;
        [SerializeField] private Text instabilityScoreLabel;

        private void OnEnable()
        {
            if (controller != null)
            {
                controller.StateChanged += Render;
                Render(controller.CurrentState);
            }
        }

        private void OnDisable()
        {
            if (controller != null)
            {
                controller.StateChanged -= Render;
            }
        }

        public void Render(GonadState state)
        {
            SetText(fateLabel, $"Predicted outcome: {FormatFate(state.Fate)}");
            SetText(explanationLabel, state.Explanation);
            SetScore(testisScoreBar, testisScoreLabel, state.TestisScore);
            SetScore(ovaryScoreBar, ovaryScoreLabel, state.OvaryScore);
            SetScore(ovotestisScoreBar, ovotestisScoreLabel, state.OvotestisScore);
            SetScore(instabilityScoreBar, instabilityScoreLabel, state.InstabilityScore);
        }

        private static void SetScore(Slider slider, Text label, float value)
        {
            float clampedValue = Mathf.Clamp01(value);
            if (slider != null)
            {
                slider.minValue = 0f;
                slider.maxValue = 1f;
                slider.SetValueWithoutNotify(clampedValue);
            }

            SetText(label, clampedValue.ToString("0.00"));
        }

        private static void SetText(Text label, string text)
        {
            if (label != null)
            {
                label.text = text;
            }
        }

        private static string FormatFate(GonadFate fate)
        {
            return fate switch
            {
                GonadFate.TestisLike => "Testis-like",
                GonadFate.OvaryLike => "Ovary-like",
                GonadFate.Ovotestis => "Ovotestis",
                _ => "Unstable"
            };
        }
    }
}
