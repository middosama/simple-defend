using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ResponsiveGridLayout : LayoutGroup, ILayoutSelfController
{
    [SerializeField]
    float cellRatio = 1f;
    [SerializeField]
    float hSpacing, vSpacing;
    float hS, vS;
    [SerializeField]
    bool isRelative = false;
    [SerializeField]
    bool autoFitHeight = true;
    [SerializeField]
    int columns = 1;

    public override void CalculateLayoutInputVertical()
    {
        if (columns == 0)
            return;
        base.CalculateLayoutInputHorizontal();
        float pWidth = rectTransform.rect.width;

        if (isRelative)
        {
            hS = hSpacing * pWidth / ((float)columns - 1);
            vS = vSpacing * pWidth / ((float)columns - 1);
        }
        else
        {
            hS = hSpacing;
            vS = vSpacing;
        }

        float cellWidth = (pWidth - hS * (columns - 1)) / (float)columns;
        float cellHeight = cellWidth * cellRatio;
        float childPosX = 0, childPosY = 0;
        int colIndex, rowIndex;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            colIndex = i % columns;
            rowIndex = i / columns;

            var childRect = rectChildren[i];

            childPosX = (cellWidth + hS) * colIndex;
            childPosY = (cellHeight + vS) * rowIndex;

            SetChildAlongAxis(childRect, 0, childPosX, cellWidth);
            SetChildAlongAxis(childRect, 1, childPosY, cellHeight);
        }
        if (autoFitHeight)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, childPosY + cellHeight);
            m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);
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
