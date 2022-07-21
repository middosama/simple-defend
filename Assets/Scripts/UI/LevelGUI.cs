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
    CanvasGroup canvasGroup;
    [SerializeField]
    CanvasGroup flashCanvas;
    public Canvas canvas;


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
    public TMP_Text txtCheckpointTicketCount;


    public TMP_Text txtCheckpointConfirmTitle, txtCheckpointConfirmHealth, txtCheckpointConfirmWave, txtCheckpointConfirmCoin;
    public Image checkpointConfirmScreenshot;
    public Action onConfirm;
    //public Image checkpointConfirmImage;

    public PopupEffect winPanel, confirmWinPopup, checkpointConfirmPopup, checkpointPanel;
    public PopupGroup popupGroup;

    public TMP_Text txtDiffReward, txtStarReward;

    [SerializeField]
    Color32 red;
    [SerializeField]
    Color32 green;

    List<Checkpoint> checkpointList; // share with Level

    const int checkpointSlot = 6;

    internal void Init(List<Checkpoint> checkpoints)
    {
        checkpointList = checkpoints;
        Instance = this;
        popupGroup.onFirstPopupShow.AddListener(x => Level.Instance.PassivePause());
        popupGroup.onLastPopupHide.AddListener(x => Level.Instance.PassiveResume());
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
        if (checkpointList.Count >= 9)
        {
            return;
        }
        // check have ticket
        if (Player.CheckpointTicket > 0)
            StartCoroutine(IcreateCheckpoint());
    }

    public IEnumerator IcreateCheckpoint()
    {
        canvasGroup.alpha = 0.001f;
        flashCanvas.gameObject.SetActive(true);
        flashCanvas.alpha = 0;
        Checkpoint? newCheckpoint;
        yield return new WaitForEndOfFrame();
        newCheckpoint = Level.Instance.CreateCheckpoint();
        var fadeAnim = flashCanvas.DOFade(1, transitionDuration / 3)
            .SetUpdate(true)
            .SetAutoKill(false);
        fadeAnim.OnComplete(() =>
            {
                if(newCheckpoint!= null)
                    InitCheckpoint(checkpointContainer, newCheckpoint.Value, true);
                RedrawCheckpointListInfo();
                canvasGroup.alpha = 1;
                fadeAnim.OnComplete(() =>
                {
                    flashCanvas.gameObject.SetActive(false);
                    fadeAnim.Kill();
                });
                fadeAnim.PlayBackwards();
            });






    }

    public void ShowWinConfirmPopup()
    {
        confirmWinPopup.Show();
        InstantiateCheckPointList(winCheckpointContainer);
    }

    public void ShowWinPanel(int diffPoint, int starPoint)
    {
        txtDiffReward.text = diffPoint.ToString();
        txtStarReward.text = starPoint.ToString();
        winPanel.Show();
    }

    public void ShowCheckpointListPopup()
    {
        RedrawCheckpointListInfo();
        checkpointPanel.Show();
        InstantiateCheckPointList(checkpointContainer);

        //checkPointList.DOPivotY()
    }
    public void CloseCheckpointListPopup()
    {
        checkpointPanel.Close();
    }

    public void RedrawCheckpointListInfo()
    {
        txtCheckpointCount.text = checkpointList.Count + "/" + checkpointSlot;
        txtCheckpointCount.color = checkpointList.Count >= checkpointSlot ? red : green;
        txtCheckpointTicketCount.text = Player.CheckpointTicket.ToString();
    }

    public void ShowCheckpointConfirmPopup(Checkpoint checkpoint, string title, Action onConfirm)
    {
        txtCheckpointConfirmTitle.text = title;
        txtCheckpointConfirmCoin.text = checkpoint.coin.ToString();
        txtCheckpointConfirmHealth.text = checkpoint.hp.ToString();
        txtCheckpointConfirmWave.text = (checkpoint.waveIndex + 1).ToString();
        this.onConfirm = onConfirm;
        checkpointConfirmPopup.Show();
    }

    public void CheckpointConfirm()
    {
        onConfirm();
        CheckpointConfirmPopupHide();
    }

    public void CheckpointConfirmPopupHide()
    {
        checkpointConfirmPopup.Close();
        // add transition later
    }

    public void ConfirmWin()
    {
        confirmWinPopup.Close();
        Level.Instance.OnWinConfirmed();

    }



    void InstantiateCheckPointList(HorizontalChaosLayout container)
    {
        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }
        checkpointList.ForEach(x =>
        {
            InitCheckpoint(container, x);
        });
        container.seed = UnityEngine.Random.Range(0, 100);
        container.DoCount(0, 1, transitionDuration, container.SetGlobalScale, true, Ease.OutQuad);
    }

    void InitCheckpoint(HorizontalChaosLayout container, Checkpoint checkpoint, bool isNew = false)
    {
        Instantiate(checkpointItemTemplate, container.transform).Init(checkpoint, isNew);
    }


}
