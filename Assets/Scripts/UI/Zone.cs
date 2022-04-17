using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class Zone : DraggableRect
{
    [SerializeField]
    LevelNode[] levelNodes;
    [SerializeField]
    string zoneName;
    const float duration = 0.5f;

    static ZoneData data;
    static string loadedZoneName;

    static int markAsCleared = -1;

    private void Start()
    {
        //Init();
    }

    public void Init()
    {
        if (zoneName != loadedZoneName)
            data = null;
        if (data == null)
        {
            Sync();
        };
        //Dictionary<int,int> bypassMap;
        LevelData prevLevel = null;
        if (markAsCleared != -1)
        {
            if (markAsCleared >= levelNodes.Length)
            {
                // unlock next zone
            }
            else
            {
                levelNodes[markAsCleared + 1].MarkAsNew();

            }
            markAsCleared = -1;
        }
        for (int i = 0; i < levelNodes.Length; i++)
        {
            LevelData currentLevel = data.levelDatas[i];
            bool unlocked = (prevLevel == null || prevLevel.star > 0 || prevLevel.bypass > 0); // first or passed prev level
            levelNodes[i].Init(i, currentLevel.star, unlocked);
            if (currentLevel.bypass > 0)
            {
                if (currentLevel.star > 0)
                {
                    currentLevel.bypass = 0;
                }
                else
                {
                    levelNodes[i + currentLevel.bypass + 1].LockBypass(i);
                }

            }
            prevLevel = currentLevel;
        }
    }



    void Sync()
    {
        // get data from binary
        data = DataManager.Load<ZoneData>(zoneName, DataManager.ZONE_OVERVIEW_PATH);
        loadedZoneName = zoneName;
        if (data == null)
        {
            data = new ZoneData(levelNodes.Length);
        }
        //Init();
    }


    public static void UpdateData(int levelIndex, LevelRecord record)
    {
        var levelData = data.levelDatas[levelIndex];
        if (levelData.star == 0)
            markAsCleared = levelIndex;
        levelData.star = Math.Max(levelData.star, record.star);
        DataManager.Save(loadedZoneName, DataManager.ZONE_OVERVIEW_PATH, data);
    }

    public void DoInTransition(bool isNext)
    {
        if (isNext)
        {
            rectTransform.DOAnchorPosX(0, duration).OnStart(() =>
            {
                rectTransform.anchoredPosition = new Vector2(LevelSelectController.Instance.rectTransform.sizeDelta.x, 0);
            });
        }
        else
        {
            rectTransform.DOAnchorPosX(LevelSelectController.Instance.rectTransform.sizeDelta.x - rectTransform.sizeDelta.x, duration).OnStart(() =>
            {
                rectTransform.anchoredPosition = new Vector2(-rectTransform.sizeDelta.x, 0);
            });
        }
    }
    public void DoOutTransition(bool isNext)
    {
        if (isNext)
        {
            rectTransform.DOAnchorPosX(-rectTransform.sizeDelta.x, duration);
        }
        else
        {
            rectTransform.DOAnchorPosX(LevelSelectController.Instance.rectTransform.sizeDelta.x, duration);
        }
    }
    [Serializable]
    class ZoneData
    {
        public LevelData[] levelDatas;

        public ZoneData(LevelData[] levelDatas)
        {
            this.levelDatas = levelDatas;
        }

        public ZoneData(int levelSize)
        {
            levelDatas = new LevelData[levelSize];
            for (int i = 0; i < levelSize; i++)
            {
                levelDatas[i] = new LevelData(0, 0);
            }
        }
    }
    [Serializable]
    class LevelData
    {
        public int star;
        public int bypass;

        public LevelData(int start, int bypass)
        {
            this.star = start;
            this.bypass = bypass;
        }
    }

    //[SerializeField]
    //LinkedListNode<LevelNode> x;

    //void xs()
    //{
    //    x.
    //}

}
