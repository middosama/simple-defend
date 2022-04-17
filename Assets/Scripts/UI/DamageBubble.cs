using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DamageBubble : DynamicObjectPool<DamageBubble>
{
    static readonly Color physXColor = new Color(1, 0.192f, 0);
    static readonly Vector2 bigSize = new Vector2(1.5f, 1.5f);
    static readonly float showTime = 0.7f;

    [SerializeField]
    TMP_Text damageNumber;
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    CanvasGroup canvasGroup;

    Transform container;

    Sequence scaleAnim;

    private void Awake()
    {
        //scaleAnim = DOTween.Sequence()
        //    .OnStart(() =>
        //    {
        //        rectTransform.sizeDelta = Vector2.one;
        //        rectTransform.anchoredPosition = Vector2.zero;
        //        //damageNumber.DOColor(new Color(0, 0, 0, 0), showTime);
        //    })
        //    .Append(rectTransform.DOSizeDelta(bigSize, showTime ).SetLoops(1, LoopType.Yoyo))
        //    //.Join(damageNumber.DOBlendableColor(new Color(0,0,0,0), showTime).SetLoops(1, LoopType.Yoyo).SetRelative())
        //    .OnComplete(() =>
        //    {
        //        Destroy(this);
        //    }).SetAutoKill(false).Pause();
    }

    public override void OnSpawn()
    {

    }

    // Use this for initialization
    public Sequence Init(int number, DamageType damageType)
    {
        rectTransform.anchoredPosition = Vector2.zero;
        damageNumber.text = number.ToString();
        gameObject.SetActive(true);
        switch (damageType)
        {
            case DamageType.PhysX:
                damageNumber.color = physXColor;
                //scaleAnim.Join();
                //scaleAnim.Restart();
                return rectTransform.DOJumpAnchorPos(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), 1, 1, showTime)
                    .OnComplete(() =>
                    {
                        Destroy(this);
                    });

                //break;
        }
        return null;

    }
}
