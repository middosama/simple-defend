using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class PopupEffect : MonoBehaviour
{
    [SerializeField]
    RectTransform targetPanel;
    [SerializeField]
    CanvasGroup background;
    [SerializeField]
    PopupGroup popupGroup;
    // Use this for initialization
    Action onDone;


    Sequence sequence;

    const float scale1 = 1.1f;
    const float duration = 0.3f;

    static Vector2 half = new Vector2(0.5f, 0.5f);

    private void Start()
    {
        popupGroup?.Assign(this);

    }

    public void Show()
    {
        gameObject.SetActive(true);
        if (sequence == null)
        {
            sequence = DOTween.Sequence();

            background.alpha = 0;
            targetPanel.localScale = Vector2.zero;
            sequence
                .Append(targetPanel.DOScale(scale1, duration))
                .Join(background.DOFade(1, duration))
                .Append(targetPanel.DOScale(1, duration / 4))
                .SetAutoKill(false)
                .SetUpdate(true)
                .OnStepComplete(() =>
                {
                    onDone?.Invoke();
                });
        }
        else
        {
            sequence.PlayForward();
        }
        popupGroup?.OnShow(this);
    }
    //public void Show()
    //{
    //    gameObject.SetActive(true);
    //}

    public void Close()
    {
        if (sequence != null)
        {
            onDone = () => { gameObject.SetActive(false); onDone = null; };
            sequence.PlayBackwards();
        }
        popupGroup?.OnHide(this);
    }
}
