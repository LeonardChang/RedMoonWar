using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AttackAnimType : int
{
    Normal = 0,
    Arrow,
    FireBall,
    IceBall,
    WindBall,
    StoneBall,
    LightBall,
    DarkBall,
    CannonBall,

    HPHealth,
    MPHealth,

    Max,
}

public enum AttackActionType : int
{
    Attack = 0,
    Skill,
    
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

    private AttackTargetType mTargetPhase = AttackTargetType.Ememy;
    private AttackAnimType mAttackAnim = AttackAnimType.Normal;
    private AttackActionType mAttackAction = AttackActionType.Attack;

    public SkillData(int _ID, string _Name, float _MultiplyDamage, int _FixedDamage, int _Range, int _Count, int _AddBuff, int _ManaCost, AttackTargetType _TargetPhase, AttackAnimType _AttackAnim, AttackActionType _AttackAction, int _Hatred)
    {
        mID = _ID;
        mName = _Name;
        mMultiplyDamage = _MultiplyDamage;
        mFixedDamage = _FixedDamage;
        mRange = _Range;
        mCount = _Count;
        mAddBuff = _AddBuff;
        mManaCost = _ManaCost;
        mTargetPhase = _TargetPhase;
        mAttackAnim = _AttackAnim;
        mAttackAction = _AttackAction;
        mHatred = _Hatred;
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
    /// ����ͨ�������Ǽ���
    /// </summary>
    public AttackActionType AttackAction
    {
        get
        {
            return mAttackAction;
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

        int id = 0;

        // 0
        SkillData skill = new SkillData(id, "Skill" + id.ToString("000"), 1, 0, 1, 1, -1, 0, AttackTargetType.Ememy, AttackAnimType.Normal, AttackActionType.Attack, 50);
        mSkillDatas[skill.ID] = skill;

        // 1
        id += 1;
        skill = new SkillData(id, "Skill" + id.ToString("000"), 1, 0, 1, 1, -1, 0, AttackTargetType.Ememy, AttackAnimType.Arrow, AttackActionType.Attack, 50);
        mSkillDatas[skill.ID] = skill;

        // 2
        id += 1;
        skill = new SkillData(id, "Skill" + id.ToString("000"), 1, 0, 1, 1, -1, 0, AttackTargetType.Ememy, AttackAnimType.FireBall, AttackActionType.Attack, 50);
        mSkillDatas[skill.ID] = skill;

        // 3
        id += 1;
        skill = new SkillData(id, "Skill" + id.ToString("000"), 1, 0, 1, 1, -1, 0, AttackTargetType.Ememy, AttackAnimType.IceBall, AttackActionType.Attack, 50);
        mSkillDatas[skill.ID] = skill;

        // 4
        id += 1;
        skill = new SkillData(id, "Skill" + id.ToString("000"), 1, 0, 1, 1, -1, 0, AttackTargetType.Ememy, AttackAnimType.WindBall, AttackActionType.Attack, 50);
        mSkillDatas[skill.ID] = skill;

        // 5
        id += 1;
        skill = new SkillData(id, "Skill" + id.ToString("000"), 1, 0, 1, 1, -1, 0, AttackTargetType.Ememy, AttackAnimType.StoneBall, AttackActionType.Attack, 50);
        mSkillDatas[skill.ID] = skill;

        // 6 
        id += 1;
        skill = new SkillData(id, "Skill" + id.ToString("000"), 1, 0, 1, 1, -1, 0, AttackTargetType.Ememy, AttackAnimType.LightBall, AttackActionType.Attack, 50);
        mSkillDatas[skill.ID] = skill;

        // 7
        id += 1;
        skill = new SkillData(id, "Skill" + id.ToString("000"), 1, 0, 1, 1, -1, 0, AttackTargetType.Ememy, AttackAnimType.DarkBall, AttackActionType.Attack, 50);
        mSkillDatas[skill.ID] = skill;

        // 8
        id += 1;
        skill = new SkillData(id, "Skill" + id.ToString("000"), 1, 0, 1, 1, -1, 0, AttackTargetType.Ememy, AttackAnimType.CannonBall, AttackActionType.Attack, 50);
        mSkillDatas[skill.ID] = skill;

        // 9
        id += 1;
        skill = new SkillData(id, "Skill" + id.ToString("000"), 0, 20, 1, 1, -1, 30, AttackTargetType.Team, AttackAnimType.HPHealth, AttackActionType.Skill, 50);
        mSkillDatas[skill.ID] = skill;
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