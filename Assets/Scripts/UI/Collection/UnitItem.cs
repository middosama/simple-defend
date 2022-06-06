using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Collection
{
    public class UnitItem : MonoBehaviour
    {
        public Image thumbnail;
        protected AllyDescription ally;
        
        public virtual void Init(AllyDescription ally)
        {
            thumbnail.sprite = ally.thumbnail;
            this.ally = ally;
        }
        public void OnClick()
        {
            UnitsBoard.Instance.LoadUnit(ally);
        }
    }
}