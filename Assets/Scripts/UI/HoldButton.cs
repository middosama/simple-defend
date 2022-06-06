using System.Collections;
using UnityCommon;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onHoldDone;
    public UnityEvent<float> onHolding;
    public Image targetGraphic;
    public float holdTime = 1;

    bool holding = false;
    float startTime;
    float currentPercent;
    Coroutine rollback;

    // Update is called once per frame
    private void OnEnable()
    {
        OnStep(0);
    }
    void Update()
    {
        if (!holding)
            return;
        float holdedTime = Time.unscaledTime - startTime;
        if (Input.GetMouseButtonUp(0))
        {
            OnStop(holdedTime);
            return;
        }
        if (holdedTime > holdTime)
        {
            onHoldDone?.Invoke();
            holding = false;
            return;
        }
        OnStep(holdedTime / holdTime);


    }

    void OnStep(float percent)
    {
        currentPercent = percent;
        onHolding?.Invoke(percent);
        if (targetGraphic != null)
            targetGraphic.fillAmount = percent;
    }
    public void OnStop(float holdedTime, float speed = 2)
    {

        rollback = this.DoCount(holdedTime, 0, holdedTime / holdTime / speed, OnStep, true, DG.Tweening.Ease.Linear);
        holding = false;
    }
    public void OnStopAbsolute(float speed)
    {
        OnStop(1, speed);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        holding = true;
        if (rollback != null)
        {
            startTime = Time.unscaledTime - holdTime * currentPercent;
            StopCoroutine(rollback);
        }
        else
        {
            startTime = Time.unscaledTime;
        }
    }
}
