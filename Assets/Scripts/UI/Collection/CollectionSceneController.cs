using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Collection
{
    public class CollectionSceneController : MonoBehaviour
    {
        public static CollectionSceneController Instance;
        public TMP_Text txtAllyName;
        public UnitsBoard unitBoard;
        // Use this for initialization
        void Start()
        {
            Instance = this;
            unitBoard.onUnitLoad = LoadAlly;
        }
        public void LoadAlly(AllyDescription allyDescription)
        {
            // todo load ally with skin
            AbilitiesBoard.Instance.LoadAllyAbilities(allyDescription.allyName);
            txtAllyName.text = "{lang.allyName}";
        }



    }
}