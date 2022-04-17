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
    [SerializeField]
    float smooth = 0.1f;

    Vector2 nextPos;
    Tweener smootingTween;
    public void OnBeginDrag(PointerEventData eventData)
    {
        nextPos = rectTransform.anchoredPosition;
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 change = eventData.delta;
        if (lockX)
        {
            change.x = 0;
        }
        else if(lockY)
        {
            change.y = 0;
        }
        nextPos += change;
        if(smootingTween.IsActive())
            smootingTween.Kill();
        smootingTween = rectTransform.DOAnchorPos(nextPos, smooth);
    }

    // Use this for initialization
    
}
