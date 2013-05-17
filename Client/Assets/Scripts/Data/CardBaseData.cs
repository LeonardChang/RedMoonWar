using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 卡牌的固有数据
/// </summary>
public class CardBaseData 
{
    private int mID = 0;

    private string mName = "";

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

    private float mBaseEatExp = 100;
    private float mGrowEatExp = 1;
    private float mBaseEatCost = 100;
    private float mGrowEatCost = 1;
    private float mBaseSellPrice = 100;
    private float mGrowSellPrice = 1;

    private GrowType mGrowType = GrowType.TypeA;
    private ElementType mElementType = ElementType.Fire;

    public CardBaseData(string _init)
    {
        Initialize(_init);
    }

    /// <summary>
    /// 卡片ID
    /// </summary>
    public int ID
    {
        get { return mID; }
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
        get { return mName; }
    }

    /// <summary>
    /// 获取某等级的HP
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetHP(int _level)
    {
        return Mathf.FloorToInt(mBaseHP + mGrowHP * _level);
    }

    /// <summary>
    /// 获取某等级的MP
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetMP(int _level)
    {
        return Mathf.FloorToInt(mBaseMP + mGrowMP * _level);
    }

    /// <summary>
    /// 获取某等级的攻击力
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetATK(int _level)
    {
        return Mathf.FloorToInt(mBaseAtk + mGrowAtk * _level);
    }

    /// <summary>
    /// 获取某等级的防御力
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetDEF(int _level)
    {
        return Mathf.FloorToInt(mBaseDef + mGrowDef * _level);
    }

    /// <summary>
    /// 获取某等级的速度
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetSPD(int _level)
    {
        return Mathf.FloorToInt(mBaseSpd + mGrowSpd * _level);
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
    /// 获取某等级的食用价格
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetEatExp(int _level)
    {
        return Mathf.FloorToInt(mBaseEatExp + mGrowEatExp * _level);
    }

    /// <summary>
    /// 获取某等级的食用价格
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetEatCost(int _level)
    {
        return Mathf.FloorToInt(mBaseEatCost + mGrowEatCost * _level);
    }

    /// <summary>
    /// 获取某等级的出售价格
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetSellPrice(int _level)
    {
        return Mathf.FloorToInt(mBaseSellPrice + mGrowSellPrice * _level);
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

    void Initialize(string _str)
    {       
        string[] list = _str.Split(',');
        if (list.Length != 27)
        {
            Debug.LogError("Error Card base data: " + _str);
            return;
        }

        try
        {
            mID = int.Parse(list[0]);
            mName = list[1];
            mElementType = (ElementType)int.Parse(list[2]);
            mBaseHP = float.Parse(list[3]);
            mGrowHP = float.Parse(list[4]);
            mBaseMP = float.Parse(list[5]);
            mGrowMP = float.Parse(list[6]);
            mBaseAtk = float.Parse(list[7]);
            mGrowAtk = float.Parse(list[8]);
            mBaseDef = float.Parse(list[9]);
            mGrowDef = float.Parse(list[10]);
            mBaseSpd = float.Parse(list[11]);
            mGrowSpd = float.Parse(list[12]);
            mCardSprite = list[13];
            mStarCount = int.Parse(list[14]);
            mEquipCost = int.Parse(list[15]);
            mSkillID = int.Parse(list[16]);
            mLeaderSkillID = int.Parse(list[17]);
            mNormalAttackSkillID = int.Parse(list[18]);
            mMaxLevel = int.Parse(list[19]);
            mGrowType = (GrowType)int.Parse(list[20]);
            mBaseEatExp = float.Parse(list[21]);
            mGrowEatExp = float.Parse(list[22]);
            mBaseEatCost = float.Parse(list[23]);
            mGrowEatCost = float.Parse(list[24]);
            mBaseSellPrice = float.Parse(list[25]);
            mGrowSellPrice = float.Parse(list[26]);
        }
        catch (System.FormatException ex)
        {
            Debug.LogError("Error Card base data: " + ex.Message);
        }
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
        mCardDatas.Clear();

        TextAsset text = Resources.Load("Datas/CardBaseData", typeof(TextAsset)) as TextAsset;
        string[] line = text.text.Split('\n');
        for (int i = 0; i < line.Length; i++ )
        {
            if (i == 0 || line[i] == "\n" || string.IsNullOrEmpty(line[i]))
            {
                continue;
            }

            CardBaseData data = new CardBaseData(line[i]);
            mCardDatas[data.ID] = data;
        }
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