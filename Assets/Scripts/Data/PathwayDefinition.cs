using System.Collections.Generic;
using UnityEngine;

namespace GonadFateSim.Data
{
    [CreateAssetMenu(menuName = "Gonad Fate Sim/Pathway Definition", fileName = "PathwayDefinition")]
    public sealed class PathwayDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [TextArea]
        [SerializeField] private string shortDescription;
        [TextArea]
        [SerializeField] private string programmerAnalogy;
        [SerializeField] private Color displayColor = Color.white;
        [SerializeField] private List<GeneDefinition> relatedGenes = new List<GeneDefinition>();

        public string Id => id;
        public string DisplayName => displayName;
        public string ShortDescription => shortDescription;
        public string ProgrammerAnalogy => programmerAnalogy;
        public Color DisplayColor => displayColor;
        public IReadOnlyList<GeneDefinition> RelatedGenes => relatedGenes;
    }
}
