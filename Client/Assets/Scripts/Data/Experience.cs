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

    public Experience()
    {
        mAType.Clear();
        mBType.Clear();
        mCType.Clear();
        mPlayer.Clear();

        TextAsset text = Resources.Load("Datas/Experience", typeof(TextAsset)) as TextAsset;
        string[] line = text.text.Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            if (i == 0 || line[i] == "\n" || string.IsNullOrEmpty(line[i]))
            {
                continue;
            }

            string[] strlist = line[i].Split('\t');
            int level = int.Parse(strlist[0]);
            int a = int.Parse(strlist[1]);
            int b = int.Parse(strlist[2]);
            int c = int.Parse(strlist[3]);
            int p = int.Parse(strlist[4]);

            mAType[level] = a;
            mBType[level] = b;
            mCType[level] = c;
            mPlayer[level] = p;
        }
    }

    public int GetATypeEXP(int _currentLevel)
    {
        if (!mAType.ContainsKey(_currentLevel))
        {
            return 0;
        }

        return mAType[_currentLevel];
    }

    public int GetBTypeEXP(int _currentLevel)
    {
        if (!mBType.ContainsKey(_currentLevel))
        {
            return 0;
        }

        return mBType[_currentLevel];
    }

    public int GetCTypeEXP(int _currentLevel)
    {
        if (!mCType.ContainsKey(_currentLevel))
        {
            return 0;
        }

        return mCType[_currentLevel];
    }

    public int GetPlayerEXP(int _currentLevel)
    {
        if (!mPlayer.ContainsKey(_currentLevel))
        {
            return 0;
        }

        return mPlayer[_currentLevel];
    }
}
