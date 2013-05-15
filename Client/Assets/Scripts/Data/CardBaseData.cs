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