using UnityEngine;
using UnityEngine.UI;

namespace GonadFateSim.MarkerSimulation
{
    public sealed class HoverOutlineView
    {
        private RawImage outlineImage;
        private Texture2D outlineTexture;

        public void Show(RectTransform parent, Vector2 localPoint, HoverZoneInfo zone)
        {
            Ensure(parent);
            outlineImage.rectTransform.anchoredPosition = localPoint;
            outlineImage.rectTransform.sizeDelta = SizeFor(zone.Category);
            outlineImage.color = ColorFor(zone.Category);
            outlineImage.enabled = true;
            outlineImage.transform.SetAsLastSibling();
        }

        public void Hide()
        {
            if (outlineImage != null)
            {
                outlineImage.enabled = false;
            }
        }

        private void Ensure(RectTransform parent)
        {
            if (outlineImage != null)
            {
                return;
            }

            GameObject outlineObject = new GameObject("Marker Hover Soft Outline", typeof(RectTransform), typeof(RawImage));
            outlineObject.transform.SetParent(parent, false);
            RectTransform rect = outlineObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            outlineImage = outlineObject.GetComponent<RawImage>();
            outlineImage.texture = OutlineTexture;
            outlineImage.raycastTarget = false;
            outlineImage.enabled = false;
        }

        private Texture2D OutlineTexture
        {
            get
            {
                if (outlineTexture != null)
                {
                    return outlineTexture;
                }

                const int width = 160;
                const int height = 110;
                outlineTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
                outlineTexture.name = "Soft Hover Zone Outline";
                float cx = (width - 1) * 0.5f;
                float cy = (height - 1) * 0.5f;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        float nx = (x - cx) / cx;
                        float ny = (y - cy) / cy;
                        float ellipse = Mathf.Sqrt(nx * nx + ny * ny);
                        float ring = Mathf.Clamp01(1f - Mathf.Abs(ellipse - 0.78f) * 12f);
                        float glow = Mathf.Clamp01(1f - Mathf.Abs(ellipse - 0.78f) * 4.2f) * 0.32f;
                        float alpha = Mathf.Clamp01(ring * 0.72f + glow);
                        outlineTexture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                    }
                }

                outlineTexture.Apply();
                return outlineTexture;
            }
        }

        private static Vector2 SizeFor(HoverZoneCategory category)
        {
            return category switch
            {
                HoverZoneCategory.Mixed => new Vector2(170f, 116f),
                HoverZoneCategory.NucleiRichLowMarker => new Vector2(134f, 92f),
                _ => new Vector2(150f, 104f)
            };
        }

        private static Color ColorFor(HoverZoneCategory category)
        {
            return category switch
            {
                HoverZoneCategory.Sox9Dominant => new Color(0.18f, 1f, 0.42f, 0.82f),
                HoverZoneCategory.Foxl2Dominant => new Color(1f, 0.22f, 0.32f, 0.82f),
                HoverZoneCategory.Mixed => new Color(1f, 0.82f, 0.24f, 0.82f),
                HoverZoneCategory.NucleiRichLowMarker => new Color(0.35f, 0.5f, 1f, 0.78f),
                _ => Color.white
            };
        }
    }
}
