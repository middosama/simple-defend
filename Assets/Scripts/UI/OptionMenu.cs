using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;


public class OptionMenu : MonoBehaviour
{
    [SerializeField]
    Transform actionContainer;
    [SerializeField]
    RectTransform canvas, selfRect;

    bool isInited = false;

    static UnitPlaceholder unitPlaceholder;
    static OptionMenu focusingMenu;
    const float animDuration = 0.3f;


    public Vector2 downSideVector = new Vector2(0,-45);

    static SellOption _sellOption;
    static SellOption sellOption
    {
        get => _sellOption ??= SellOption.CreateInstance<SellOption>();
    }

    public static void Show( UnitPlaceholder u)
    {
        if (unitPlaceholder == u)
        {
            return;
        }
        if(focusingMenu!= null)
        {
            focusingMenu.OnBlur();
        }
        unitPlaceholder = u;
        OptionMenu x = Instantiate(Level.Instance.optionMenuPrefab,Main.CurrentGUI);
        focusingMenu = x;
        //x.transform.SetParent(unitPlaceholder.transform, false);

        x.Init();
        //todo
    }

    private void Update()
    {
        if (isInited && Input.GetMouseButtonDown(0) && !IsPointerOver())
        {
            OnBlur();
        }
    }

    bool IsPointerOver()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    //public static void Show(Ally ally, UnitPlaceholder unitPlaceholder)
    //{
    //    //todo
    //}


    void Init()
    {
        selfRect.anchoredPosition = Camera.main.WorldToScreenPoint(unitPlaceholder.transform.position);
        foreach (Transform child in actionContainer.transform)
        {
            Destroy(child.gameObject);
        }

        if (unitPlaceholder.IsAssigned)
        {
            sellOption.SetPrice = unitPlaceholder;
            Instantiate(Level.Instance.optionItemPrefab, actionContainer.transform).Init(downSideVector, sellOption, (x) =>
            {
                unitPlaceholder.Sell();
                OnBlur();
            });
            var options = unitPlaceholder.ally.GetAbilities();
            float singleAngle = 360 / (options.Length + 1);
            for (int i = 0; i < options.Length; i++)
            {
                Vector2 pos = Quaternion.AngleAxis(singleAngle * (i+1), Vector3.back) * downSideVector;
                Instantiate(Level.Instance.optionItemPrefab, actionContainer.transform).Init(pos, options[i],(x) =>
                {
                    unitPlaceholder.ApplyAbility(x);
                    OnBlur();
                });
            }

            unitPlaceholder.ally.OnFocus();
        }
        else
        {
            var options = Player.Instance.ChooseAlly();
            for (int i = 0; i < options.Length; i++)
            {
                Vector2 pos = Quaternion.AngleAxis(360 / options.Length * i, Vector3.back) * downSideVector;
                Instantiate(Level.Instance.optionItemPrefab, actionContainer.transform).Init(pos, options[i],(x) =>
                {
                    unitPlaceholder.BuyAlly(x);
                    OnBlur();
                });
            }
        }
        //unitPlaceholder
        // check 
        
        canvas.localScale = Vector2.zero;
        canvas.DOScale(Vector2.one, animDuration).OnComplete(() =>
        {
            isInited = true;
        }).SetUpdate(true);
    }

    public void OnBlur()
    {
        if (!isInited)
        {
            return;
        }
        if (unitPlaceholder.IsAssigned)
            unitPlaceholder.ally.OnBlur();
        
        isInited = false;
        canvas.DOScale(Vector2.zero, animDuration).OnComplete(() =>
        {
            Destroy(gameObject);
        }).SetUpdate(true);
        unitPlaceholder = null;
    }

    public static void BlurAll()
    {
        focusingMenu?.OnBlur();
    }

}

public abstract class Option : ScriptableObject
{
    public Sprite image;
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

    public override string description => Language.AllyAbilities[AllyAbilityName.Sell];

}
