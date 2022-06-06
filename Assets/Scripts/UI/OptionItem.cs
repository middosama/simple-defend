using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionItem : MonoBehaviour
{
    static OptionItem confirmingOption;
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    TMP_Text price;
    [SerializeField]
    Image optionIcon;
    [SerializeField]
    GameObject lockIcon;
    Action action;
    Option option;
    public void Init<T>(Vector2 localPos, T option, Action<T> action) where T : Option
    {
        rectTransform.localPosition = localPos;
        this.action = () =>
        {
            action(option);
        };
        this.option = option;
        if(option.thumbnail != null)
            optionIcon.sprite = option.thumbnail;
        SetPrice();
        Level.Instance.OnCoinChange.AddListener(SetPrice);
    }

    void SetPrice()
    {
        if (option.Price > 0)
        {
            if (Level.Instance.IsEnoughCoin(option.Price))
            {
                price.text = option.Price.ToString();
                lockIcon.SetActive(false);
            }
            else
            {
                price.text = $"<color=#ff0000>{-option.Price}</color>";
                lockIcon.SetActive(true);
            }

        }
        else
        {
            lockIcon.SetActive(false);
            price.text = "<color=#005500>+</color>" + (-option.Price);
        }
    }

    private void OnDestroy()
    {
        Level.Instance.OnCoinChange.RemoveListener(SetPrice);
        if (confirmingOption == this)
        {
            confirmingOption = null;
        }
    }

    public void Click()
    {
        if (confirmingOption != null)
        {
            confirmingOption.Reject();
        }
        if (!lockIcon.activeSelf)
        {

            confirmingOption = this;
            optionIcon.gameObject.SetActive(false);
        }
    }
    public void Reject()
    {
        optionIcon.gameObject.SetActive(true);
    }

    public void OnConfirm()
    {
        action();
    }
}
