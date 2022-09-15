using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using Collection;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    public static OptionMenu Instance;

    [SerializeField]
    RectTransform selfTransform;

    public AbilitiesBoard abilitiesBoard;
    public Button btnApplyAbility, btnBuyUnit;
    public TMP_Text txtAbilityPrice, txtSellPrice, txtReviewingUnitName, txtReviewingUnitDescription, txtSelectingUnitName;

    public UnitsBoard unitsBoard;
    public GameObject abilityPanel, unitPanel;

    float lastCameraSize = 0;

    AbilityNode activingNode;
    bool isInited = false;

    static UnitPlaceholder currentTarget;




    const float animDuration = 0.3f;


    //public Vector2 downSideVector = new Vector2(0,-45);

    //static SellOption _sellOption;
    //static SellOption sellOption
    //{
    //    get => _sellOption ??= SellOption.CreateInstance<SellOption>();
    //}

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
        unitsBoard.onUnitLoad = OnSelectUnit;
    }

    public static void Show(UnitPlaceholder u)
    {
        if (currentTarget == u)
        {
            return;
        }
        currentTarget?.OnBlur();
        currentTarget = u;
        Instance.Init();

    }



    void Init()
    {
        if (!gameObject.activeSelf)
        {
            lastCameraSize = ZoomableCam.Instance.CurrentSize;
            selfTransform.pivot = Vector2.one;
            gameObject.SetActive(true);
            selfTransform.DOPivotX(0, animDuration).SetUpdate(true);
        }
        else
        {
            //StopReviewUnit();
        }
        ZoomableCam.Instance.Focus(currentTarget.transform.position, -0.3333f, 1);


        abilityPanel.SetActive(currentTarget.IsAssigned);
        unitPanel.SetActive(!currentTarget.IsAssigned);

        if (currentTarget.IsAssigned)
        {
            txtSelectingUnitName.text = currentTarget.ally.AllyName.GetDisplayName();
            abilitiesBoard.onNodeFocus = OnAbilityFocus;
            abilitiesBoard.LoadAlly(currentTarget.ally);
            txtSellPrice.text = "$ +" + currentTarget.SellPrice.ToString();
            currentTarget.OnFocus();
        }
        else
        {
            ClearUnitSelectState();
            txtReviewingUnitName.text = Language.Other["chooseUnit"];
            txtReviewingUnitDescription.text = Language.Other["chooseUnitDesc"];
        }

        isInited = true;
        Level.Instance.OnCoinChange.AddListener(ReCheckPrice);

    }

    void OnAbilityFocus(AbilityNode abilityNode)
    {
        //abilitiesBoard

        btnApplyAbility.gameObject.SetActive(!abilityNode.isLocking);
        activingNode = abilityNode;
        SetAbilityPrice();
    }

    void ReCheckPrice()
    {

        if (currentTarget.IsAssigned)
        {
            SetAbilityPrice();
            txtSellPrice.text = "$ +" + currentTarget.SellPrice;
        }
    }

    void SetAbilityPrice()
    {
        if (activingNode == null || activingNode.isLocking)
            return;
        int price = activingNode.allyAbility.Price;

        txtAbilityPrice.text = price.ToString();
        btnApplyAbility.interactable = Level.Instance.IsEnoughCoin(price);
        txtAbilityPrice.color = btnApplyAbility.interactable ? Color.black : Color.red;
    }

    void OnSelectUnit(AllyDescription allyDescription)
    {
        txtReviewingUnitName.text = allyDescription.displayName;
        txtReviewingUnitDescription.text = allyDescription.description;
        currentTarget.StopReview();
        btnBuyUnit.gameObject.SetActive(true);
        currentTarget.Review(allyDescription.allyTemplate);
    }

    public void OnBuyUnit()
    {
        currentTarget.BuyAlly(unitsBoard.selectingUnit);
        Blur();
    }

    void ClearUnitSelectState()
    {
        btnBuyUnit.gameObject.SetActive(false);
        unitsBoard.ClearState();
    }


    public void OnApplyAbility()
    {
        currentTarget.ApplyAbility(activingNode.allyAbility);
        abilitiesBoard.ReloadStatus();
        OnAbilityFocus(activingNode);
    }

    public void SellUnit()
    {
        currentTarget.Sell();
        Blur();

    }

    public void Blur()
    {
        if (!isInited)
        {
            return;
        }
        ClearUnitSelectState();

        Level.Instance.OnCoinChange.RemoveListener(ReCheckPrice);
        currentTarget.OnBlur();

        isInited = false;
        selfTransform.DOPivotX(1, animDuration).OnComplete(() =>
        {
            gameObject.SetActive(false);
        }).SetUpdate(true);
        ZoomableCam.Instance.ClampedSmoothZoom(lastCameraSize);
        currentTarget = null;
    }

}

public abstract class Option : ScriptableObject
{
    public Sprite thumbnail;
    [SerializeField]
    protected int price;
    public int Price { get => price; }

    public abstract string description
    {
        get;
    }

}
public class SellOption : Option
{
    public UnitPlaceholder SetPrice { set => price = value.SellPrice; }

    public override string description => Language.AllyAbilitiesDescription[AllyAbilityName.Sell];

}
