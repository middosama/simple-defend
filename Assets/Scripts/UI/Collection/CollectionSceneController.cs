using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Collection
{
    public class CollectionSceneController : MonoBehaviour
    {
        public static CollectionSceneController Instance;
        public TMP_Text txtAllyName;
        public UnitsBoard unitBoard;
        public Button backBtn;
        // Use this for initialization
        void Start()
        {
            Instance = this;
            //unitBoard.
            unitBoard.onUnitLoad = LoadAlly;
        }
        public void LoadAlly(AllyDescription allyDescription)
        {
            // todo load ally with skin
            AbilitiesBoard.Instance.LoadAllyAbilities(allyDescription.allyName);
            txtAllyName.text = "{lang.allyName}";
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("Home");
        }

    }
}