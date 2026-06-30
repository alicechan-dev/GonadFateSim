using GonadFateSim.Simulation;
using UnityEngine;

namespace GonadFateSim.Data
{
    [CreateAssetMenu(menuName = "Gonad Fate Sim/Scenario Definition", fileName = "ScenarioDefinition")]
    public sealed class ScenarioDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [TextArea]
        [SerializeField] private string shortDescription;
        [SerializeField] private SimulationParameters parameters = SimulationParameters.Default;

        public string Id => id;
        public string DisplayName => displayName;
        public string ShortDescription => shortDescription;
        public SimulationParameters Parameters => parameters;
    }
}
