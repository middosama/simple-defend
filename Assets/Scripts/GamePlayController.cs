using System.Collections;
using UnityEngine;


public class GamePlayController : MonoBehaviour
{
    public static Level SelectedLevel;
    // Use this for initialization
    void Start()
    {
        Instantiate(SelectedLevel);
        Main.EndLoading();
    }
    
}
