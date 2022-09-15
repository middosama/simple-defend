using System.Collections;
using UnityEngine;
using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;

public class LevelRecordList : MonoBehaviour, IEnhancedScrollerDelegate
{
    List<LevelRecord> records;
    [SerializeField]
    LevelRecordItem levelRecordItemTemplate;
    [SerializeField]
    float cellSize = 40;
    public float cellSpacing = 10f;
    public int cellPadding = 40;
    [SerializeField]
    EnhancedScroller scroller;
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cell = scroller.GetCellView(levelRecordItemTemplate);
        var record = records[cellIndex];
        cell.Init(record);
        return cell;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return cellSize;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return records.Count;
    }

    public void Load(List<LevelRecord> data)
    {
        records = data;
        //if (data == null)
        //{
        //    records = new List<LevelRecord>();
        //}

        scroller.spacing = cellSpacing;
        scroller.padding.top = cellPadding;
        scroller.padding.left = cellPadding;
        scroller.padding.right = cellPadding;
        scroller.ReloadData();
    }

    private void Start()
    {
        scroller.Delegate = this;
    }

}