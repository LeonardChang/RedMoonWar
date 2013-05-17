using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ��������
/// </summary>
[System.Serializable]
public class Reward
{
    private DropRewardType mDropReward;
}

/// <summary>
/// ս����������
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

    private Dictionary<System.Int64, CharacterData> mCharacters = new Dictionary<System.Int64, CharacterData>(); // �ؿ��е����н�ɫ

    private List<FormationData> mFormation = new List<FormationData>(); // �ҷ���ɫ

    private List<FormationData> mEnemy = new List<FormationData>(); // �з���ɫ
    private Dictionary<System.Int64, DropRewardType> mDropRewards = new Dictionary<System.Int64, DropRewardType>(); // �з�������Ʒ�����б�
}
