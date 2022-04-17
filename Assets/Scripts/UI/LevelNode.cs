using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class LevelNode : MonoBehaviour
{
    [SerializeField]
    Level level;
    //LinkedListNode<LevelNode> currentNode;
    int levelIndex;

    [SerializeField]
    TMP_Text txtStar; // fix later

    List<int> bypassLevel;

    bool isNew = false;
    public void Init(int levelIndex, int star, bool unlocked, bool isBypassed = false)
    {
        this.levelIndex = levelIndex;
        txtStar.text = star.ToString();
    }

    public void LockBypass(int levelIndex)
    {
        bypassLevel.Add(levelIndex);
        // set unlock to false here
    }

    public void OnClick()
    {
        // create popup
        LevelSelectController.Instance.ShowLevelPopup(levelIndex, level);

    }

    public void MarkAsNew()
    {
        isNew = true;
    }


}
