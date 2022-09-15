using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Main : MonoBehaviour
{
    public static Transform ObjectPool { get; set; }
    public static Transform CurrentGUI { get; set; }

    [SerializeField]
    public AllyDescription[] allyDescriptionList;
    [SerializeField]
    RectTransform mainUICanvas, loadingCloudL, loadingCloudR;
    [SerializeField]
    Canvas UICanvas;

    public static Dictionary<AllyName, AllyDescription> allyDescriptions = new Dictionary<AllyName, AllyDescription>();

    const float duration = 1f;

    public static Sequence loadingSequence;
    public static Action beginAnimDoneAction;

    public static Main Instance;

    private void Awake()
    {
        
        Instance = this;
        DontDestroyOnLoad(this);
        Player.Instance = new Player() { UnitChooseOrder = { } };
        Language.Init();

        //return;

        allyDescriptions.Clear();
        foreach (var allyDescription in allyDescriptionList)
        {
            allyDescriptions.Add(allyDescription.allyName, allyDescription);
        }
        //foreach (var ally in allyTemplateList)
        //{
        //    allyTemplates.Add(ally.AllyName, ally);
        //}
        //foreach (var ability in allyAbilityList)
        //{
        //    allyAbilities.Add(ability.allyAbilityName, ability);
        //}


        loadingSequence = DOTween.Sequence();
        loadingSequence.Append(loadingCloudL.DOAnchorPosX(mainUICanvas.sizeDelta.x, duration))
            .Join(loadingCloudR.DOAnchorPosX(-mainUICanvas.sizeDelta.x, duration))
            .SetAutoKill(false)
            .SetUpdate(true)
            .OnStepComplete(() =>
            {
                if (beginAnimDoneAction != null)
                {
                    beginAnimDoneAction();
                    //loadingDoneAction = null;
                }
            })
            .SetDelay(0.05f);

        beginAnimDoneAction = () =>
        {
            LoadScene("Home");
        };
    }

    public static void LoadScene(string sceneName, Action<AsyncOperation> onLoadDone = null)
    {
        beginAnimDoneAction = () =>
        {
            Time.timeScale = 1;
            var asyncOperator = SceneManager.LoadSceneAsync(sceneName);
            
            if (onLoadDone != null)
            {
                asyncOperator.completed += onLoadDone;
            }
            beginAnimDoneAction = null;
        };
        loadingSequence.PlayBackwards();
    }

    public static void EndLoading()
    {
        loadingSequence.PlayForward();
        //Instance.UICanvas.worldCamera = Camera.main;
    }


}
