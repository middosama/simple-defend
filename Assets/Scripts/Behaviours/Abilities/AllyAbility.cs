using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AllyAbility", menuName = "ScriptableObjects/AllyAbility", order = 1)]
public class AllyAbility : Option
{
    [SerializeField]
    public AllyAbility[] nextAbility;
    [SerializeField]
    public AllyAbilityName allyAbilityName;
    public int maxStack = 1;
    public override string description => Language.AllyAbilitiesDescription[allyAbilityName];
}