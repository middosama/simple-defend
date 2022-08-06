using DG.Tweening;
using System.Collections;
using UnityCommon;
using UnityEngine;
using UnityEngine.UI;

namespace Collection
{
    public class AbilityPath : MonoBehaviour
    {
        public AbilityNode destinationItem;
        public Image border;
        public float blinkAlpha = 0.3f;
        Tweener blinkFX;
        public Color mainTheme;
        float duration;
        // Use this for initialization
        public void SetPathStatus(NodeStatus status)
        {
            // visualize 
            //SetPathStatusStandalone(status);
            destinationItem.SetNodeStatus(status);
        }

        public void SetPathStatusStandalone(NodeStatus status)
        {
            blinkFX.Kill(true);
            border.color = mainTheme;
            if (status == NodeStatus.Focusing || status == NodeStatus.Next || status == NodeStatus.Canbe)
            {
                DoBlink(status);
            }
        }
        public void SetUnlock(bool isUnlock)
        {
            destinationItem.SetUnlock(isUnlock);
        }
        void DoBlink(NodeStatus status)
        {
            border.color = mainTheme;
            switch (status)
            {
                case NodeStatus.Focusing:
                    duration = 0.2f;
                    break;
                case NodeStatus.Next:
                    duration = 0.4f;
                    break;
                case NodeStatus.Canbe:
                    duration = 0.5f;
                    break;
            }
            blinkFX = border?.DoBlink(blinkAlpha, duration);
        }
    }
}