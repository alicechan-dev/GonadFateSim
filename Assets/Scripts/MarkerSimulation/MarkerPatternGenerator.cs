using System;
using System.Collections.Generic;
using GonadFateSim.Simulation;
using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public enum MarkerSignalType
    {
        Dapi,
        Sox9,
        Foxl2
    }

    public readonly struct MarkerCell
    {
        public readonly MarkerSignalType SignalType;
        public readonly Vector2 NormalizedPosition;
        public readonly float Radius;
        public readonly float Intensity;

        public MarkerCell(MarkerSignalType signalType, Vector2 normalizedPosition, float radius, float intensity)
        {
            SignalType = signalType;
            NormalizedPosition = normalizedPosition;
            Radius = radius;
            Intensity = intensity;
        }
    }

    public sealed class MarkerPatternGenerator
    {
        public IReadOnlyList<MarkerCell> Generate(GonadState state)
        {
            int seed = 173 + (int)state.Fate * 97;
            System.Random random = new System.Random(seed);
            List<MarkerCell> cells = new List<MarkerCell>();

            AddCells(cells, random, MarkerSignalType.Dapi, 70, 0.018f, 0.85f);

            switch (state.Fate)
            {
                case GonadFate.TestisLike:
                    AddCells(cells, random, MarkerSignalType.Sox9, 34, 0.026f, 0.95f);
                    AddCells(cells, random, MarkerSignalType.Foxl2, 8, 0.021f, 0.45f);
                    break;
                case GonadFate.OvaryLike:
                    AddCells(cells, random, MarkerSignalType.Sox9, 8, 0.021f, 0.45f);
                    AddCells(cells, random, MarkerSignalType.Foxl2, 34, 0.026f, 0.95f);
                    break;
                case GonadFate.Ovotestis:
                    AddCells(cells, random, MarkerSignalType.Sox9, 24, 0.024f, 0.82f, 0.18f, 0.52f);
                    AddCells(cells, random, MarkerSignalType.Foxl2, 24, 0.024f, 0.82f, 0.48f, 0.82f);
                    break;
                case GonadFate.Unstable:
                    AddCells(cells, random, MarkerSignalType.Sox9, 14, 0.02f, 0.5f);
                    AddCells(cells, random, MarkerSignalType.Foxl2, 14, 0.02f, 0.5f);
                    break;
                default:
                    AddCells(cells, random, MarkerSignalType.Sox9, 8, 0.019f, 0.35f);
                    AddCells(cells, random, MarkerSignalType.Foxl2, 8, 0.019f, 0.35f);
                    break;
            }

            return cells;
        }

        private static void AddCells(
            ICollection<MarkerCell> cells,
            System.Random random,
            MarkerSignalType signalType,
            int count,
            float radius,
            float intensity,
            float minX = 0.08f,
            float maxX = 0.92f)
        {
            for (int index = 0; index < count; index++)
            {
                float x = Lerp(minX, maxX, (float)random.NextDouble());
                float y = Lerp(0.1f, 0.9f, (float)random.NextDouble());
                float jitteredRadius = radius * Lerp(0.75f, 1.25f, (float)random.NextDouble());
                cells.Add(new MarkerCell(signalType, new Vector2(x, y), jitteredRadius, intensity));
            }
        }

        private static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
    }
}
