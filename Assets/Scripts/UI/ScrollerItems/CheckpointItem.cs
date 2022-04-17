using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Level;

public class CheckpointItem : MonoBehaviour
{
    [SerializeField]
    TMP_Text txtWave, txtHealth, txtCoin;
    [SerializeField]
    Image screenshot;
    Checkpoint checkpoint;
    // Use this for initialization
    internal void Init(Checkpoint checkpoint)
    {
        txtWave.text = (checkpoint.waveIndex+1).ToString();
        txtHealth.text = checkpoint.hp.ToString();
        txtCoin.text = checkpoint.coin.ToString();
        screenshot.sprite = checkpoint.screenshot;
        this.checkpoint = checkpoint;

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
        Destroy(this.gameObject);
    }
}
