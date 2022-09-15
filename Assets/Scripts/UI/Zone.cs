using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;
using UnityEngine.UI;

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


    [SerializeField]
    SubNodethingy SubNodeTemplate;
    [SerializeField]
    Transform SubNodeContainer;
    public float wantedDistance = 120f;
    public int GetNodeLength()
    {
        return levelNodes.Length;
    }

    public int GetBypassJump(int levelIndex)
    {
        int nextJump = data.levelDatas[levelIndex].bypass + levelIndex + 1;
        return nextJump >= levelNodes.Length ? data.levelDatas[levelIndex].bypass == 0 ? -1 : 0 : nextJump; // -1 is next zone, 0 is done
    }

    public void Init()
    {


        if (zoneName != loadedZoneName)
            data = null;
        if (data == null)
        {
            Sync();
        };
        LevelData prevLevel = null;
        if (markAsCleared != -1)
        {
            if (markAsCleared + 1 >= levelNodes.Length)
            {
                // unlock next zone
            }
            else
            {
                levelNodes[markAsCleared + 1].MarkAsNew();

            }
            markAsCleared = -1;
        }

        bool unlockFisrt = LevelSelectController.Instance.IsCurrentZoneUnlocked();

        for (int i = 0; i < levelNodes.Length; i++)
        {
            LevelData currentLevel = data.levelDatas[i];
            bool unlocked;
            if (prevLevel == null)// first or passed prev level
            {
                unlocked = unlockFisrt;
            }
            else
            {
                unlocked = prevLevel.star > 0 || prevLevel.bypass > 0;
            }


            levelNodes[i].Init(i, currentLevel.star, unlocked, currentLevel.bypass);
            if (!unlocked && i > 0)
            {
                levelNodes[i].LockByBypass(i - 1);
            }
            if (currentLevel.bypass > 0)
            {
                if (currentLevel.star > 0)
                {
                    currentLevel.bypass = 0;
                }
                else
                {
                    if (i + currentLevel.bypass + 1 < levelNodes.Length)
                        levelNodes[i + currentLevel.bypass + 1].LockByBypass(i);
                }

            }
            prevLevel = currentLevel;
        }

        for (int i = 0; i < levelNodes.Length - 1; i++)
        {
            //x.anchoredPosition = new Vector2()
            var pointA = levelNodes[i].selfRectTransform.anchoredPosition;
            var pointB = levelNodes[i + 1].selfRectTransform.anchoredPosition;
            //if (levelNodes[i].IsUnlocked )
            
            if (levelNodes[i].IsUnlocked && levelNodes[i + 1].IsUnlocked)
            {
                VisualizeSubNode(pointA, pointB, true);
            }
            else
            {
                VisualizeSubNode(pointA, pointB, false);
            }
        }
    }

    public void VisualizeSubNode(Vector2 a, Vector2 b, bool status)
    {
        int subNodeNum;
        double distance = (a - b).magnitude;
        //Debug.Log(Math.Round(distance / wantedDistance));
        subNodeNum = (int)Math.Round(distance / wantedDistance);
        for (int j = 1; j <= subNodeNum; j++)
        {
            var tempNode = new Vector2();
            tempNode = b * j + a * (subNodeNum + 1 - j);
            tempNode /= (subNodeNum + 1);
            //tempNode.x = (b.x * j + a.x * ) / ;
            //tempNode.y = (b.y * j + a.y * (subNodeNum + 1 - j)) / ;
            var clone = Instantiate(SubNodeTemplate, SubNodeContainer);
            clone.Init(tempNode, status);
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
        else if (data.levelDatas.Length < levelNodes.Length)
        {
            var newData = new LevelData[levelNodes.Length];
            for (int i = 0; i < levelNodes.Length; i++)
            {
                if (i < data.levelDatas.Length)
                {
                    newData[i] = data.levelDatas[i];
                }
                else
                {
                    newData[i] = new LevelData();
                }
            }
            data.levelDatas = newData;
        }
        //Init();
    }

    public bool IsComplete()
    {
        ZoneData zoneData = DataManager.Load<ZoneData>(zoneName, DataManager.ZONE_OVERVIEW_PATH);
        if (zoneData == null) return false;
        var tempData = zoneData.levelDatas[zoneData.levelDatas.Length - 1];
        return tempData.star > 0 || tempData.bypass > 0; // last level bypass or pass
    }

    public void Bypass(int levelIndex)
    {
        //levelNodes[levelIndex].AddBypass();

        // add
        int currentBypass = ++data.levelDatas[levelIndex].bypass;
        Save();
        if (levelIndex == levelNodes.Length - 1) // last level, unlock next zone, 
        {
            //todo
            return;
        }



        // unlock next level and lock next 2nd level
        if (levelIndex + currentBypass >= levelNodes.Length)
            return;
        levelNodes[levelIndex + currentBypass].UnlockByBypass(levelIndex);
        if (levelIndex + currentBypass + 1 >= levelNodes.Length)
            return;
        levelNodes[levelIndex + currentBypass + 1].LockByBypass(levelIndex);

    }

    public static void UpdateData(int levelIndex, LevelRecord record)
    {
        var levelData = data.levelDatas[levelIndex];
        if (levelData.star == 0)
            markAsCleared = levelIndex;
        levelData.star = Math.Max(levelData.star, record.star);
        Save();
    }

    static void Save()
    {
        DataManager.Save(loadedZoneName, DataManager.ZONE_OVERVIEW_PATH, data);
    }

    public void DoInTransition(bool isNext, TweenCallback onTransitionDone)
    {
        if (isNext)
        {
            rectTransform.anchoredPosition = new Vector2(LevelSelectController.Instance.rectTransform.sizeDelta.x, 0);
            rectTransform.DOAnchorPosX(0, duration).OnComplete(onTransitionDone);
        }
        else
        {
            rectTransform.anchoredPosition = new Vector2(-rectTransform.sizeDelta.x, 0);
            rectTransform.DOAnchorPosX(LevelSelectController.Instance.rectTransform.sizeDelta.x - rectTransform.sizeDelta.x, duration).OnComplete(onTransitionDone);
                
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
                levelDatas[i] = new LevelData();
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
        public LevelData() : this(0, 0)
        {
        }
    }

    //[SerializeField]
    //LinkedListNode<LevelNode> x;

    //void xs()
    //{
    //    x.
    //}

}
