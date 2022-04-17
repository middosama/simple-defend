using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllyWithNumberCountItem : MonoBehaviour
{
    public Image image;
    public TMP_Text txtCount;
    
    public void Init(Sprite sprite, int count)
    {
        txtCount.text = "x"+count;
        image.sprite = sprite;
    }

}
