using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Language
{
    public static Dictionary<AllyName, string> AllyDescription;
    public static Dictionary<AllyName, string> AllyName;
    public static Dictionary<AllyAbilityName, string> AllyAbilitiesName;
    public static Dictionary<AllyAbilityName, string> AllyAbilitiesDescription;
    public static string[][] LevelDescription;
    public static Dictionary<string,string> Other;

    //static StringBuilder sb = new StringBuilder();
    public static void Init()
    {
        Other = new Dictionary<string, string>();
        // hardcode, English
        Other["needToSolveBeforeNextZone"] = "You need to pass this level before nexxt zone";
    }

}
