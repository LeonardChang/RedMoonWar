using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 卡牌的固有数据
/// </summary>
public class CardBaseData 
{
    private int mID = 0; // 卡片索引ID
    private string mName = ""; // 卡片的名字
    private int mGrowingType = 0;

    private string mCardSprite = ""; // 卡片对应的sprite名字
    private int mStarCount = 1; // 星星数
    private int mEquipCost = 1; // 装备COST

    private int mNormalAttackSkillID = -1; // 普通攻击技能ID
    private int mSkillID = -1; // 技能ID
    private int mLeaderSkillID = -1; // 主将技能ID

    private int mMaxLevel = 99; // 最大等级

    private int mBaseEatExp = 100; // 吃掉获得经验基础值
 
    private int mSellPrice = 100; // 卖掉获得金币基础值

    private GrowType mEXPGrowingType = GrowType.TypeA; // 经验曲线类型
    private ElementType mElementType = ElementType.Fire; // 属性
    
    private int mEvolution = 0; // 可进化目标
    private List<int> mEvolutionMaterial = new List<int>(); // 进化所需材料

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
    /// 成长类型
    /// </summary>
    public int GrowingType
    {
        get
        {
            return mGrowingType;
        }
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
        return mBaseEatExp * _level;
    }

    /// <summary>
    /// 获取某等级的食用价格
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetEatCost(int _level, int _count)
    {
        return _level * _count * 100;
    }

    /// <summary>
    /// 获取某等级的出售价格
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int SellPrice
    {
        get
        {
            return mSellPrice;
        }
    }

    /// <summary>
    /// 成长类型
    /// </summary>
    public GrowType Grow
    {
        get { return mEXPGrowingType; }
    }

    /// <summary>
    /// 属性
    /// </summary>
    public ElementType Element
    {
        get { return mElementType; }
    }

    /// <summary>
    /// 进化目标，0为无法进化
    /// </summary>
    public int Evolution
    {
        get
        {
            return mEvolution;
        }
    }

    /// <summary>
    /// 进化需要的材料，无法进化时为空
    /// </summary>
    public int[] EvolutionMaterials
    {
        get
        {
            return mEvolutionMaterial.ToArray();
        }
    }

    void Initialize(string _str)
    {       
        string[] list = _str.Split('\t');
        if (list.Length != 16)
        {
            Debug.LogError("Error Card base data: " + _str);
            return;
        }

        try
        {
            string trimstr = " \t\r\n\f\",";

            mID = int.Parse(list[0]);
            mName = list[1].Trim(trimstr.ToCharArray());
            mElementType = (ElementType)int.Parse(list[2]);
            mGrowingType = int.Parse(list[3]);
            mCardSprite = list[4].Trim(trimstr.ToCharArray());
            mStarCount = int.Parse(list[5]);
            mEquipCost = int.Parse(list[6]);
            mSkillID = int.Parse(list[7]);
            mLeaderSkillID = int.Parse(list[8]);
            mNormalAttackSkillID = int.Parse(list[9]);
            mMaxLevel = int.Parse(list[10]);
            mEXPGrowingType = (GrowType)int.Parse(list[11]);
            mBaseEatExp = int.Parse(list[12]);
            mSellPrice = int.Parse(list[13]);
            mEvolution = int.Parse(list[14]);

            mEvolutionMaterial.Clear();
            if (!string.IsNullOrEmpty(list[15].Trim(trimstr.ToCharArray())))
            {
                string[] materials = list[15].Split(',');
                for (int i = 0; i < materials.Length; i++)
                {
                    mEvolutionMaterial.Add( int.Parse( materials[i].Trim( trimstr.ToCharArray() ) ) );
                }
            }
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

    private Dictionary<int, CardBaseData> mCardDatas = new Dictionary<int, CardBaseData>(); // 卡片队列

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