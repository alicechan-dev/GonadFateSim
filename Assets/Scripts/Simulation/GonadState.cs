using System;

namespace GonadFateSim.Simulation
{
    [Serializable]
    public struct GonadState
    {
        public GonadFate Fate;
        public float TestisScore;
        public float OvaryScore;
        public float OvotestisScore;
        public float InstabilityScore;
        public float EffectiveSryScore;
        public float Sox9Score;
        public float WntBetaCateninScore;
        public string Explanation;

        public PathwayState TestisPathway => new("SOX9/Testis", TestisScore, Fate == GonadFate.TestisLike);
        public PathwayState OvaryPathway => new("WNT/beta-catenin/Ovary", OvaryScore, Fate == GonadFate.OvaryLike);
        public PathwayState InstabilityPathway => new("Instability / mixed-state", InstabilityScore, Fate == GonadFate.Unstable || Fate == GonadFate.Ovotestis);
    }
}
