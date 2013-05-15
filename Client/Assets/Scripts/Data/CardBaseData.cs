using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GrowType : int
{
    TypeA = 0,
    TypeB,
    TypeC,

    Max,
}

/// <summary>
/// 卡牌的固有数据
/// </summary>
public class CardBaseData 
{
    private int mID = 0;

    private float mBaseHP = 100;
    private float mBaseMP = 100;
    private float mBaseAtk = 100;
    private float mBaseDef = 100;
    private float mBaseSpd = 100;

    private float mGrowHP = 10;
    private float mGrowMP = 0;
    private float mGrowAtk = 5;
    private float mGrowDef = 2;
    private float mGrowSpd = 1;

    private string mCardSprite = "";
    private int mStarCount = 1;
    private int mEquipCost = 1;

    private int mNormalAttackSkillID = -1;
    private int mSkillID = -1;
    private int mLeaderSkillID = -1;

    private int mMaxLevel = 99;
    private int mMaxSkillLevel = 6;

    private GrowType mGrowType = GrowType.TypeA;
    private ElementType mElementType = ElementType.Fire;

    /// <summary>
    /// 卡片ID
    /// </summary>
    public int ID
    {
        get { return mID; }
    }

    /// <summary>
    /// 基础HP
    /// </summary>
    public float BaseHP
    {
        get { return mBaseHP; }
    }

    /// <summary>
    /// 基础MP
    /// </summary>
    public float BaseMP
    {
        get { return mBaseMP; }
    }

    /// <summary>
    /// 基础攻击力
    /// </summary>
    public float BaseAtk
    {
        get { return mBaseAtk; }
    }

    /// <summary>
    /// 基础防御力
    /// </summary>
    public float BaseDef
    {
        get { return mBaseDef; }
    }

    /// <summary>
    /// 基础速度
    /// </summary>
    public float BaseSpd
    {
        get { return mBaseSpd; }
    }

    /// <summary>
    /// HP成长
    /// </summary>
    public float GrowHP
    {
        get { return mGrowHP; }
    }

    /// <summary>
    /// MP成长
    /// </summary>
    public float GrowMP
    {
        get { return mGrowMP; }
    }

    /// <summary>
    /// 攻击力成长
    /// </summary>
    public float GrowAtk
    {
        get { return mGrowAtk; }
    }

    /// <summary>
    /// 防御力成长
    /// </summary>
    public float GrowDef
    {
        get { return mGrowDef; }
    }

    /// <summary>
    /// 速度成长
    /// </summary>
    public float GrowSpd
    {
        get { return mGrowSpd; }
    }

    /// <summary>
    /// 卡片精灵
    /// </summary>
    public string CardSprite
    {
        get { return mCardSprite; }
    }

    /// <summary>
    /// 星数
    /// </summary>
    public int StarCount
    {
        get { return mStarCount; }
    }

    /// <summary>
    /// 消耗的领导力
    /// </summary>
    public int EquipCost
    {
        get { return mEquipCost; }
    }

    /// <summary>
    /// 普攻技能ID
    /// </summary>
    public int NormalAttackSkillID
    {
        get { return mNormalAttackSkillID; }
    }

    /// <summary>
    /// 技能ID
    /// </summary>
    public int SkillID
    {
        get { return mSkillID; }
    }

    /// <summary>
    /// 主将技ID
    /// </summary>
    public int LeaderSkillID
    {
        get { return mLeaderSkillID; }
    }

    /// <summary>
    /// 最大等级
    /// </summary>
    public int MaxLevel
    {
        get { return mMaxLevel; }
    }

    /// <summary>
    /// 最大技能等级
    /// </summary>
    public int MaxSkillLevel
    {
        get { return mMaxSkillLevel; }
    }

    /// <summary>
    /// 成长类型
    /// </summary>
    public GrowType Grow
    {
        get { return mGrowType; }
    }

    /// <summary>
    /// 属性
    /// </summary>
    public ElementType Element
    {
        get { return mElementType; }
    }
}

/// <summary>
/// 卡牌数据管理器
/// </summary>
public class CardManager
{
    private static volatile CardManager instance;
    private static object syncRoot = new System.Object();

    public static CardManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new CardManager();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<int, CardBaseData> mCardDatas = new Dictionary<int, CardBaseData>();

    public CardManager()
    {

    }

    /// <summary>
    /// 获取卡牌数据
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public CardBaseData GetCard(int _id)
    {
        if (!mCardDatas.ContainsKey(_id))
        {
            return null;
        }

        return mCardDatas[_id];
    }
}