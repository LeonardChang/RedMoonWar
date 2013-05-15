using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 角色数据
/// </summary>
public class CharacterData 
{
    private int mID = 0;

    private int mMaxHP = 100;
    private int mMaxMP = 100;
    private int mAtk = 100;
    private int mDef = 100;
    private int mSpd = 100;

    private int mCardID = 0;

    private int mLevel = 1;
    private int mSkillLevel = 1;

    /// <summary>
    /// 卡片ID
    /// </summary>
    public int ID
    {
        get { return mID; }
    }

    /// <summary>
    /// 最大HP
    /// </summary>
    public int MaxHP
    {
        get { return mMaxHP; }
    }

    /// <summary>
    /// 最大MP
    /// </summary>
    public int MaxMP
    {
        get { return mMaxMP; }
    }

    /// <summary>
    /// 攻击力
    /// </summary>
    public int Atk
    {
        get { return mAtk; }
    }

    /// <summary>
    /// 防御力
    /// </summary>
    public int Def
    {
        get { return mDef; }
    }

    /// <summary>
    /// 速度
    /// </summary>
    public int Spd
    {
        get { return mSpd; }
    }

    /// <summary>
    /// 对应的卡片ID
    /// </summary>
    public int CardID
    {
        get { return mCardID; }
    }

    /// <summary>
    /// 当前等级
    /// </summary>
    public int Level
    {
        get { return mLevel; }
    }
    
    /// <summary>
    /// 当前技能等级
    /// </summary>
    public int SkillLevel
    {
        get { return mSkillLevel; }
    }
}

/// <summary>
/// 角色管理器
/// </summary>
public class CharacterManager
{
    private static volatile CharacterManager instance;
    private static object syncRoot = new System.Object();

    public static CharacterManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new CharacterManager();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<int, CharacterData> mCharacterDatas = new Dictionary<int, CharacterData>();

    public CharacterManager()
    {

    }

    /// <summary>
    /// 获取角色数据
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public CharacterData GetCharacter(int _id)
    {
        if (!mCharacterDatas.ContainsKey(_id))
        {
            return null;
        }

        return mCharacterDatas[_id];
    }
}