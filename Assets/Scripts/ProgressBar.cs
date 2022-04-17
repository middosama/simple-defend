using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityCommon;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer mainLine;

    public void SetPercent(float percent)
    {
        mainLine.size = new Vector2(percent, mainLine.size.y);
    }

}
