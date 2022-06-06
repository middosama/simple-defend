using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class WorldClickEvent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent onClick;
    public UnityEvent onEnter;
    public UnityEvent onExit;
    public TouchPhase phase;
    public SpriteRenderer targetGraphic;

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        phase = TouchPhase.Began;
        onEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        phase = TouchPhase.Ended;
        onExit?.Invoke();
    }

}
