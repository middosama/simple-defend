using Collection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitGlobalConfig", menuName = "ScriptableObjects/UnitGlobalConfig")]
public class UnitGlobalConfig : ScriptableObject
{
    public List<AbilitiesBoardContent> abilitiesContentList;
}
