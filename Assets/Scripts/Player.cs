using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

internal class Player
{
    internal static Player Instance;
    internal static UnityEvent<int> OnCoinChange = new UnityEvent<int>();

    [SerializeField]
    public int[] UnitChooseOrder;

    RegularInfo rInfo;

    const string RegularlyFileName = "RF";
    const string NonRegularlyFileName = "NRF";

    internal Player()
    {
        rInfo = DataManager.Load<RegularInfo>(RegularlyFileName, DataManager.PLAYER_INFO_PATH);
        if (rInfo == null)
            rInfo = new RegularInfo();

    }


    internal AllyDescription[] GetAllyList()
    {
        //todo
        // do something with order

        return Main.Instance.allyDescriptionList;
    }

    internal static void CoinChange(int coin)
    {
        Instance.rInfo.CoinChange(coin);
        Save(RegularlyFileName);
        OnCoinChange.Invoke(Instance.rInfo.Coin);
    }

    internal static int Coin { get => Instance.rInfo.Coin; }
    internal static bool CreateCheckpoint()
    {
        if (Instance.rInfo.CreateCheckpoint())
        {
            Save(RegularlyFileName);
            return true;
        }
        return false;
    }
    internal static bool Bypass()
    {
        if (Instance.rInfo.Bypass())
        {
            Save(RegularlyFileName);
            return true;
        }
        return false;
    }

    internal static int CheckpointTicket { get => Instance.rInfo.CheckpointTicket; }
    internal static int BypassTicket { get => Instance.rInfo.BypassTicket; }

    static void Save(string fileName)
    {
        DataManager.Save(fileName, DataManager.PLAYER_INFO_PATH, Instance.rInfo);
        PlayerInfoDisplayController.onDataChange.Invoke();
    }

    [Serializable]
    class RegularInfo
    {
        [SerializeField]
        int _coin;
        [SerializeField]
        int _checkpointTicket = 99999;
        [SerializeField]
        int _bypassTicket = 9;
        public int Coin
        {
            get => _coin;
        }
        public int CheckpointTicket
        {
            get => _checkpointTicket/3;
        }
        public int BypassTicket
        {
            get => _bypassTicket / 3;
        }

        public void CoinChange(int coin)
        {
            _coin += coin;
        }
        public bool CreateCheckpoint()
        {
            if (_checkpointTicket <= 0)
                return false;
            _checkpointTicket-=3;
            return true;
        }
        public bool Bypass()
        {
            if (_bypassTicket <= 0)
                return false;
            _bypassTicket -= 3;
            return true;
        }
    } 


}
