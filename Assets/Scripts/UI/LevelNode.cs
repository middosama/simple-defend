using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour
{
    [SerializeField]
    Level level;
    public int subNodeNumber = 3;

    public RectTransform selfRectTransform;
    //LinkedListNode<LevelNode> currentNode;
    int levelIndex;

    [SerializeField]
    TMP_Text txtStar; // fix later
    [SerializeField]
    Button btn;

    List<int> waitingBypassingLevel;
    bool isUnlocked;
    bool isNew = false;

    public bool IsUnlocked { get => isUnlocked; }

    public void Init(int levelIndex, int star, bool unlocked, int bypass = 0)
    {
        this.levelIndex = levelIndex;
        txtStar.text = star.ToString();
        SetUnlock(unlocked);

        // todo :))
    }



    void SetUnlock(bool isUnlocked)
    {
        this.isUnlocked = isUnlocked;
        btn.targetGraphic.color = isUnlocked ? Color.black : Color.gray;
    }

    public void LockByBypass(int levelIndex) // wwhen need unlock bypassed level first
    {
        waitingBypassingLevel ??= new List<int>();
        waitingBypassingLevel.Add(levelIndex);
        SetUnlock(false);
        // set unlock to false here
    }

    public void UnlockByBypass(int levelIndex)
    {
        waitingBypassingLevel ??= new List<int>();
        waitingBypassingLevel.Remove(levelIndex);
        if (waitingBypassingLevel.Count == 0)
        {
            SetUnlock(true);
        }
        // set unlock to false here
    }

    public void RemoveBypassingLevel(int index)
    {

    }

    public void OnClick()
    {
        // create popup
        LevelSelectController.Instance.ShowLevelPopup(levelIndex, level,isUnlocked, waitingBypassingLevel);

    }

    public void MarkAsNew()
    {
        isNew = true;
    }


}
