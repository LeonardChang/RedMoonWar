using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 小关卡
/// </summary>
[System.Serializable]
public class Battle
{
    private string mName;
    private bool mFinish;
    private int mPowerCost;
}

/// <summary>
/// 大关卡
/// </summary>
[System.Serializable]
public class BigBattle
{
    private string mName;
    private List<Battle> mBattleList = new List<Battle>();
}

/// <summary>
/// 战场数据
/// </summary>
[System.Serializable]
public class Battles 
{
    private static volatile Battles instance;
    private static object syncRoot = new System.Object();

    public static Battles Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Battles();
                    }
                }
            }
            return instance;
        }
    }

    private List<BigBattle> mStoryList = new List<BigBattle>();
    private List<BigBattle> mActivityList = new List<BigBattle>();
}
