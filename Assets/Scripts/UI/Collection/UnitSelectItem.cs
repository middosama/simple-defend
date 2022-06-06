using System.Collections;
using TMPro;
using UnityEngine;

namespace Collection
{
    public class UnitSelectItem : UnitItem
    {
        public TMP_Text price;
        public GameObject icoLock;

        public override void Init(AllyDescription ally)
        {
            base.Init(ally);
            SetPrice();
            Level.Instance.OnCoinChange.AddListener(SetPrice);
        }

        private void OnDestroy()
        {
            Level.Instance.OnCoinChange.RemoveListener(SetPrice);
        }

        void SetPrice()
        {
            if (ally.Price > 0)
            {
                if (Level.Instance.IsEnoughCoin(ally.Price))
                {
                    price.text = ally.Price.ToString();
                    icoLock.SetActive(false);
                }
                else
                {
                    price.text = $"<color=#ff0000>{-ally.Price}</color>";
                    icoLock.SetActive(true);
                }

            }
            else
            {
                icoLock.SetActive(false);
                price.text = "<color=#005500>+</color>" + (-ally.Price);
            }
        }
    }
}