using System.Collections;
using UnityEngine;


public class ComfortPopupPosition : MonoBehaviour
{

    public RectTransform selfRect;
    public float xOffset;
    public float paddingLeft, paddingRight, paddingTop,paddingBottom;
    public bool isRelative;
    float leftPivot, rightPivot;
    // Use this for initialization
    private void Awake()
    {
        if (isRelative)
        {
            paddingLeft *= Screen.safeArea.width;
            paddingRight *= Screen.safeArea.width;
            paddingTop *= Screen.safeArea.height;
            paddingBottom *= Screen.safeArea.height;
        }
    }
    void OnEnable()
    {
        leftPivot = 1 + xOffset;
        rightPivot = -xOffset;
        FormatPosition();
    }

    public void FormatPosition()
    {
        float xPivot;
        Vector2 position = selfRect.anchoredPosition;
        float xSpace = selfRect.sizeDelta.x * (1 + xOffset);
        float ySpace = selfRect.sizeDelta.y/2;
        if (position.x < xSpace + paddingLeft)
        {
            xPivot = rightPivot;
        }
        else if (position.x > Screen.safeArea.width - xSpace - paddingRight)
        {
            xPivot = leftPivot;
        }
        else
        {
            xPivot = position.x > Screen.safeArea.width / 2 ? rightPivot : leftPivot;
        }
        selfRect.anchoredPosition = new Vector2(position.x, Mathf.Clamp(position.y + paddingBottom, ySpace, Screen.safeArea.height - ySpace - paddingTop));
        selfRect.pivot = new Vector2(xPivot, selfRect.pivot.y);
    }
}
