using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class HorizontalChaosLayout : LayoutGroup, ILayoutSelfController
{
    [SerializeField]
    float cellRatio = 1;
    //[SerializeField]
    //bool useChildScale = false;
    [HideInInspector]
    public int collumnCount, rowCount;
    [HideInInspector]
    public bool autoFitRow;
    [HideInInspector]
    public float vSpacing;

    public int seed;
    public float rotateChaosScale = 0.3f;
    public float posXChaosScale = 0;
    public float posYChaosScale = 0.1f;
    public float globalScale = 1;

    public bool isMultipleRow;



    float cellWidth,cellHeight;
    float vSpace = 0, vSpacingAbsolute = 0;
    public override void CalculateLayoutInputVertical()
    {
        if (rectChildren.Count == 0 || cellRatio <= 0)
            return;
        base.CalculateLayoutInputHorizontal();
        float pHeight = rectTransform.rect.height;
        cellHeight = pHeight;

        if (isMultipleRow)
        {
            if (autoFitRow)
            {
                rowCount = (rectChildren.Count+ collumnCount-1) / collumnCount;
            }
            vSpacingAbsolute = pHeight * vSpacing;
            cellHeight = (pHeight - (vSpacingAbsolute* (rowCount -1))) / rowCount ;
            vSpace = rowCount > 1 ? ((pHeight - cellHeight) / (rowCount - 1)) : pHeight;
            //cellHeight = vSpace;
        }
        cellWidth = cellHeight / cellRatio;

        float childPosX = 0, childPosY = 0;
        float offsetX, offsetY, offsetRotate, cellSpace;
        if (rectChildren.Count == 1 || (isMultipleRow && collumnCount == 1))
        {
            cellSpace = rectTransform.rect.width;
        }
        else
        {
            cellSpace = (rectTransform.rect.width - cellWidth) / ((isMultipleRow ? collumnCount : rectChildren.Count) - 1);
        }



        for (int i = 0; i < rectChildren.Count; i++)
        {
            Random.InitState(seed + i);
            offsetX = Random.Range(-1f, 1f);
            Random.InitState(seed * i);
            offsetY = Random.Range(-1f, 1f);
            offsetRotate = offsetX + offsetY;

            var childRect = rectChildren[i];

            if (isMultipleRow)
            {
                childPosX = cellSpace * (i % collumnCount);
                childPosY = (vSpace ) * (i / collumnCount);
            }
            else
            {
                childPosX = cellSpace * i;

            }

            SetChildAlongAxis(childRect, 0, (childPosX + (offsetX * posXChaosScale)) * globalScale, cellWidth);
            SetChildAlongAxis(childRect, 1, ((childPosY + offsetY * posYChaosScale) * globalScale), cellHeight);
            if (rotateChaosScale != 0)
                childRect.localRotation = Quaternion.Euler(0, 0, offsetRotate * rotateChaosScale * globalScale);
        }

    }

    public void SetGlobalScale(float time)
    {
        globalScale = time;
    }

    public override void SetLayoutHorizontal()
    { }

    public override void SetLayoutVertical()
    { }

    // Use this for initialization
}

[CustomEditor(typeof(HorizontalChaosLayout))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var layout = target as HorizontalChaosLayout;


        if (layout.isMultipleRow)
        {
            layout.collumnCount = EditorGUILayout.IntField("Collumn", layout.collumnCount);
            layout.autoFitRow = EditorGUILayout.Toggle("Auto Fit Row", layout.autoFitRow);
            layout.vSpacing = EditorGUILayout.FloatField("V Spacing", layout.vSpacing);
            if (!layout.autoFitRow)
            {
                layout.rowCount = EditorGUILayout.IntField("Row", layout.rowCount);
            }
        }

    }
}
