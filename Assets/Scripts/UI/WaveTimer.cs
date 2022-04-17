using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class WaveTimer : MonoBehaviour
{
    [SerializeField]
    RectTransform rectTransform, buttonRectTransform;
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    Button button;
    [SerializeField]
    Image leftTimer;
    [SerializeField]
    float maxTime, duration = 0.5f;

    Sequence appearSequence;
    Action waitingAction;

    bool displaying = true, displayDone;

    private void Start()
    {
        appearSequence = DOTween.Sequence()
                .Append(rectTransform.DOScale(1.1f, duration))
                .Join(canvasGroup.DOFade(1, duration))
                .Append(rectTransform.DOScale(1, duration / 4))
                .OnStart(() =>
                {
                    rectTransform.localScale = new Vector2(0.5f, 0.5f);
                    canvasGroup.alpha = 0;
                    
                })
                .SetAutoKill(false)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    displayDone = true;
                    if (waitingAction != null)
                    {
                        waitingAction();
                        waitingAction = null;
                    }
                });
    }
    public void SetTimer(float leftTime)
    {
        if (leftTime <= maxTime)
        {
            if (leftTime < 0)
            {
                return;
            }
            else
            {
                leftTimer.fillAmount = leftTime / maxTime;
            }
            Appear();
        }
        else
        {
            return;
        }
    }

    public void BurnTime(float leftTime)
    {
        if (leftTime < 0)
            return;
        if (leftTime > maxTime)
            leftTime = maxTime;
        SetTimer(leftTime);
        if (displayDone)
        {
            DoBurnTimer(leftTime);
        }
        else
        {
            waitingAction = () =>
            {
                DoBurnTimer(leftTime);
            };
        }
    }

    void DoBurnTimer(float leftTime)
    {
        leftTimer.DOFillAmount(0, duration * (leftTime / maxTime)).OnComplete(Active);
    }

    void Active()
    {
        buttonRectTransform.DOLocalRotate(Vector3.zero, duration).SetUpdate(true);
    }
    void InActive()
    {
        buttonRectTransform.DOLocalRotate(new Vector2(-90,0), duration).SetUpdate(true);
    }

    public void Appear()
    {
        if (displaying)
            return;
        Debug.Log(displaying);
        displaying = true;
        gameObject.SetActive(true);
        appearSequence.PlayForward();

    }

    public void Disappear()
    {
        if (!displaying)
            return;
        appearSequence.PlayBackwards();
        InActive();
        displaying = false;
        displayDone = false;
    }
}
