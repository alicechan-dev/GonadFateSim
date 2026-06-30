using System.Collections.Generic;
using GonadFateSim.Simulation;
using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public sealed class MarkerPatternRenderer
    {
        private const int TextureSize = 768;

        private readonly TissueMaskGenerator tissueMaskGenerator = new TissueMaskGenerator();
        private readonly NucleiFieldGenerator nucleiFieldGenerator = new NucleiFieldGenerator();

        public Texture2D Render(GonadState state)
        {
            System.Random random = new System.Random(BuildSeed(state));
            TissueMask mask = tissueMaskGenerator.Generate(state, random);
            IReadOnlyList<SyntheticNucleus> nuclei = nucleiFieldGenerator.Generate(mask, state, random);

            Color[] pixels = CreateBackground(mask);
            DrawMarkerChannels(pixels, mask, state, random);
            for (int index = 0; index < nuclei.Count; index++)
            {
                SyntheticNucleus nucleus = nuclei[index];
                FluorescenceStamp.AddSoftCircle(
                    pixels,
                    TextureSize,
                    TextureSize,
                    nucleus.Position,
                    nucleus.Radius,
                    new Color(0.1f, 0.24f, 1f, 1f),
                    nucleus.Brightness);
            }

            Texture2D texture = new Texture2D(TextureSize, TextureSize, TextureFormat.RGBA32, false);
            texture.name = $"Synthetic DAPI Marker Pattern {state.Fate}";
            for (int y = 0; y < TextureSize; y++)
            {
                for (int x = 0; x < TextureSize; x++)
                {
                    texture.SetPixel(x, y, pixels[y * TextureSize + x]);
                }
            }

            texture.Apply();
            return texture;
        }

        private static void DrawMarkerChannels(Color[] pixels, TissueMask mask, GonadState state, System.Random random)
        {
            (int greenRegions, int redRegions, float greenIntensity, float redIntensity) = ChannelProfile(state.Fate);
            DrawChannelRegions(pixels, mask, random, greenRegions, new Color(0.02f, 0.9f, 0.24f, 1f), greenIntensity);
            DrawChannelRegions(pixels, mask, random, redRegions, new Color(0.95f, 0.04f, 0.12f, 1f), redIntensity);
        }

        private static (int greenRegions, int redRegions, float greenIntensity, float redIntensity) ChannelProfile(GonadFate fate)
        {
            return fate switch
            {
                GonadFate.TestisLike => (22, 5, 0.18f, 0.035f),
                GonadFate.OvaryLike => (5, 22, 0.035f, 0.18f),
                GonadFate.Ovotestis => (15, 15, 0.12f, 0.12f),
                GonadFate.Unstable => (8, 8, 0.055f, 0.055f),
                _ => (5, 5, 0.04f, 0.04f)
            };
        }

        private static void DrawChannelRegions(Color[] pixels, TissueMask mask, System.Random random, int regionCount, Color color, float intensity)
        {
            for (int region = 0; region < regionCount; region++)
            {
                Vector2 center = FindPointInMask(mask, random);
                float regionRadius = Lerp(12f, 34f, Next(random));
                float regionIntensity = intensity * Lerp(0.55f, 1.1f, Next(random));
                FluorescenceStamp.AddSoftCircle(pixels, TextureSize, TextureSize, center, regionRadius, color, regionIntensity);

                int punctaCount = 12 + random.Next(28);
                for (int puncta = 0; puncta < punctaCount; puncta++)
                {
                    float spread = Lerp(0.012f, 0.055f, Next(random));
                    Vector2 local = new Vector2(
                        Mathf.Clamp01(center.x + Lerp(-spread, spread, Next(random))),
                        Mathf.Clamp01(center.y + Lerp(-spread, spread, Next(random))));
                    if (mask.Contains(local))
                    {
                        FluorescenceStamp.AddSoftCircle(pixels, TextureSize, TextureSize, local, Lerp(2.4f, 5.8f, Next(random)), color, regionIntensity * Lerp(0.65f, 1.4f, Next(random)));
                    }
                }
            }
        }

        private static Vector2 FindPointInMask(TissueMask mask, System.Random random)
        {
            for (int attempt = 0; attempt < 140; attempt++)
            {
                Vector2 point = new Vector2(Next(random), Next(random));
                if (mask.Contains(point))
                {
                    return point;
                }
            }

            return new Vector2(0.5f, 0.5f);
        }

        private static Color[] CreateBackground(TissueMask mask)
        {
            Color[] pixels = new Color[TextureSize * TextureSize];
            for (int y = 0; y < TextureSize; y++)
            {
                for (int x = 0; x < TextureSize; x++)
                {
                    Vector2 point = new Vector2(x / (float)(TextureSize - 1), y / (float)(TextureSize - 1));
                    float tissue = mask.DensityAt(point);
                    float tissueGlow = tissue * 0.032f;
                    pixels[y * TextureSize + x] = new Color(0.005f + tissueGlow, 0.007f + tissueGlow, 0.015f + tissueGlow * 1.4f, 1f);
                }
            }

            return pixels;
        }

        private static int BuildSeed(GonadState state)
        {
            int seed = 92821;
            seed = seed * 31 + (int)state.Fate;
            seed = seed * 31 + Quantize(state.TestisScore);
            seed = seed * 31 + Quantize(state.OvaryScore);
            seed = seed * 31 + Quantize(state.InstabilityScore);
            return seed;
        }

        private static int Quantize(float value)
        {
            return (int)(Mathf.Clamp01(value) * 100f);
        }

        private static float Next(System.Random random)
        {
            return (float)random.NextDouble();
        }

        private static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }
    }
}
