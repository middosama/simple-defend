using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Language
{
    public static Dictionary<AllyName, string> AllyDescription;
    public static Dictionary<AllyName, string> AllyNames;
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
        Other["chooseUnit"] = "Choose unit";
        Other["chooseUnitDesc"] = "";
        AllyNames = new Dictionary<AllyName, string>();
        AllyNames.Add(AllyName.Simple, "Simple");
        AllyNames.Add(AllyName.Complex, "Complex");
        AllyNames.Add(AllyName.TestObject, "TestObject");
        AllyDescription = new Dictionary<AllyName, string>();
        AllyDescription.Add(AllyName.Simple, "Một cái gì đó mạnh đầu game và yếu cuối game,....");
        AllyDescription.Add(AllyName.Complex, "ComplexDesc");
        AllyDescription.Add(AllyName.TestObject, "ObjDesc");
    }

    public static string GetDisplayName(this AllyName allyName)
    {
        return AllyNames[allyName];
    }

}
