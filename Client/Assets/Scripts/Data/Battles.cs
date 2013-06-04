using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 小关卡
/// </summary>
[System.Serializable]
public class Battle
{
    private int mID;
    private string mName;
    
    private bool mFinish;
    private int mEnergyCost;

    public int ID
    {
        set { mID = value; }
        get { return mID; }
    }

    public string Name
    {
        set { mName = value; }
        get { return mName; }
    }

    public bool Finish
    {
        set { mFinish = value; }
        get { return mFinish; }
    }

    public int EnergyCost
    {
        set { mEnergyCost = value; }
        get { return mEnergyCost; }
    }
}

/// <summary>
/// 大关卡
/// </summary>
[System.Serializable]
public class BigBattle
{
    private int mID;
    private string mName;

    private StageSpecialActivity mActivity = StageSpecialActivity.None;
    private Dictionary<int, Battle> mBattleList = new Dictionary<int, Battle>();

    public int ID
    {
        set { mID = value; }
        get { return mID; }
    }

    public string Name
    {
        set { mName = value; }
        get { return mName; }
    }

    public StageSpecialActivity Activity
    {
        set { mActivity = value; }
        get { return mActivity; }
    }

    public Dictionary<int, Battle> BattleList
    {
        get
        {
            return mBattleList;
        }
    }
}

/// <summary>
/// 战场数据
/// </summary>
[System.Serializable]
public class Battles 
{
    private static volatile Battles instance;
    private static object syncRoot = new System.Object();

    public static Battles Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Battles();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<int, BigBattle> mStoryList = new Dictionary<int, BigBattle>();
    private Dictionary<int, BigBattle> mActivityList = new Dictionary<int, BigBattle>();

    public Battles()
    {
        mStoryList.Clear();
        mActivityList.Clear();

        try
        {
            TextAsset text = Resources.Load("Datas/Story", typeof(TextAsset)) as TextAsset;
            string[] line = text.text.Split('\n');
            for (int i = 0; i < line.Length; i++)
            {
                if (i == 0 || line[i] == "\n" || string.IsNullOrEmpty(line[i]))
                {
                    continue;
                }

                string[] substring = line[i].Split('\t');
                int id = int.Parse(substring[0]);
                string name = substring[1];
                int belongid = int.Parse(substring[2]);
                int energy = int.Parse(substring[3]);
                int stageConfig = int.Parse(substring[4]);
                int reward = int.Parse(substring[5]);

                if (belongid == 0)
                {
                    BigBattle bigstage = mStoryList.ContainsKey(id) ? mStoryList[id] : new BigBattle();
                    bigstage.ID = id;
                    bigstage.Name = name;
                    bigstage.Activity = StageSpecialActivity.None;

                    mStoryList[id] = bigstage;
                }
                else
                {
                    Battle stage = new Battle();
                    stage.ID = id;
                    stage.Name = name;
                    stage.EnergyCost = energy;
                    stage.Finish = false;

                    if (!mStoryList.ContainsKey(belongid))
                    {
                        BigBattle bigstage = new BigBattle();
                        bigstage.ID = belongid;
                        mStoryList[belongid] = bigstage;
                    }

                    mStoryList[belongid].BattleList[id] = stage;
                }
            }
        }
        catch (System.FormatException ex)
        {
            Debug.LogError("Error Story data: " + ex.Message);
        }
    }
}
