﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Buff
/// </summary>
public class BuffData
{
    private int mID = 0; // 索引ID
    private string mName = ""; // 名称
    private BuffType mBuffType = BuffType.Bad;

    public BuffData(string _init)
    {
        Initialize(_init);
    }

    /// <summary>
    /// ID
    /// </summary>
    public int ID
    {
        get
        {
            return mID;
        }
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
        get
        {
            return mName;
        }
    }

    public BuffType BuffType
    {
        get
        {
            return mBuffType;
        }
    }

    void Initialize(string _str)
    {
        string[] list = _str.Split('\t');
        if (list.Length < 3)
        {
            Debug.LogError("Error Buff data: " + _str);
            return;
        }

        try
        {
            mID = int.Parse(list[0]);
            mName = list[1];
            mBuffType = (BuffType)(int.Parse(list[2]));
        }
        catch (System.FormatException ex)
        {
            Debug.LogError("Error Buff data: " + ex.Message);
        }
    }
}

/// <summary>
/// Buff管理器
/// </summary>
public class BuffManager
{
    private static volatile BuffManager instance;
    private static object syncRoot = new System.Object();

    public static BuffManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new BuffManager();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<int, BuffData> mBuffs = new Dictionary<int, BuffData>();

    public BuffManager()
    {
        mBuffs.Clear();

        TextAsset text = Resources.Load("Datas/Buff", typeof(TextAsset)) as TextAsset;
        string[] line = text.text.Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            if (i == 0 || line[i] == "\n" || string.IsNullOrEmpty(line[i]))
            {
                continue;
            }

            BuffData data = new BuffData(line[i]);
            mBuffs[data.ID] = data;
        }
    }

    public BuffData GetBuff(int _index)
    {
        if (!mBuffs.ContainsKey(_index))
        {
            return null;
        }

        return mBuffs[_index];
    }
}