using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 主动技能数据
/// </summary>
public class SkillData 
{
    private int mID = 0; // 技能的索引ID

    private string mName = ""; // 技能的名称
    private string mDescription = ""; // 技能的描述
    private float mMultiplyDamage = 1; // 技能的攻击力倍率
    private int mFixedDamage = 0; // 技能造成的固定伤害
    private int mRange = 1; // 释放范围
    private int mCount = 1; // 目标数量
    private int mAddBuff = -1; // 附加的Buff
    private int mManaCost = 10; // 需花费的mana
    private int mManaCostGrow = -1; // 需花费的mana的成长
    private int mHatred = 100; // 增加的仇恨度

    private int mMaxLevel = 6; // 最大等级

    private AttackTargetType mTargetPhase = AttackTargetType.Ememy;
    private AttackAnimType mAttackAnim = AttackAnimType.NormalAttack;
    private FindTargetConditionType mSearchType = FindTargetConditionType.Random;

    public SkillData(string _init)
    {
        Initialize(_init);
    }

    /// <summary>
    /// 技能ID
    /// </summary>
    public int ID
    {
        get
        {
            return mID;
        }
    }

    /// <summary>
    /// 技能名称
    /// </summary>
    public string Name
    {
        get
        {
            return mName;
        }
    }

    /// <summary>
    /// 技能描述
    /// </summary>
    public string Description
    {
        get
        {
            return mDescription;
        }
    }

    /// <summary>
    /// 攻击力伤害倍率
    /// </summary>
    public float MultiplyDamage
    {
        get
        {
            return mMultiplyDamage;
        }
    }

    /// <summary>
    /// 固定伤害
    /// </summary>
    public int FixedDamage
    {
        get
        {
            return mFixedDamage;
        }
    }

    /// <summary>
    /// 攻击距离
    /// </summary>
    public int Range
    {
        get
        {
            return mRange;
        }
    }

    /// <summary>
    /// 攻击目标数
    /// </summary>
    public int Count
    {
        get
        {
            return mCount;
        }
    }

    /// <summary>
    /// 附加Buff
    /// </summary>
    public int AddBuff
    {
        get
        {
            return mAddBuff;
        }
    }

    /// <summary>
    /// Mana消耗
    /// </summary>
    public int GetManaCost(int _level)
    {
        return mManaCost + mManaCostGrow * (_level - 1);
    }

    /// <summary>
    /// 技能产生的仇恨
    /// </summary>
    public int Hatred
    {
        get
        {
            return mHatred;
        }
    }

    /// <summary>
    /// 目标势力
    /// </summary>
    public AttackTargetType TargetPhase
    {
        get 
        {
            return mTargetPhase;
        }
    }

    /// <summary>
    /// 动画形式
    /// </summary>
    public AttackAnimType AttackAnim
    {
        get
        {
            return mAttackAnim;
        }
    }

    /// <summary>
    /// 搜索目标的方式
    /// </summary>
    public FindTargetConditionType SearchType
    {
        get
        {
            return mSearchType;
        }
    }

    /// <summary>
    /// 最大等级
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
        string[] list = _str.Split('\t');
        if (list.Length < 15)
        {
            Debug.LogError("Error Skill data: " + _str);
            return;
        }

        try
        {
            mID = int.Parse(list[0]);
            mName = list[1];
            mDescription = list[2];
            mMultiplyDamage = float.Parse(list[3]);
            mFixedDamage = int.Parse(list[4]);
            mRange = int.Parse(list[5]);
            mCount = int.Parse(list[6]);
            mAddBuff = int.Parse(list[7]);
            mManaCost = int.Parse(list[8]);
            mManaCostGrow = int.Parse(list[9]);
            mHatred = int.Parse(list[10]);
            mMaxLevel = int.Parse(list[11]);
            mTargetPhase = (AttackTargetType)int.Parse(list[12]);
            mAttackAnim = (AttackAnimType)int.Parse(list[13]);
            mSearchType = (FindTargetConditionType)int.Parse(list[14]);
        }
        catch (System.FormatException ex)
        {
            Debug.LogError("Error Skill data: " + ex.Message);
        }
    }
}

/// <summary>
/// 主动技能管理器
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
    /// 获取技能数据
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