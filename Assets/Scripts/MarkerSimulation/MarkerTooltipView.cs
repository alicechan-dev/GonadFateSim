using UnityEngine;
using UnityEngine.UI;

namespace GonadFateSim.MarkerSimulation
{
    public sealed class MarkerTooltipView
    {
        private RectTransform tooltipRect;
        private Text tooltipText;

        public void Show(RectTransform parent, Vector2 localPoint, HoverZoneInfo zone)
        {
            Ensure(parent);
            tooltipText.text = Format(zone);
            tooltipRect.anchoredPosition = ClampedPosition(parent, localPoint + new Vector2(22f, -18f));
            tooltipRect.gameObject.SetActive(true);
            tooltipRect.SetAsLastSibling();
        }

        public void Hide()
        {
            if (tooltipRect != null)
            {
                tooltipRect.gameObject.SetActive(false);
            }
        }

        private void Ensure(RectTransform parent)
        {
            if (tooltipRect != null)
            {
                return;
            }

            GameObject tooltipObject = new GameObject("Marker Hover Tooltip", typeof(RectTransform), typeof(Image));
            tooltipObject.transform.SetParent(parent, false);
            tooltipRect = tooltipObject.GetComponent<RectTransform>();
            tooltipRect.anchorMin = new Vector2(0.5f, 0.5f);
            tooltipRect.anchorMax = new Vector2(0.5f, 0.5f);
            tooltipRect.pivot = new Vector2(0f, 1f);
            tooltipRect.sizeDelta = new Vector2(330f, 188f);

            Image background = tooltipObject.GetComponent<Image>();
            background.color = new Color(0.025f, 0.032f, 0.04f, 0.96f);
            background.raycastTarget = false;

            GameObject textObject = new GameObject("Tooltip Text", typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(tooltipObject.transform, false);
            tooltipText = textObject.GetComponent<Text>();
            tooltipText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            tooltipText.fontSize = 13;
            tooltipText.alignment = TextAnchor.UpperLeft;
            tooltipText.horizontalOverflow = HorizontalWrapMode.Wrap;
            tooltipText.verticalOverflow = VerticalWrapMode.Overflow;
            tooltipText.color = new Color(0.9f, 0.94f, 0.98f, 1f);
            tooltipText.raycastTarget = false;
            RectTransform textRect = tooltipText.rectTransform;
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(12f, 10f);
            textRect.offsetMax = new Vector2(-12f, -10f);

            tooltipRect.gameObject.SetActive(false);
        }

        private Vector2 ClampedPosition(RectTransform parent, Vector2 desired)
        {
            Rect rect = parent.rect;
            Vector2 size = tooltipRect.sizeDelta;
            float xMin = rect.xMin + 8f;
            float xMax = rect.xMax - size.x - 8f;
            float yMin = rect.yMin + size.y + 8f;
            float yMax = rect.yMax - 8f;
            return new Vector2(Mathf.Clamp(desired.x, xMin, xMax), Mathf.Clamp(desired.y, yMin, yMax));
        }

        private static string Format(HoverZoneInfo zone)
        {
            return string.Format(
                "{0}\n\nInterpretation:\n{1}\n\nSOX9-like: {2:0.00}   FOXL2-like: {3:0.00}   DAPI-like: {4:0.00}\n\nSimulated interpretation, not real microscopy.",
                zone.Name,
                zone.Interpretation,
                zone.Sox9Like,
                zone.Foxl2Like,
                zone.DapiLike);
        }
    }
}
