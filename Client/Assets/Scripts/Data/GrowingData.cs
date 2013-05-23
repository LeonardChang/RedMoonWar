using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrowingData
{
    private int mID = 0;
    private string mName = "";

    private float mBaseHP = 100; // ����HP
    private float mBaseMP = 100; // ����MP
    private float mBaseAtk = 100; // ����ATK
    private float mBaseDef = 100; // ����DEF
    private float mBaseSpd = 100; // ����SPD

    private float mGrowHP = 10; // HP�ɳ�
    private float mGrowMP = 0; // MP�ɳ�
    private float mGrowAtk = 5; // ATK�ɳ�
    private float mGrowDef = 2; // DEF�ɳ�
    private float mGrowSpd = 1; // SPD�ɳ�

    public GrowingData(string _init)
    {
        Initialize(_init);
    }

    /// <summary>
    /// ID
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
        return Mathf.FloorToInt(mBaseHP + mGrowHP * (_level - 1));
    }

    /// <summary>
    /// ��ȡĳ�ȼ���MP
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetMP(int _level)
    {
        return Mathf.FloorToInt(mBaseMP + mGrowMP * (_level - 1));
    }

    /// <summary>
    /// ��ȡĳ�ȼ��Ĺ�����
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetATK(int _level)
    {
        return Mathf.FloorToInt(mBaseAtk + mGrowAtk * (_level - 1));
    }

    /// <summary>
    /// ��ȡĳ�ȼ��ķ�����
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetDEF(int _level)
    {
        return Mathf.FloorToInt(mBaseDef + mGrowDef * (_level - 1));
    }

    /// <summary>
    /// ��ȡĳ�ȼ����ٶ�
    /// </summary>
    /// <param name="_level"></param>
    /// <returns></returns>
    public int GetSPD(int _level)
    {
        return Mathf.FloorToInt(mBaseSpd + mGrowSpd * (_level - 1));
    }

    void Initialize(string _str)
    {
        string[] list = _str.Split(',');
        if (list.Length != 12)
        {
            Debug.LogError("Error Card base data: " + _str);
            return;
        }

        try
        {
            mID = int.Parse(list[0]);
            mName = list[1];
            mBaseHP = float.Parse(list[2]);
            mGrowHP = float.Parse(list[3]);
            mBaseMP = float.Parse(list[4]);
            mGrowMP = float.Parse(list[5]);
            mBaseAtk = float.Parse(list[6]);
            mGrowAtk = float.Parse(list[7]);
            mBaseDef = float.Parse(list[8]);
            mGrowDef = float.Parse(list[9]);
            mBaseSpd = float.Parse(list[10]);
            mGrowSpd = float.Parse(list[11]);
        }
        catch (System.FormatException ex)
        {
            Debug.LogError("Error Card base data: " + ex.Message);
        }
    }
}

public class GrowingManager
{
    private static volatile GrowingManager instance;
    private static object syncRoot = new System.Object();

    public static GrowingManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new GrowingManager();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<int, GrowingData> mGrowings = new Dictionary<int, GrowingData>();

    public GrowingManager()
    {
        mGrowings.Clear();

        TextAsset text = Resources.Load("Datas/Growing", typeof(TextAsset)) as TextAsset;
        string[] line = text.text.Split('\n');
        for (int i = 0; i < line.Length; i++ )
        {
            if (i == 0 || line[i] == "\n" || string.IsNullOrEmpty(line[i]))
            {
                continue;
            }

            GrowingData data = new GrowingData(line[i]);
            mGrowings[data.ID] = data;
        }
    }

    public GrowingData GetGrowing(int _id)
    {
        if (!mGrowings.ContainsKey(_id))
        {
            return null;
        }

        return mGrowings[_id];
    }
}