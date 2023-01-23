using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private RectTransform rect;
    private Image sprite;

    private void Awake()
    {
        sprite = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        sprite.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        sprite.raycastTarget = true;
    }
}