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
/// ���ƵĹ�������
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
    /// ��ƬID
    /// </summary>
    public int ID
    {
        get { return mID; }
    }

    /// <summary>
    /// ����HP
    /// </summary>
    public float BaseHP
    {
        get { return mBaseHP; }
    }

    /// <summary>
    /// ����MP
    /// </summary>
    public float BaseMP
    {
        get { return mBaseMP; }
    }

    /// <summary>
    /// ����������
    /// </summary>
    public float BaseAtk
    {
        get { return mBaseAtk; }
    }

    /// <summary>
    /// ����������
    /// </summary>
    public float BaseDef
    {
        get { return mBaseDef; }
    }

    /// <summary>
    /// �����ٶ�
    /// </summary>
    public float BaseSpd
    {
        get { return mBaseSpd; }
    }

    /// <summary>
    /// HP�ɳ�
    /// </summary>
    public float GrowHP
    {
        get { return mGrowHP; }
    }

    /// <summary>
    /// MP�ɳ�
    /// </summary>
    public float GrowMP
    {
        get { return mGrowMP; }
    }

    /// <summary>
    /// �������ɳ�
    /// </summary>
    public float GrowAtk
    {
        get { return mGrowAtk; }
    }

    /// <summary>
    /// �������ɳ�
    /// </summary>
    public float GrowDef
    {
        get { return mGrowDef; }
    }

    /// <summary>
    /// �ٶȳɳ�
    /// </summary>
    public float GrowSpd
    {
        get { return mGrowSpd; }
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
    /// ����ܵȼ�
    /// </summary>
    public int MaxSkillLevel
    {
        get { return mMaxSkillLevel; }
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