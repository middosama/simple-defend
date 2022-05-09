using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInfoDisplayController : MonoBehaviour
{
    public static UnityEvent onDataChange = new UnityEvent();

    public TMP_Text txtCheckPointTicketCount, txtBypassTicketCount, txtCoin;

    private void OnEnable()
    {
        onDataChange.AddListener(ReLoadData);
        ReLoadData();
    }

    private void OnDisable()
    {
        onDataChange.RemoveListener(ReLoadData);
    }

    void ReLoadData()
    {
        txtCoin?.SetText(Player.Coin.ToString());
        txtBypassTicketCount?.SetText(Player.BypassTicket.ToString());
        txtCheckPointTicketCount?.SetText(Player.CheckpointTicket.ToString());
    }
}
