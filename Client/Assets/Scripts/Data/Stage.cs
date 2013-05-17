using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 奖励数据
/// </summary>
[System.Serializable]
public class Reward
{
    private DropRewardType mDropReward;
}

/// <summary>
/// 战场配置数据
/// </summary>
[System.Serializable]
public class Stage 
{
    private static volatile Stage instance;
    private static object syncRoot = new System.Object();

    public static Stage Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Stage();
                    }
                }
            }
            return instance;
        }
    }

    private int mWidth;
    private int mHeight;
    private int mBackground;

    private Dictionary<System.Int64, CharacterData> mCharacters = new Dictionary<System.Int64, CharacterData>(); // 关卡中的所有角色

    private List<FormationData> mFormation = new List<FormationData>(); // 我方角色

    private List<FormationData> mEnemy = new List<FormationData>(); // 敌方角色
    private Dictionary<System.Int64, DropRewardType> mDropRewards = new Dictionary<System.Int64, DropRewardType>(); // 敌方掉落物品类型列表
}
