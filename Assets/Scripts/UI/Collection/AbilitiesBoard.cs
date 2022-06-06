using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Collection
{
    public class AbilitiesBoard : MonoBehaviour
    {
        public static AbilitiesBoard Instance;
        [SerializeField]
        TMP_Text abilityDes;
        [SerializeField]
        Transform container;
        public UnitGlobalConfig unitConfig;
        AbilitiesBoardContent showingContent;
        public Action<AbilityNode> onNodeFocus;
        public Ally loadedAlly;

        private void Start()
        {
            Instance = this;
        }

        public void LoadAlly(Ally ally)
        {
            LoadAllyAbilities(ally.AllyName);
            showingContent.onFocus = onNodeFocus;
            showingContent.LoadCurrentStatus(ally);
            loadedAlly = ally;
        }

        public void ReloadStatus()
        {
            showingContent.LoadCurrentStatus(loadedAlly);
        }

        public void LoadAllyAbilities(AllyName allyName)
        {
            if (showingContent != null)
                Destroy(showingContent.gameObject);
            AbilitiesBoardContent content = unitConfig.abilitiesContentList.Find(x => x.allyName == allyName);
            if (content == null) return;
            showingContent = Instantiate(content, container);
        }

        public void SetDescription(string description)
        { // ????
            abilityDes.text = description;
        }
    }
}