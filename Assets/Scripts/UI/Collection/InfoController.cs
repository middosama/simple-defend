using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoController : MonoBehaviour
{
    public Image image;
    public static InfoController Instance;
    public void ChangeBackground(Sprite sprite)
    {
        image.sprite = sprite;
    }
    private void Start()
    {
        Instance = this;
    }
}
