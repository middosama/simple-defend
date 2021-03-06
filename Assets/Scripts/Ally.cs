using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Ally : MonoBehaviour
{

    //public override Faction Faction { get => Faction.Ally; }


    //public abstract void Assign(UnitPlaceholder unitPlaceholder);

    protected abstract void OnAssign();

    //public AllyAbility[] GetAbilities()
    //{
    //    return currentAbility.nextAbility;
    //}

    public List<AllyAbilityName> appliedAbility = new List<AllyAbilityName>();

    public void ApplyAbility(AllyAbilityName ability)
    {
        appliedAbility.Add(ability);
        OnApplyAbility(ability);
    }
    public abstract void OnApplyAbility(AllyAbilityName ability);
    public abstract float SellValue { get; }

    public abstract AllyName AllyName { get; }

    UnitPlaceholder unitPlaceholder;

    public void Assign(UnitPlaceholder unitPlaceholder)
    {
        this.unitPlaceholder = unitPlaceholder;

        OnAssign();
    }

    public abstract void OnFocus();
    public abstract void OnBlur();

    

}

public enum AllyName
{
    Simple
}









public class Skill
{
    public string description;
    Sprite sprite;
}
