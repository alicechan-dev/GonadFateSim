using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public static class FluorescenceStamp
    {
        public static void AddSoftCircle(Color[] pixels, int width, int height, Vector2 normalizedPosition, float radiusPixels, Color color, float intensity)
        {
            int centerX = Mathf.Clamp((int)(normalizedPosition.x * (width - 1)), 0, width - 1);
            int centerY = Mathf.Clamp((int)(normalizedPosition.y * (height - 1)), 0, height - 1);
            int radius = Mathf.Max(1, (int)(radiusPixels * 3f));

            for (int y = centerY - radius; y <= centerY + radius; y++)
            {
                if (y < 0 || y >= height)
                {
                    continue;
                }

                for (int x = centerX - radius; x <= centerX + radius; x++)
                {
                    if (x < 0 || x >= width)
                    {
                        continue;
                    }

                    float dx = (x - centerX) / Mathf.Max(radiusPixels, 0.001f);
                    float dy = (y - centerY) / Mathf.Max(radiusPixels, 0.001f);
                    float distanceSquared = dx * dx + dy * dy;
                    if (distanceSquared > 9f)
                    {
                        continue;
                    }

                    float alpha = SoftFalloff(distanceSquared) * intensity;
                    int pixelIndex = y * width + x;
                    Color current = pixels[pixelIndex];
                    pixels[pixelIndex] = new Color(
                        Mathf.Clamp01(current.r + color.r * alpha),
                        Mathf.Clamp01(current.g + color.g * alpha),
                        Mathf.Clamp01(current.b + color.b * alpha),
                        1f);
                }
            }
        }

        private static float SoftFalloff(float distanceSquared)
        {
            float core = Mathf.Clamp01(1f - distanceSquared / 9f);
            return core * core;
        }
    }
}
