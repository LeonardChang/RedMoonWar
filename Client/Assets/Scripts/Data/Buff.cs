using UnityEngine;
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
    private int mRoundMin = 3; // buff持续最小回合数
    private int mRoundMax = 3; // buff持续最大回合数
    private string mSpriteName = ""; // 图标名称

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

    public int RoundMin
    {
        get
        {
            return mRoundMin;
        }
    }

    public int RoundMax
    {
        get
        {
            return mRoundMax;
        }
    }

    public string SpriteName
    {
        get
        {
            return mSpriteName;
        }
    }

    void Initialize(string _str)
    {
        string[] list = _str.Split('\t');
        if (list.Length < 6)
        {
            Debug.LogError("Error Buff data: " + _str);
            return;
        }

        try
        {
            string trimstr = " \t\r\n\f";

            mID = int.Parse(list[0]);
            mName = list[1];
            mBuffType = (BuffType)(int.Parse(list[2]));
            mRoundMin = int.Parse(list[3]);
            mRoundMax = int.Parse(list[4]);
            mSpriteName = list[5].Trim(trimstr.ToCharArray()); ;
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
        Initialize();
    }

    public void Initialize()
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

    public void Initialize(string _file)
    {
        mBuffs.Clear();

        string[] line = _file.Split('\n');
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

    public BuffData GetBuff(int _id)
    {
        if (!mBuffs.ContainsKey(_id))
        {
            Debug.LogError("Can't find buff id: " + _id.ToString());
            return null;
        }

        return mBuffs[_id];
    }
}