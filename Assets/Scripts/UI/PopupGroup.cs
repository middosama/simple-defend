using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupGroup : MonoBehaviour
{
    List<PopupEffect> popupList = new List<PopupEffect>();
    List<PopupEffect> showingList = new List<PopupEffect>();
    public UnityEvent<PopupEffect> onFirstPopupShow = new UnityEvent<PopupEffect>(); 
    public UnityEvent<PopupEffect> onLastPopupHide = new UnityEvent<PopupEffect>(); 
    public void Assign(PopupEffect popup)
    {
        if (!popupList.Contains(popup))
        {
            popupList.Add(popup);
        }
    }

    public void OnShow(PopupEffect popup)
    {
        showingList.Add(popup);
        if (showingList.Count == 1)
        {
            onFirstPopupShow.Invoke(popup);
        }
    }
    public void OnHide(PopupEffect popup)
    {
        showingList.Remove(popup);
        if (showingList.Count == 0)
        {
            onLastPopupHide.Invoke(popup);
        }
    }

    public void HideAll()
    {
        for (int i = showingList.Count -1; i > -1; i--)
        {
            showingList[i].Close();
        }
    }
}
