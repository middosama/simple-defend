using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AllyDescription", menuName = "ScriptableObjects/AllyDescription", order = 1)]
[Serializable]
public class AllyDescription : Option
{
    [SerializeField]
    Ally _allyTemplate;
    public Ally allyTemplate { get=> _allyTemplate; }
    [SerializeField]
    public AllyName allyName;
    [SerializeField]
    AllyAbility _firstAbility;
    public AllyAbility firstAbility { get=> _firstAbility; }
    public override string description => Language.AllyDescription[allyName];
}