using GonadFateSim.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GonadFateSim.UI
{
    public sealed class ScenarioButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private SimulationController controller;
        [SerializeField] private ScenarioDefinition scenario;

        private void Awake()
        {
            if (button != null)
            {
                button.onClick.AddListener(ApplyScenario);
            }
        }

        private void OnDestroy()
        {
            if (button != null)
            {
                button.onClick.RemoveListener(ApplyScenario);
            }
        }

        private void ApplyScenario()
        {
            if (controller != null)
            {
                controller.ApplyScenario(scenario);
            }
        }
    }
}
