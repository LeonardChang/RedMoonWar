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
        if (list.Length != 21)
        {
            Debug.LogError("Error Card base data: " + _str);
            return;
        }

        try
        {
            mID = int.Parse(list[0]);
            mName = list[1];
            mElementType = (ElementType)int.Parse(list[2]);
            mBaseHP = int.Parse(list[3]);
            mGrowHP = int.Parse(list[4]);
            mBaseMP = int.Parse(list[5]);
            mGrowMP = int.Parse(list[6]);
            mBaseAtk = int.Parse(list[7]);
            mGrowAtk = int.Parse(list[8]);
            mBaseDef = int.Parse(list[9]);
            mGrowDef = int.Parse(list[10]);
            mBaseSpd = int.Parse(list[11]);
            mGrowSpd = int.Parse(list[12]);
            mCardSprite = list[13];
            mStarCount = int.Parse(list[14]);
            mEquipCost = int.Parse(list[15]);
            mSkillID = int.Parse(list[16]);
            mLeaderSkillID = int.Parse(list[17]);
            mNormalAttackSkillID = int.Parse(list[18]);
            mMaxLevel = int.Parse(list[19]);
            mGrowType = (GrowType)int.Parse(list[20]);
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