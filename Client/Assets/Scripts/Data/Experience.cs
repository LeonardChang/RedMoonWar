using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Experience
{
    private static volatile Experience instance;
    private static object syncRoot = new System.Object();

    public static Experience Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Experience();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<int, int> mAType = new Dictionary<int, int>();
    private Dictionary<int, int> mBType = new Dictionary<int, int>();
    private Dictionary<int, int> mCType = new Dictionary<int, int>();
    private Dictionary<int, int> mPlayer = new Dictionary<int, int>();

    private Dictionary<int, int> mCost = new Dictionary<int, int>();
    private Dictionary<int, int> mEnergyMax = new Dictionary<int, int>();

    private Dictionary<int, double> mEnergyInc = new Dictionary<int, double>();

    public Experience()
    {
        mAType.Clear();
        mBType.Clear();
        mCType.Clear();
        mPlayer.Clear();
        mCost.Clear();
        mEnergyMax.Clear();
        mEnergyInc.Clear();

        TextAsset text = Resources.Load("Datas/Experience", typeof(TextAsset)) as TextAsset;
        string[] line = text.text.Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            if (i < 2 || line[i] == "\n" || string.IsNullOrEmpty(line[i]))
            {
                continue;
            }
            
            string[] strlist = line[i].Split('\t');
            int level = int.Parse(strlist[0]);
            int a = int.Parse(strlist[1]);
            int b = int.Parse(strlist[2]);
            int c = int.Parse(strlist[3]);
            int p = int.Parse(strlist[4]);
            int cost = int.Parse(strlist[5]);
            int energy = int.Parse(strlist[6]);

            mAType[level] = a;
            mBType[level] = b;
            mCType[level] = c;
            mPlayer[level] = p;
            mCost[level] = cost;
            mEnergyMax[level] = energy;
            mEnergyInc[level] = 0.0017;
        }
    }

    public void ResetPlayerData(sLevelList _levelList)
    {
        mPlayer.Clear();
        mCost.Clear();
        mEnergyMax.Clear();
        mEnergyInc.Clear();

        foreach (sLevelData level in _levelList.level)
        {
            int id = level.id;
            mPlayer[id] = level.exp;
            mEnergyInc[id] = double.Parse(level.enegy_inc);
            mEnergyMax[id] = level.enegy_max;
            mCost[id] = level.cost;
        }
    }

    public int GetATypeEXP(int _targetLevel)
    {
        if (!mAType.ContainsKey(_targetLevel))
        {
            return 0;
        }

        return mAType[_targetLevel];
    }

    public int GetBTypeEXP(int _targetLevel)
    {
        if (!mBType.ContainsKey(_targetLevel))
        {
            return 0;
        }

        return mBType[_targetLevel];
    }

    public int GetCTypeEXP(int _targetLevel)
    {
        if (!mCType.ContainsKey(_targetLevel))
        {
            return 0;
        }

        return mCType[_targetLevel];
    }

    public int GetPlayerEXP(int _targetLevel)
    {
        if (!mPlayer.ContainsKey(_targetLevel))
        {
            return 0;
        }

        return mPlayer[_targetLevel];
    }

    public int GetPlayerCost(int _targetLevel)
    {
        if (!mCost.ContainsKey(_targetLevel))
        {
            return 0;
        }

        return mCost[_targetLevel];
    }

    public int GetPlayerEnergyMax(int _targetLevel)
    {
        if (!mEnergyMax.ContainsKey(_targetLevel))
        {
            return 0;
        }

        return mEnergyMax[_targetLevel];
    }

    public double GetPlayerEnergyInc(int _targetLevel)
    {
        if (!mEnergyInc.ContainsKey(_targetLevel))
        {
            return 0;
        }

        return mEnergyInc[_targetLevel];
    }
}
