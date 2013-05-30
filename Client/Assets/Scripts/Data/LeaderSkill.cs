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

    void Initialize(string _str)
    {
        string[] list = _str.Split('\t');
        if (list.Length < 3)
        {
            Debug.LogError("Error LeaderSkill data: " + _str);
            return;
        }

        try
        {
            mID = int.Parse(list[0]);
            mName = list[1];
            mDescription = list[2];
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
}