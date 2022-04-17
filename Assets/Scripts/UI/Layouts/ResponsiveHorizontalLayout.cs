using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ResponsiveHorizontalLayout : LayoutGroup, ILayoutSelfController
{
    [SerializeField]
    float cellRatio = 1;
    [SerializeField]
    float spacing;
    float hS;
    [SerializeField]
    bool isRelative = false;
    [SerializeField]
    bool autoFitWidth = true;
    

    private DrivenRectTransformTracker m_Tracker;
    public override void CalculateLayoutInputVertical()
    {
        if (rectChildren.Count == 0 || cellRatio <= 0)
            return;
        base.CalculateLayoutInputHorizontal();
        float pHeight = rectTransform.rect.height;

        float cellWidth = pHeight / cellRatio;
        if (isRelative)
        {
            hS = spacing * cellWidth;
        }
        else
        {
            hS = spacing;
        }

        float childPosX = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {

            var childRect = rectChildren[i];

            childPosX = (cellWidth + hS) * i;

            SetChildAlongAxis(childRect, 0, childPosX, cellWidth);
            SetChildAlongAxis(childRect, 1, 0, pHeight);
        }
        if (autoFitWidth)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, childPosX + cellWidth);
            m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);
        }
        else
        {
            m_Tracker.Clear();
        }
    }

    public override void SetLayoutHorizontal()
    { }

    public override void SetLayoutVertical()
    { }

    // Use this for initialization
}
