using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Level;
using DG.Tweening;

public class CheckpointItem : MonoBehaviour
{
    [SerializeField]
    TMP_Text txtWave, txtHealth, txtCoin;
    [SerializeField]
    Image screenshot;
    Checkpoint checkpoint;
    //static Tweener scaleAnim;

    // Use this for initialization
    internal void Init(Checkpoint checkpoint, bool isNew)
    {
        txtWave.text = (checkpoint.waveIndex+1).ToString();
        txtHealth.text = checkpoint.hp.ToString();
        txtCoin.text = checkpoint.coin.ToString();
        screenshot.sprite = checkpoint.screenshot;
        this.checkpoint = checkpoint;
        if (isNew)
        {
            transform.localScale = Vector3.zero;
            //if (scaleAnim == null)
            //{
                transform.DOScale(1, 0.2f).SetUpdate(true);
            //}
            //else
            //{
            //    scaleAnim.SetTarget(transform).Restart();
            //}
        }
    }

    public void Load()
    {
        LevelGUI.Instance.ShowCheckpointConfirmPopup(checkpoint, "Lang.load", OnLoad);
    }

    public void Remove()
    {
        LevelGUI.Instance.ShowCheckpointConfirmPopup(checkpoint, "Lang.load", OnRemove);
    }

    void OnLoad()
    {
        Level.Instance.LoadCheckpoint(checkpoint);
    }
    void OnRemove()
    {
        Level.Instance.RemoveCheckpoint(checkpoint);
        LevelGUI.Instance.RedrawCheckpointListInfo();
        //transform.DOScale(0, 0.2f).SetUpdate(true);
        Destroy(this.gameObject);
    }
}
