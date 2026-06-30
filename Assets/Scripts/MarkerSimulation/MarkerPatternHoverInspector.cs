using UnityEngine;
using UnityEngine.EventSystems;

namespace GonadFateSim.MarkerSimulation
{
    public sealed class MarkerPatternHoverInspector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        private readonly HoverZoneInterpreter interpreter = new HoverZoneInterpreter();
        private readonly HoverOutlineView outlineView = new HoverOutlineView();
        private readonly MarkerTooltipView tooltipView = new MarkerTooltipView();

        private RectTransform markerRect;
        private RectTransform overlayRoot;
        private VisualStateDescriptor visualState;
        private bool isInside;

        public void Initialize(RectTransform markerImageRect, RectTransform tooltipRoot)
        {
            markerRect = markerImageRect;
            overlayRoot = tooltipRoot != null ? tooltipRoot : markerImageRect;
        }

        public void SetVisualState(VisualStateDescriptor descriptor)
        {
            visualState = descriptor.Clamped();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isInside = true;
            Inspect(eventData);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (isInside)
            {
                Inspect(eventData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isInside = false;
            outlineView.Hide();
            tooltipView.Hide();
        }

        private void Inspect(PointerEventData eventData)
        {
            if (markerRect == null || overlayRoot == null)
            {
                return;
            }

            Vector2 localPoint;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(markerRect, eventData.position, eventData.pressEventCamera, out localPoint))
            {
                Hide();
                return;
            }

            Rect rect = markerRect.rect;
            float u = rect.width > 0f ? (localPoint.x - rect.xMin) / rect.width : 0f;
            float v = rect.height > 0f ? (localPoint.y - rect.yMin) / rect.height : 0f;

            if (u < 0f || u > 1f || v < 0f || v > 1f)
            {
                Hide();
                return;
            }

            HoverZoneInfo zone = interpreter.Interpret(new Vector2(u, v), visualState);
            outlineView.Show(markerRect, localPoint, zone);
            tooltipView.Show(overlayRoot, ToOverlayLocalPoint(localPoint), zone);
        }

        private Vector2 ToOverlayLocalPoint(Vector2 markerLocalPoint)
        {
            if (overlayRoot == markerRect)
            {
                return markerLocalPoint;
            }

            Vector3 worldPoint = markerRect.TransformPoint(markerLocalPoint);
            return overlayRoot.InverseTransformPoint(worldPoint);
        }

        private void Hide()
        {
            outlineView.Hide();
            tooltipView.Hide();
        }
    }
}
