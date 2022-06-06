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
    private void Start()
    {
        Instance = this;
        var allyList = Player.Instance.GetAllyList();
        foreach (var allyDescription in allyList)
        {
            Instantiate(unitItemTemplate, unitPoolContainer).Init(allyDescription);
        }

    }

    public void LoadUnit(AllyDescription allyDescription)
    {
        onUnitLoad?.Invoke(allyDescription);
    }
    
}
