using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DraggableRect : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField]
    bool lockX, lockY;
    [SerializeField]
    protected RectTransform rectTransform;
    RectTransform parentRectTransform;
    [SerializeField]
    float smooth = 0.1f;

    Vector2 nextPos;
    Tweener smootingTween;
    public void OnBeginDrag(PointerEventData eventData)
    {
        nextPos = rectTransform.anchoredPosition;
        //throw new System.NotImplementedException();
    }

    public void Start()
    {
        parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 change = eventData.delta;
        if (lockX)
        {
            change.x = 0;
        }
        else if (lockY)
        {
            change.y = 0;
        }
        nextPos += change;
        //
        if (nextPos.x < 0f && nextPos.x > (parentRectTransform.rect.width - rectTransform.sizeDelta.x))
        {
            if (smootingTween.IsActive())
                smootingTween.Kill();
            smootingTween = rectTransform.DOAnchorPos(nextPos, smooth);
        }

        // limit
    }

    // Use this for initialization

}
