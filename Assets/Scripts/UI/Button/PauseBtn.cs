using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseBtn : MonoBehaviour
{
    private int pauseState = 0;
    public Sprite[] switchSprite;
    public Image target;
    // Start is called before the first frame update
    private void Start()
    {
        pauseState = 0;
    }
    public void Toggle()
    {
        pauseState = 1 - pauseState;
        target.sprite = switchSprite[pauseState];
    }
}
