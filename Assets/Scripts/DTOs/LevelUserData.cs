using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelUserData
{
    List<PassLevelRecord> records;
    int highestHealth { get => records.Max(x => x.healthLeft); }


}
public struct PassLevelRecord
{
    List<Invest> invests;
    public int healthLeft;

    struct Invest
    {
        public AllyName name;
        public int investPercent;
    }
}


