using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Level;
using UnityCommon;
using DG.Tweening;

public class LevelGUI : MonoBehaviour
{
    public static LevelGUI Instance;
    [SerializeField]
    float transitionDuration;


    [SerializeField]
    TMP_Text txtCoin, txtHealth;
    public WaveTimer waveTimer;
    [SerializeField]
    HorizontalChaosLayout checkpointContainer, winCheckpointContainer;
    [SerializeField]
    CheckpointItem checkpointItemTemplate;
    [SerializeField]
    Button btnCreateCheckpoint;
    public TMP_Text txtCheckpointCount;
    public TMP_Text txtCheckpointConfirmTitle, txtCheckpointConfirmHealth, txtCheckpointConfirmWave, txtCheckpointConfirmCoin;
    public Image checkpointConfirmScreenshot;
    public Action onConfirm;
    //public Image checkpointConfirmImage;
    public RectTransform winPanel, confirmWinPopup, checkpointConfirmPopup;

    public TMP_Text txtDiffReward, txtStarReward;

    [SerializeField]
    Color32 red;
    [SerializeField]
    Color32 green;

    List<Checkpoint> checkpointList; // share with Level

    const int checkpointSlot = 6;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCoin(int coin)
    {
        txtCoin.text = coin.ToString();
    }
    public void SetHealth(int health)
    {
        txtHealth.text = health.ToString();
    }

    public void CreateCheckpoint()
    {
        // check slot
        if (Level.Instance.GetCheckpointListCount() >= 9)
        {
            return;
        }
        if()
    }

    public void ShowWinConfirmPopup(List<Checkpoint> checkpoints)
    {
        InstantiateCheckPointList(checkpoints, winCheckpointContainer);
        confirmWinPopup.gameObject.SetActive(true);
    }

    public void ShowWinPanel(int diffPoint, int starPoint)
    {
        txtDiffReward.text = diffPoint.ToString();
        txtStarReward.text = starPoint.ToString();
        winPanel.gameObject.SetActive(true);
    }

    public void ShowCheckpointListPopup()
    {
        RedrawCheckpointListInfo();
        InstantiateCheckPointList()
        //checkPointList.DOPivotY()
    }

    void RedrawCheckpointListInfo()
    {
        int checkPointCount = Level.Instance.GetCheckpointListCount();
        txtCheckpointCount.text = checkPointCount + "/"+ checkpointSlot;
        if(checkPointCount >= checkpointSlot) // fullslot
        {
            btnCreateCheckpoint.interactable = false;
            txtCheckpointCount.color = red;
        }
        else
        {
            btnCreateCheckpoint.interactable = true;
            txtCheckpointCount.color = green;
        }
    }

    public void ShowCheckpointConfirmPopup(Checkpoint checkpoint, string title, Action onConfirm)
    {
        txtCheckpointConfirmTitle.text = title;
        txtCheckpointConfirmCoin.text = checkpoint.coin.ToString();
        txtCheckpointConfirmHealth.text = checkpoint.hp.ToString();
        txtCheckpointConfirmWave.text = (checkpoint.waveIndex + 1).ToString();
        this.onConfirm = onConfirm;
        checkpointConfirmPopup.gameObject.SetActive(true);
    }



    public void ConfirmWin()
    {
        confirmWinPopup.gameObject.SetActive(false);
        Level.Instance.OnWinConfirmed();

    }



    public void InstantiateCheckPointList(List<Checkpoint> checkpoints, HorizontalChaosLayout container)
    {
        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }
        checkpoints.ForEach(x =>
        {
            Instantiate(checkpointItemTemplate, container.transform).Init(x);
        });
        container.seed = UnityEngine.Random.Range(0, 100);
        container.DoCount(0, 1, transitionDuration, container.SetGlobalScale);
    }

    
}
