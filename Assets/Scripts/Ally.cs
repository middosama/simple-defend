using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Ally : MonoBehaviour
{

    //public override Faction Faction { get => Faction.Ally; }


    //public abstract void Assign(UnitPlaceholder unitPlaceholder);

    protected abstract void OnAssign();

    public AllyAbility[] GetAbilities()
    {
        return currentAbility.nextAbility;
    }

    public List<AllyAbility> appliedAbility = new List<AllyAbility>();

    public void ApplyAbility(AllyAbility ability)
    {
        appliedAbility.Add(ability);
        currentAbility = ability;
        OnApplyAbility(ability);
    }
    public abstract void OnApplyAbility(AllyAbility ability);
    public abstract float SellValue { get; }

    public abstract AllyName AllyName { get; }

    UnitPlaceholder unitPlaceholder;

    protected AllyAbility currentAbility;

    public void Assign(UnitPlaceholder unitPlaceholder)
    {
        this.unitPlaceholder = unitPlaceholder;

        currentAbility = Main.allyDescriptions[AllyName].firstAbility;
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
