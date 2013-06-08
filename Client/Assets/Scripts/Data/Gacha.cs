using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GachaData
{
    private int mId;
    private int mCardDropGroup;
    private int mCost;

    public int ID
    {
        get { return mId; }
        set { mId = value; }
    }

    public int CardDropGroup
    {
        get { return mCardDropGroup; }
        set { mCardDropGroup = value; }
    }

    public int Cost
    {
        get { return mCost; }
        set { mCost = value; }
    }
}

public class GachaManager
{
    private static volatile GachaManager instance;
    private static object syncRoot = new System.Object();

    public static GachaManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new GachaManager();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<int, GachaData> mGachas = new Dictionary<int, GachaData>();

    public void ResetData(sGachaList _gachaList)
    {
        mGachas.Clear();

        foreach (sGachaData gacha in _gachaList.gacha)
        {
            GachaData data = new GachaData();
            data.ID = gacha.id;
            data.CardDropGroup = gacha.card_drop;
            data.Cost = gacha.cost;

            mGachas[data.ID] = data;
        }
    }
}
