using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Player 
{
    public static Player Instance;
    public static UnityEvent<int> OnCoinChange = new UnityEvent<int>();

    [SerializeField]
    public int[] UnitChooseOrder;

    RegularInfo rInfo;

    const string RegularlyFileName = "RF";
    const string NonRegularlyFileName = "NRF";

    public Player()
    {
        rInfo = DataManager.Load<RegularInfo>(RegularlyFileName, DataManager.PLAYER_INFO_PATH);
        if (rInfo == null)
            rInfo = new RegularInfo();

    }
    

    public AllyDescription[] ChooseAlly()
    {
        //todo
        // do something with order

        return Main.Instance.allyDescriptionList;
    }

    public static void CoinChange(int coin)
    {
        Instance.rInfo.CoinChange(coin);
        DataManager.Save(RegularlyFileName, DataManager.PLAYER_INFO_PATH, Instance.rInfo);
        OnCoinChange.Invoke(Instance.rInfo.Coin);
    }

    [Serializable]
    class RegularInfo
    {
        [SerializeField]
        int _coin;
        public int Coin
        {
            get => _coin;
        }

        public void CoinChange(int coin)
        {
            _coin += coin;
        }
        
    } 


}
