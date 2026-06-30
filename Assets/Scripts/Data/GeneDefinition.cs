using UnityEngine;

namespace GonadFateSim.Data
{
    [CreateAssetMenu(menuName = "Gonad Fate Sim/Gene Definition", fileName = "GeneDefinition")]
    public sealed class GeneDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [TextArea]
        [SerializeField] private string shortDescription;
        [TextArea]
        [SerializeField] private string programmerAnalogy;
        [Range(0f, 1f)]
        [SerializeField] private float defaultActivity = 0.5f;
        [Range(0f, 1f)]
        [SerializeField] private float minActivity;
        [Range(0f, 1f)]
        [SerializeField] private float maxActivity = 1f;

        public string Id => id;
        public string DisplayName => displayName;
        public string ShortDescription => shortDescription;
        public string ProgrammerAnalogy => programmerAnalogy;
        public float DefaultActivity => defaultActivity;
        public float MinActivity => minActivity;
        public float MaxActivity => maxActivity;
    }
}
