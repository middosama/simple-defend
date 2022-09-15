using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubNodethingy : MonoBehaviour
{
    public RectTransform selfRectTransform;
    public Image image;
    // Start is called before the first frame update
    
    public void Init(Vector2 pos, bool status)
    {
        selfRectTransform.anchoredPosition = pos;
        image.color = (status) ? Color.white : Color.black;
    }
}
