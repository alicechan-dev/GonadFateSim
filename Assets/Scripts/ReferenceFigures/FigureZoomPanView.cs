using UnityEngine;
using UnityEngine.EventSystems;

namespace GonadFateSim.ReferenceFigures
{
    public sealed class FigureZoomPanView : MonoBehaviour, IScrollHandler, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private float minScale = 0.6f;
        [SerializeField] private float maxScale = 4f;
        [SerializeField] private float zoomStep = 0.12f;

        private Vector2 dragStartPosition;
        private Vector2 contentStartPosition;

        public void Bind(RectTransform target)
        {
            content = target;
            ResetView();
        }

        public void ResetView()
        {
            if (content == null)
            {
                return;
            }

            content.localScale = Vector3.one;
            content.anchoredPosition = Vector2.zero;
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (content == null)
            {
                return;
            }

            float scale = Mathf.Clamp(content.localScale.x + eventData.scrollDelta.y * zoomStep, minScale, maxScale);
            content.localScale = Vector3.one * scale;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (content == null)
            {
                return;
            }

            dragStartPosition = eventData.position;
            contentStartPosition = content.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (content == null)
            {
                return;
            }

            content.anchoredPosition = contentStartPosition + (eventData.position - dragStartPosition);
        }
    }
}
