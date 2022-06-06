using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using Collection;
using UnityEngine.UI;
using TMPro;

public class OptionMenu : MonoBehaviour
{

    [SerializeField]
    ComfortPopupPosition popupPanel;

    [SerializeField]
    AbilitiesBoard abilitiesBoard;
    [SerializeField]
    Button btnApplyAbility;
    [SerializeField]
    TMP_Text txtAbilityPrice, txtSellPrice;

    [SerializeField]
    UnitsBoard unitsBoard;
    [SerializeField]
    GameObject abilityPanel, unitPanel;


    AbilityNode activingNode;
    bool isInited = false;

    static UnitPlaceholder currentTarget;
    static OptionMenu focusingMenu;

    static Vector2? _abilitiesBoardSize;
    static Vector2? _unitsBoardSize;
    static Vector2 rectSize
    {
        get
        {
            if (currentTarget.IsAssigned)
                return _abilitiesBoardSize ?? new Vector2(Screen.safeArea.height / 1.7f, Screen.safeArea.height / 1.3f);
            return _unitsBoardSize ?? new Vector2(Screen.safeArea.height / 2, Screen.safeArea.height / 1.5f);
        }
    }

    const float animDuration = 0.3f;


    //public Vector2 downSideVector = new Vector2(0,-45);

    //static SellOption _sellOption;
    //static SellOption sellOption
    //{
    //    get => _sellOption ??= SellOption.CreateInstance<SellOption>();
    //}

    public static void Show(UnitPlaceholder u)
    {
        if (currentTarget == u)
        {
            return;
        }
        if (focusingMenu != null)
        {
            focusingMenu.OnBlur();
        }
        currentTarget = u;
        OptionMenu x = Instantiate(Level.Instance.optionMenuPrefab, Main.CurrentGUI);
        focusingMenu = x;

        x.Init();
        //todo
    }

    private void LateUpdate()
    {

        if (isInited && Input.GetMouseButtonDown(0) && !IsPointerOver() && currentTarget.eventTarget.phase == TouchPhase.Ended)
        {
            OnBlur();
        }
    }

    bool IsPointerOver()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(popupPanel.selfRect, Input.mousePosition, Camera.main);
        //return EventS ystem.current.IsPointerOverGameObject();
    }


    void Init()
    {
        // set position 
        var position = Camera.main.WorldToScreenPoint(currentTarget.transform.position, Camera.MonoOrStereoscopicEye.Mono);
        popupPanel.selfRect.anchoredPosition = position;
        popupPanel.selfRect.sizeDelta = rectSize;
        popupPanel.FormatPosition();
        popupPanel.selfRect.localScale = Vector3.zero;
        popupPanel.selfRect.DOScale(Vector2.one, animDuration).SetUpdate(true);


        abilityPanel.SetActive(currentTarget.IsAssigned);
        unitPanel.SetActive(!currentTarget.IsAssigned);

        if (currentTarget.IsAssigned)
        {
            abilitiesBoard.onNodeFocus = OnAbilityFocus;
            abilitiesBoard.LoadAlly(currentTarget.ally);
            txtSellPrice.text = "$ +" + currentTarget.SellPrice.ToString();
        }
        else
        {
            unitsBoard.onUnitLoad = OnSelectUnit;

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
        currentTarget.BuyAlly(allyDescription);
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
        OnBlur();
    }

    public void OnBlur()
    {
        if (!isInited)
        {
            return;
        }
        Level.Instance.OnCoinChange.RemoveListener(ReCheckPrice);
        if (currentTarget.IsAssigned)
            currentTarget.ally.OnBlur();

        isInited = false;
        popupPanel.selfRect.DOScale(Vector2.zero, animDuration).OnComplete(() =>
        {
            Destroy(gameObject);
        }).SetUpdate(true);
        currentTarget = null;
    }

    public static void BlurAll()
    {
        focusingMenu?.OnBlur();
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
