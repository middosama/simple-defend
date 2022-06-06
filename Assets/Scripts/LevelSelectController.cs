using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class LevelSelectController : MonoBehaviour
{
    public static LevelSelectController Instance;


    [SerializeField]
    Zone[] zones;
    [SerializeField]
    TMP_Text levelName, levelDes, levelNote, levelBypass;
    [SerializeField]
    Image levelPreview;
    [SerializeField]
    Button startButton, btnBypass;

    public RectTransform rectTransform, zoneContainer, loadingCloud;
    public PopupEffect levelSelectPopup;
    public LevelRecordList levelRecordList;

    Zone currentZone;

    static int zoneIndex;
    static int selectingLevelIndex;
    static List<LevelRecord> selectingLevelRecords;

    const string LAST_ZONE_INDEX_KEY = "LAST_ZONE_INDEX";
    const string LAST_LEVEL_INDEX_KEY = "LAST_LEVEL_INDEX";

    const float duration = 0.5f;

    private void Start()
    {
        Instance = this;
        zoneIndex = PlayerPrefs.GetInt(LAST_ZONE_INDEX_KEY, 0);
        currentZone = Instantiate(zones[zoneIndex], zoneContainer);
        currentZone.Init();
        GamePlayController.SelectedLevel = null;
        Main.EndLoading();
        //loadingCloud.DOAnchorPosX(rectTransform.sizeDelta.x,duration);
        loadingCloud.anchoredPosition = new Vector2(rectTransform.sizeDelta.x, 0);
    }

    public void NextZone()
    {
        if (zoneIndex + 1 < zones.Length)
        {
            LoadZone(zoneIndex + 1);
        }
    }

    public void PrevZone()
    {
        if (zoneIndex > 0)
        {
            LoadZone(zoneIndex - 1);
        }
    }

    void LoadZone(int nextZone)
    {
        DoZoneTrasition(nextZone > zoneIndex, () =>
         {
             zoneIndex = nextZone;
             Destroy(currentZone);
             currentZone = Instantiate(zones[zoneIndex], zoneContainer);
             currentZone.Init();
         });

    }

    void DoZoneTrasition(bool isNext, Action action)
    {
        int direction = isNext ? 1 : -1;
        Debug.Log(direction);
        currentZone.DoOutTransition(isNext);
        loadingCloud.anchoredPosition = new Vector2(rectTransform.sizeDelta.x * direction, 0);
        loadingCloud.DOAnchorPosX(0, duration * 0.75f).OnComplete(() =>
        {
            action();
            loadingCloud.DOAnchorPosX(rectTransform.sizeDelta.x * -direction, duration * 1.5f);
            Debug.Log(rectTransform.sizeDelta.x * -direction);
            currentZone.DoInTransition(isNext);

        });
    }

    public bool IsCurrentZoneUnlocked()
    {
        return zoneIndex == 0 ? true : zones[zoneIndex - 1].IsComplete();
    }

    public void ShowLevelPopup(int levelIndex, Level level, bool isUnlocked, List<int> waitBypassList)
    {
        selectingLevelIndex = levelIndex;
        GamePlayController.SelectedLevel = level;

        levelPreview.sprite = level.levelPreview;
        levelName.text = zoneIndex + "-" + levelIndex;


        selectingLevelRecords = DataManager.Load<List<LevelRecord>>(level.name, DataManager.LEVEL_RECORDS_PATH);
        if (selectingLevelRecords == null)
            selectingLevelRecords = new List<LevelRecord>();

        LoadRecordList();
        startButton.interactable = isUnlocked;

        btnBypass.gameObject.SetActive(true);

        if (selectingLevelRecords.Count > 0)
        {
            levelNote.text = "";
            btnBypass.gameObject.SetActive(false);
        }
        else
        {
            GenLevelNote(levelIndex, isUnlocked, waitBypassList);
        }

        levelSelectPopup.Show();


        //PlayerPrefs.SetInt(LAST_LEVEL_INDEX_KEY, levelIndex);
        // enable on have language
        //levelDes.text = Language.LevelDescription[zoneIndex][levelIndex];
    }

    void GenLevelNote(int levelIndex, bool isUnlocked, List<int> waitBypassList)
    {
        int nextJump = currentZone.GetBypassJump(levelIndex);
        if (isUnlocked)
        {
            levelNote.text = "{lang.needToSolve before}" + (nextJump > 0 ? nextJump + "{lang.next}" : "{lang.nextzone}");
            if (nextJump == 0)
            {
                levelNote.text = "";
                btnBypass.gameObject.SetActive(false);
            }
            return;
        }

        string listBypassing = "";
        if (waitBypassList == null) return;
        //if (waitBypassList != null)
        //{
        for (int i = 0; i < waitBypassList.Count; i++)
        {
            if (i == waitBypassList.Count - 1 && waitBypassList.Count > 1)
            {
                listBypassing += GetLevelName(waitBypassList[i]);
            }
            else
            {
                listBypassing += GetLevelName(waitBypassList[i])+ ", ";
            }
        }
        //}
        levelNote.text = Language.Other["needToSolveBeforeNextZone"] + listBypassing;

    }

    string GetLevelName(int levelIndex)
    {
        return zoneIndex + "-" + levelIndex;
    }

    /// <returns>Total diff</returns>
    public static int SaveRecord(LevelRecord newRecord)
    {
        int diff = 0;
        if (selectingLevelRecords.Count != 0)
        {
            diff = selectingLevelRecords.Min((x) => newRecord.allies.Except(x.allies).Count()); // find most similar record
        }
        Zone.UpdateData(selectingLevelIndex, newRecord);

        selectingLevelRecords.Add(newRecord);
        DataManager.Save(GamePlayController.SelectedLevel.name, DataManager.LEVEL_RECORDS_PATH, selectingLevelRecords);
        return diff;
    }

    void LoadRecordList()
    {

        levelRecordList.Load(selectingLevelRecords);
    }


    public void StartGame()
    {
        if (GamePlayController.SelectedLevel != null)
            Main.LoadScene("GameplayScene");
    }

    public void Bypass()
    {
        if (Player.Bypass())
            currentZone.Bypass(selectingLevelIndex);
    }
}

[Serializable]
public struct LevelRecord
{
    public List<AllyCount> allies;
    public int star;
}
[Serializable]
public class AllyCount
{
    public AllyName allyName;
    public int count = 1;

    public AllyCount(AllyName allyName)
    {
        this.allyName = allyName;
    }
}

