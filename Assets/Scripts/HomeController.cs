using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    public Button playBtn;
    // Start is called before the first frame update
    void Start()
    {
        Main.EndLoading();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadCollection()
    {
        Main.LoadScene("CollectionScene");
    }
    public void LoadStage()
    {
        Main.LoadScene("LevelSelectScene");
    }

    public void OnHover (Image image)
    {
        image.color = new Color(255, 255, 255, 1f);
    }

    public void OutHover (Image image)
    {
        image.color = new Color(255, 255, 255, 0f);
    }
}
