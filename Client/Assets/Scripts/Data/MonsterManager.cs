using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 怪物字典
/// </summary>
public class MonsterManager
{
    private static volatile MonsterManager instance;
    private static object syncRoot = new System.Object();

    public static MonsterManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new MonsterManager();
                    }
                }
            }
            return instance;
        }
    }

    public MonsterManager()
    {
        Initialize();
    }

    public void Initialize()
    {
        mData.Clear();

        TextAsset text = Resources.Load("Datas/Monster", typeof(TextAsset)) as TextAsset;
        string[] line = text.text.Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            if (i < 2 || line[i] == "\n" || string.IsNullOrEmpty(line[i]))
            {
                continue;
            }

            string[] lineValue = line[i].Split('\t');

            int id = int.Parse(lineValue[0]);
            string name = lineValue[1];
            string profile = lineValue[2];
            string img = lineValue[3];
            int hp = int.Parse(lineValue[4]);
            int mp = int.Parse(lineValue[5]);
            int speed = int.Parse(lineValue[6]);
            int attack = int.Parse(lineValue[7]);
            int defence = int.Parse(lineValue[8]);
            int normalskill = int.Parse(lineValue[9]);
            int skill = int.Parse(lineValue[10]);
            int leaderskill = int.Parse(lineValue[11]);
            int level = int.Parse(lineValue[12]);
            int ai = int.Parse(lineValue[13]);
            int price = int.Parse(lineValue[14]);

            sMonsterData data = new sMonsterData();
            data.id = id;
            data.name = name;
            data.profile = profile;
            data.img = img;
            data.ai = ai;
            data.hp = hp;
            data.mp = mp;
            data.speed = speed;
            data.attack = attack;
            data.defence = defence;
            data.normalskill = normalskill;
            data.skill = skill;
            data.leaderskill = leaderskill;
            data.level = level;
            data.price = price;
            mData[data.id] = data;
        }
    }

    private Dictionary<int, sMonsterData> mData = new Dictionary<int, sMonsterData>();
    public void Initialize(sMonsterList _list)
    {
        mData.Clear();
        foreach (sMonsterData data in _list.monster)
        {
            mData[data.id] = data;
        }
    }

    /// <summary>
    /// 获取怪物数据
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public sMonsterData GetMonsterData(int _id)
    {
        return mData.ContainsKey(_id) ? mData[_id] : null;
    }
}

public class sMonsterGroupData
{
    public int id;
    public int group;
    public int x;
    public int y;
    public int monster;
    public int rate;
    public int card_drop;
    public int money_drop;
}

public class MonsterGroupManager
{
    private static volatile MonsterGroupManager instance;
    private static object syncRoot = new System.Object();

    public static MonsterGroupManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new MonsterGroupManager();
                    }
                }
            }
            return instance;
        }
    }

    public MonsterGroupManager()
    {
        Initialize();
    }

    public void Initialize()
    {
        mData.Clear();

        TextAsset text = Resources.Load("Datas/MonsterGroup", typeof(TextAsset)) as TextAsset;
        string[] line = text.text.Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            if (i < 2 || line[i] == "\n" || string.IsNullOrEmpty(line[i]))
            {
                continue;
            }

            string[] lineValue = line[i].Split('\t');

            sMonsterGroupData data = new sMonsterGroupData();

            data.id = int.Parse(lineValue[0]);
            data.group = int.Parse(lineValue[1]);
            data.x = int.Parse(lineValue[2]);
            data.y = int.Parse(lineValue[3]);
            data.monster = int.Parse(lineValue[4]);
            data.rate = int.Parse(lineValue[5]);
            data.card_drop = int.Parse(lineValue[6]);
            data.money_drop = int.Parse(lineValue[7]);

            mData[data.id] = data;
        }
    }

    private Dictionary<int, sMonsterGroupData> mData = new Dictionary<int, sMonsterGroupData>();

    public IEnumerable<sMonsterGroupData> GetMonsterGroupData(int _groupId)
    {
        foreach (int key in mData.Keys)
        {
            if (mData[key].group == _groupId)
            {
                yield return mData[key];
            }
        }
    }
}