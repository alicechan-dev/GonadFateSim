using System;
using UnityEngine;

namespace GonadFateSim.Simulation
{
    [Serializable]
    public struct GeneSignal
    {
        public string Id;
        public string DisplayName;
        [Range(0f, 1f)] public float CurrentLevel;
        public AnimationCurve ActivityOverTime;

        public GeneSignal(string id, string displayName, float currentLevel, AnimationCurve activityOverTime = null)
        {
            Id = id;
            DisplayName = displayName;
            CurrentLevel = Mathf.Clamp01(currentLevel);
            ActivityOverTime = activityOverTime;
        }
    }
}
