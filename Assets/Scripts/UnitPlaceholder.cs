using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UnitPlaceholder : MonoBehaviour
{
    public WorldClickEvent eventTarget;
    Ally _ally;
    public Ally ally
    {
        get => _ally;
        private set
        {
            _ally = value;
        }
    }

    int investedValue;

    public int SellPrice
    {
        get => Mathf.RoundToInt(investedValue * _ally.SellValue);
    }

    // Use this for initialization

    public void ShowOption()
    {
        OptionMenu.Show(this);
    }

    public void BuyAlly(AllyDescription allyDescription)
    {
        if (IsAssigned)
            return;
        investedValue += allyDescription.Price;
        Level.Instance.CoinChange(allyDescription.Price);
        AssignAlly(allyDescription);
    }

    public void AssignAlly(AllyDescription allyDescription)
    {
        ally = Instantiate(allyDescription.allyTemplate, transform);
        eventTarget.targetGraphic.enabled = false;
        ally.Assign(this);
        ally.ApplyAbility(allyDescription.firstAbility.allyAbilityName);
    }

    public void LoadSnapshot(UnitSnapshot snapshot= null)
    {
        Clear();
        if (snapshot != null)
        {
            Clear();
            AssignAlly(Main.allyDescriptions[snapshot.allyName]);
            snapshot.appliedAbility.ForEach(x =>
            {
                ally.ApplyAbility(x);
            });
            investedValue = snapshot.investedValue;
        }
    }

    public UnitSnapshot GetSnapshot()
    {
        if (ally == null)
            return null;
        return new UnitSnapshot(ally, investedValue);
    }

    //public void ChooseOption(AllyAbilityName abilityName)
    //{
    //    ally.ApplyAbility(Main.allyAbilities[abilityName]);
    //}

    public void Sell()
    {
        Level.Instance.CoinChange(-SellPrice);
        Clear();
    }

    void Clear()
    {
        investedValue = 0;
        if (ally != null)
        {
            Destroy(ally.gameObject);
            eventTarget.targetGraphic.enabled = true;
            ally = null;
        }
    }

    public void ApplyAbility(AllyAbility ability)
    {
        investedValue += ability.Price;
        Level.Instance.CoinChange(ability.Price);
        ally.ApplyAbility(ability.allyAbilityName);
    }


    public bool IsAssigned { get => ally != null; }

    public void OnClick()
    {
        ShowOption();
    }

}
public class UnitSnapshot
{
    public AllyName allyName { get; private set; }
    public List<AllyAbilityName> appliedAbility { get; private set; }
    public int investedValue;

    public UnitSnapshot(Ally ally, int invest)
    {
        investedValue = invest;
        allyName = ally.AllyName;
        appliedAbility = new List<AllyAbilityName>(ally.appliedAbility);
    }
}
