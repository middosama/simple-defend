using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Collection
{
    public class AbilityNode : MonoBehaviour
    {
        [SerializeField]
        //AbilityPath[] abilityPaths;
        AbilityNode[] abilityPaths;
        public AllyAbility allyAbility;
        public AllyAbilityName abilityName { get => allyAbility.allyAbilityName; }
        [SerializeField]
        Button button;
        [SerializeField]
        TMP_Text txtStack;
        [SerializeField]
        Image icoLock;

        int stack = 0;
        public bool isLocking
        {
            get;
            private set;
        }
        [HideInInspector]
        public NodeStatus currentStatus;
        [HideInInspector]
        public AbilitiesBoardContent abilitiesBoardContent;

        public void Focus(bool isCurrentNode = false)
        {
            SetNodeStatusStandalone(NodeStatus.Focusing);
            foreach (var path in abilityPaths)
            {
                //path.SetPathStatus(isActive);
                path.SetNodeStatus(NodeStatus.Next);
                if (isCurrentNode)
                    path.SetUnlock(true);
            }
        }

        public void SetUnlock(bool isUnlock)
        {
            isLocking = !isUnlock;
            icoLock.gameObject.SetActive(isLocking);
        }

        public void StatusReset()
        {
            SetUnlock(false);
            SetNodeStatusStandalone(NodeStatus.Cannot);
            stack = 0;
        }

        public void SetNodeStatus(NodeStatus status)
        {
            if (status >= currentStatus) return;
            SetNodeStatusStandalone(status);
            if (status == NodeStatus.Next)
                status = NodeStatus.Canbe;
            foreach (var path in abilityPaths)
            {
                //path.SetPathStatus(isActive);
                path.SetNodeStatus(status);
            }
        }

        public void SetNodeStatusStandalone(NodeStatus status)
        {
            // visualize
            currentStatus = status;
            switch (status)
            {
                case NodeStatus.Focusing:
                    button.targetGraphic.color = Color.red;
                    break;
                case NodeStatus.Next:
                    button.targetGraphic.color = Color.blue;
                    break;
                case NodeStatus.Canbe:
                    button.targetGraphic.color = Color.white;
                    break;
                case NodeStatus.Cannot:
                    button.targetGraphic.color = Color.black;
                    break;

            }
        }

        public void SetStack()
        {
            stack++;
            txtStack.text = stack + "/" + allyAbility.maxStack;
        }

        public void Toggle()
        {
            abilitiesBoardContent.Focus(this);
        }
    }

    public enum NodeStatus
    {
        Focusing,
        Next,
        Canbe,
        Cannot
    }
}