using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AttackAnimType : int
{
    Normal = 0,
    Arrow = 1,
    FireBall = 2,
    IceBall = 3,
    WindBall = 4,
    StoneBall = 5,
    LightBall = 6,
    DarkBall = 7,
    CannonBall = 8,

    HPHealth = 9,
    MPHealth = 10,

    Max,
}

public enum AttackTargetType : int
{
    Self = 0,
    Team,
    Ememy,
    All,

    Max,
}

/// <summary>
/// ������������
/// </summary>
public class SkillData 
{
    private int mID = 0;

    private string mName = "";
    private float mMultiplyDamage = 1;
    private int mFixedDamage = 0;
    private int mRange = 1;
    private int mCount = 1;
    private int mAddBuff = -1;
    private int mManaCost = 10;
    private int mHatred = 100;

    private int mMaxLevel = 6;

    private AttackTargetType mTargetPhase = AttackTargetType.Ememy;
    private AttackAnimType mAttackAnim = AttackAnimType.Normal;

    public SkillData(string _init)
    {
        Initialize(_init);
    }

    /// <summary>
    /// ����ID
    /// </summary>
    public int ID
    {
        get
        {
            return mID;
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    public string Name
    {
        get
        {
            return mName;
        }
    }

    /// <summary>
    /// �������˺�����
    /// </summary>
    public float MultiplyDamage
    {
        get
        {
            return mMultiplyDamage;
        }
    }

    /// <summary>
    /// �̶��˺�
    /// </summary>
    public int FixedDamage
    {
        get
        {
            return mFixedDamage;
        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    public int Range
    {
        get
        {
            return mRange;
        }
    }

    /// <summary>
    /// ����Ŀ����
    /// </summary>
    public int Count
    {
        get
        {
            return mCount;
        }
    }

    /// <summary>
    /// ����Buff
    /// </summary>
    public int AddBuff
    {
        get
        {
            return mAddBuff;
        }
    }

    /// <summary>
    /// Mana����
    /// </summary>
    public int ManaCost
    {
        get
        {
            return mManaCost;
        }
    }

    /// <summary>
    /// ���ܲ����ĳ��
    /// </summary>
    public int Hatred
    {
        get
        {
            return mHatred;
        }
    }

    /// <summary>
    /// Ŀ������
    /// </summary>
    public AttackTargetType TargetPhase
    {
        get 
        {
            return mTargetPhase;
        }
    }

    /// <summary>
    /// ������ʽ
    /// </summary>
    public AttackAnimType AttackAnim
    {
        get
        {
            return mAttackAnim;
        }
    }

    /// <summary>
    /// ���ȼ�
    /// </summary>
    public int MaxLevel
    {
        get
        {
            return mMaxLevel;
        }
    }

    void Initialize(string _str)
    {
        string[] list = _str.Split(',');
        if (list.Length != 12)
        {
            Debug.LogError("Error Skill data: " + _str);
            return;
        }

        try
        {
            mID = int.Parse(list[0]);
            mName = list[1];
            mMultiplyDamage = float.Parse(list[2]);
            mFixedDamage = int.Parse(list[3]);
            mRange = int.Parse(list[4]);
            mCount = int.Parse(list[5]);
            mAddBuff = int.Parse(list[6]);
            mManaCost = int.Parse(list[7]);
            mHatred = int.Parse(list[8]);
            mMaxLevel = int.Parse(list[9]);
            mTargetPhase = (AttackTargetType)int.Parse(list[10]);
            mAttackAnim = (AttackAnimType)int.Parse(list[11]);
        }
        catch (System.FormatException ex)
        {
            Debug.LogError("Error Skill data: " + ex.Message);
        }
    }
}

/// <summary>
/// �������ܹ�����
/// </summary>
public class SkillManager
{
    private static volatile SkillManager instance;
    private static object syncRoot = new System.Object();

    public static SkillManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new SkillManager();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<int, SkillData> mSkillDatas = new Dictionary<int, SkillData>();

    public SkillManager()
    {
        mSkillDatas.Clear();

        TextAsset text = Resources.Load("Datas/SkillData", typeof(TextAsset)) as TextAsset;
        string[] line = text.text.Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            if (i == 0 || line[i] == "\n" || string.IsNullOrEmpty(line[i]))
            {
                continue;
            }

            SkillData data = new SkillData(line[i]);
            mSkillDatas[data.ID] = data;
        }
    }

    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public SkillData GetSkill(int _id)
    {
        if (!mSkillDatas.ContainsKey(_id))
        {
            return null;
        }

        return mSkillDatas[_id];
    }
}