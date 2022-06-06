using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Collection
{
    public class AbilitiesBoardContent : MonoBehaviour
    {
        public AllyName allyName;
        // Use this for initialization
        //[SerializeField]
        //List<AbilityItem> rootList = new List<AbilityItem>();
        [SerializeField]
        List<AbilityNode> nodeList = new List<AbilityNode>();
        public Action<AbilityNode> onFocus;

        private void Start()
        {
            nodeList.ForEach(x => x.abilitiesBoardContent = this);
        }
        public void Focus(AbilityNode focusItem)
        {
            nodeList.ForEach(x => x.SetNodeStatusStandalone(NodeStatus.Cannot));
            focusItem.Focus();
            onFocus?.Invoke(focusItem);
            //string desTemp = Language.AllyAbilities [focusItem.abilityName];
        }

        public void LoadCurrentStatus(Ally ally)
        {
            nodeList.ForEach(x => x.StatusReset());
            AbilityNode node = null;
            ally.appliedAbility.ForEach(x =>
            {
                node = nodeList.Find(node => node.abilityName == x);
                node?.SetStack();
            });
            node?.Focus(true);
            if(node != null)
                onFocus?.Invoke(node);
        }
    }
}