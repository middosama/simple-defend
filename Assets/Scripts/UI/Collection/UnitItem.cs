using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Collection
{
    public class UnitItem : MonoBehaviour
    {
        public Image thumbnail;
        public Button button;
        public AllyDescription ally;
        
        public virtual void Init(AllyDescription ally)
        {
            thumbnail.sprite = ally.thumbnail;
            this.ally = ally;
            SetStatus(false);
        }
        public void OnClick()
        {
            UnitsBoard.Instance.LoadUnit(this);
        }

        public void SetStatus(bool isActiving)
        {
            button.interactable = !isActiving;
        }
    }
}