using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ���ƵĹ�������
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
    /// ��ƬID
    /// </summary>
    public int ID
    {
        get { return mID; }
    }

    /// <summary>
    /// ����
    /// </summary>
    public string Name
    {
        get { return mName; }
    }

    /// <summary>
    /// ��ȡĳ�ȼ���HP
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetHP(int _level)
    {
        return Mathf.FloorToInt(mBaseHP + mGrowHP * _level);
    }

    /// <summary>
    /// ��ȡĳ�ȼ���MP
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetMP(int _level)
    {
        return Mathf.FloorToInt(mBaseMP + mGrowMP * _level);
    }

    /// <summary>
    /// ��ȡĳ�ȼ��Ĺ�����
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetATK(int _level)
    {
        return Mathf.FloorToInt(mBaseAtk + mGrowAtk * _level);
    }

    /// <summary>
    /// ��ȡĳ�ȼ��ķ�����
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetDEF(int _level)
    {
        return Mathf.FloorToInt(mBaseDef + mGrowDef * _level);
    }

    /// <summary>
    /// ��ȡĳ�ȼ����ٶ�
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetSPD(int _level)
    {
        return Mathf.FloorToInt(mBaseSpd + mGrowSpd * _level);
    }

    /// <summary>
    /// ��Ƭ����
    /// </summary>
    public string CardSprite
    {
        get { return mCardSprite; }
    }

    /// <summary>
    /// ����
    /// </summary>
    public int StarCount
    {
        get { return mStarCount; }
    }

    /// <summary>
    /// ���ĵ��쵼��
    /// </summary>
    public int EquipCost
    {
        get { return mEquipCost; }
    }

    /// <summary>
    /// �չ�����ID
    /// </summary>
    public int NormalAttackSkillID
    {
        get { return mNormalAttackSkillID; }
    }

    /// <summary>
    /// ����ID
    /// </summary>
    public int SkillID
    {
        get { return mSkillID; }
    }

    /// <summary>
    /// ������ID
    /// </summary>
    public int LeaderSkillID
    {
        get { return mLeaderSkillID; }
    }

    /// <summary>
    /// ���ȼ�
    /// </summary>
    public int MaxLevel
    {
        get { return mMaxLevel; }
    }

    /// <summary>
    /// ��ȡĳ�ȼ���ʳ�ü۸�
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetEatExp(int _level)
    {
        return Mathf.FloorToInt(mBaseEatExp + mGrowEatExp * _level);
    }

    /// <summary>
    /// ��ȡĳ�ȼ���ʳ�ü۸�
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetEatCost(int _level)
    {
        return Mathf.FloorToInt(mBaseEatCost + mGrowEatCost * _level);
    }

    /// <summary>
    /// ��ȡĳ�ȼ��ĳ��ۼ۸�
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetSellPrice(int _level)
    {
        return Mathf.FloorToInt(mBaseSellPrice + mGrowSellPrice * _level);
    }

    /// <summary>
    /// �ɳ�����
    /// </summary>
    public GrowType Grow
    {
        get { return mGrowType; }
    }

    /// <summary>
    /// ����
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
/// �������ݹ�����
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
    /// ��ȡ��������
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