using Collection;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class UnitsBoard : MonoBehaviour
{
    public Transform unitPoolContainer;
    public static UnitsBoard Instance;
    public Action<AllyDescription> onUnitLoad;
    public UnitItem unitItemTemplate;
    UnitItem selectingUnitItem;

    public AllyDescription selectingUnit
    {
        get => selectingUnitItem?.ally;
    }
    private void Start()
    {
        Instance = this;
        var allyList = Player.Instance.GetAllyList();
        foreach (var allyDescription in allyList)
        {
            Instantiate(unitItemTemplate, unitPoolContainer).Init(allyDescription);
        }

    }

    public void ClearState()
    {
        selectingUnitItem?.SetStatus(false);
        selectingUnitItem = null;
    }

    public void LoadUnit(UnitItem unitItem)
    {
        selectingUnitItem?.SetStatus(false);
        selectingUnitItem = unitItem;
        selectingUnitItem.SetStatus(true);
        onUnitLoad?.Invoke(unitItem.ally);
    }

}
