using System;
using UnityEngine;
using UnityEngine.UI;

namespace GonadFateSim.UI
{
    public sealed class ParameterSliderView : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Text valueLabel;
        [SerializeField] private string valueFormat = "0.00";

        public event Action<float> ValueChanged;

        private void Awake()
        {
            if (slider != null)
            {
                slider.minValue = 0f;
                slider.maxValue = 1f;
                slider.onValueChanged.AddListener(HandleSliderChanged);
            }
        }

        private void OnDestroy()
        {
            if (slider != null)
            {
                slider.onValueChanged.RemoveListener(HandleSliderChanged);
            }
        }

        public void SetValueWithoutNotify(float value)
        {
            float clampedValue = Mathf.Clamp01(value);
            if (slider != null)
            {
                slider.SetValueWithoutNotify(clampedValue);
            }

            UpdateLabel(clampedValue);
        }

        private void HandleSliderChanged(float value)
        {
            float clampedValue = Mathf.Clamp01(value);
            UpdateLabel(clampedValue);
            ValueChanged?.Invoke(clampedValue);
        }

        private void UpdateLabel(float value)
        {
            if (valueLabel != null)
            {
                valueLabel.text = value.ToString(valueFormat);
            }
        }
    }
}
