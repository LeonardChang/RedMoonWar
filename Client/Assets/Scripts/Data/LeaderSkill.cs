using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 队长技能
/// </summary>
public class LeaderSkillData
{
    private int mID = 0; // 技能的索引ID
    private string mName = ""; // 技能的名称
    private string mDescription = ""; // 技能的描述
    
    private ElementType mElement = ElementType.None; // 有效对象的属性
    private int mAddHP = 0; // 每回合额外增加HP
    private int mAddMP = 0; // 每回合额外增加MP
    private float mDamageDown = 1; // 伤害下降
    private float mAtkUp = 1; // 攻击上升
    private float mDefUp = 1; // 防御上升
    private float mSpdUp = 1; // 速度上升
    private float mSpecial = 0; // 特殊值
        
    public LeaderSkillData(string _init)
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

    public ElementType Element
    {
        get
        {
            return mElement;
        }
    }

    public int AddHP
    {
        get
        {
            return mAddHP;
        }
    }

    public int AddMP
    {
        get
        {
            return mAddMP;
        }
    }

    public float DamageDown
    {
        get
        {
            return mDamageDown;
        }
    }

    public float AtkUp
    {
        get
        {
            return mAtkUp;
        }
    }

    public float DefUp
    {
        get
        {
            return mDefUp;
        }
    }

    public float SpdUp
    {
        get
        {
            return mSpdUp;
        }
    }

    public float Special
    {
        get
        {
            return mSpecial;
        }
    }

    void Initialize(string _str)
    {
        string[] list = _str.Split('\t');
        if (list.Length < 11)
        {
            Debug.LogError("Error LeaderSkill data: " + _str);
            return;
        }

        try
        {
            mID = int.Parse(list[0]);
            mName = list[1];
            mDescription = list[2];

            mElement = (ElementType)int.Parse(list[3]);
            mAddHP = int.Parse(list[4]);
            mAddMP = int.Parse(list[5]);
            mDamageDown = float.Parse(list[6]);
            mAtkUp = float.Parse(list[7]);
            mDefUp = float.Parse(list[8]);
            mSpdUp = float.Parse(list[9]);
            mSpecial = float.Parse(list[10]);
        }
        catch (System.FormatException ex)
        {
            Debug.LogError("Error LeaderSkill data: " + ex.Message);
        }
    }
}

/// <summary>
/// 队长技能管理器
/// </summary>
public class LeaderSkillManager
{
    private static volatile LeaderSkillManager instance;
    private static object syncRoot = new System.Object();

    public static LeaderSkillManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new LeaderSkillManager();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<int, LeaderSkillData> mLeaderSkillDatas = new Dictionary<int, LeaderSkillData>();

    public LeaderSkillManager()
    {
        mLeaderSkillDatas.Clear();

        TextAsset text = Resources.Load("Datas/LeaderSkillData", typeof(TextAsset)) as TextAsset;
        string[] line = text.text.Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            if (i == 0 || line[i] == "\n" || string.IsNullOrEmpty(line[i]))
            {
                continue;
            }

            LeaderSkillData data = new LeaderSkillData(line[i]);
            mLeaderSkillDatas[data.ID] = data;
        }
    }

    public LeaderSkillData GetSkill(int _id)
    {
        if (!mLeaderSkillDatas.ContainsKey(_id))
        {
            //Debug.LogError("Can't find leader skill id: " + _id.ToString());
            return null;
        }

        return mLeaderSkillDatas[_id];
    }
}