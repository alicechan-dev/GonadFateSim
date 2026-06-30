using UnityEngine;
using UnityEngine.UI;

namespace GonadFateSim.UI
{
    public sealed class SignalBarView : MonoBehaviour
    {
        [SerializeField] private Slider bar;
        [SerializeField] private Text label;
        [SerializeField] private string labelPrefix;

        public void Render(float value)
        {
            float clampedValue = Mathf.Clamp01(value);

            if (bar != null)
            {
                bar.minValue = 0f;
                bar.maxValue = 1f;
                bar.SetValueWithoutNotify(clampedValue);
            }

            if (label != null)
            {
                label.text = string.IsNullOrEmpty(labelPrefix)
                    ? clampedValue.ToString("0.00")
                    : $"{labelPrefix}: {clampedValue:0.00}";
            }
        }
    }
}
