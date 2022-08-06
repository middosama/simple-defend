using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedBtn : MonoBehaviour
{
    private int speed;
    public TMP_Text currentSpd;
    // Start is called before the first frame update
    private void Start()
    {
        speed = 0;
    }
    public void ChangeSpeed ()
    {
        speed++;
        if (speed>2)
        {
            speed = 0;
        }
        if (speed == 0 )
        {
            currentSpd.text = "1x";
        } else
        {
            currentSpd.text = (speed * 2).ToString() + "x";
        }
    }
}
