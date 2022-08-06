using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityCommon;

namespace Collection
{
    public class AbilityNode : MonoBehaviour
    {
        [SerializeField]
        AbilityPath[] abilityPaths = new AbilityPath[0];
        //AbilityNode[] abilityPaths;
        public AllyAbility allyAbility;
        public AllyAbilityName abilityName { get => allyAbility.allyAbilityName; }
        [SerializeField]
        Button button;
        [SerializeField]
        TMP_Text txtStack;
        [SerializeField]
        Image icoLock;
        public Image border;
        public Sprite focusSprite;
        public Sprite originSprite;
        //public Sprite focusSprite, nextSprite, canbeSprite;
        public Color focusColor, nextColor, canbeColor;
        public float blinkAlpha = 0.3f;

        Tweener blinkFX;
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
                path.SetPathStatus(NodeStatus.Next);
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
            txtStack.text = "0/" + allyAbility.maxStack;
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
                path.SetPathStatus(status);
            }
        }

        public void SetNodeStatusStandalone(NodeStatus status)
        {
            // visualize
            currentStatus = status;
            blinkFX.Kill(true);
            switch (status)
            {
                case NodeStatus.Focusing:
                    border.sprite = focusSprite;
                    /*border.color = focusColor;*/
                    DoBlink();
                    //button.targetGraphic.color = Color.red;
                    break;
                case NodeStatus.Next:
                    //button.targetGraphic.color = Color.blue;
                    //border.sprite = nextSprite;
                    /*border.color = nextColor;*/
                    DoBlink();
                    break;
                case NodeStatus.Canbe:
                    //button.targetGraphic.color = Color.white;
                    //border.sprite = canbeSprite;
                    /*border.color = canbeColor;*/
                    //DoBlink();
                    break;
                case NodeStatus.Cannot:
                    /*border.gameObject.SetActive(false);*/
                    border.sprite = originSprite;
                    border.color = Color.white;
                    break;
            }
            foreach (var path in abilityPaths)
            {
                path.SetPathStatusStandalone(status);
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

        void DoBlink()
        {
            blinkFX = border?.DoBlink(blinkAlpha);
        }

        private void OnDestroy()
        {
            blinkFX.Kill();
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