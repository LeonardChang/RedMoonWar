using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 角色数据
/// </summary>
[System.Serializable]
public class CharacterData 
{
    protected System.Int64 mID = 0;

    protected int mMaxHP = 100;
    protected int mMaxMP = 100;
    protected int mAtk = 100;
    protected int mDef = 100;
    protected int mSpd = 100;
    protected int mCardID = 0;
    protected int mLevel = 1;
    protected int mSkillLevel = 1;
    protected int mExp = 0;
    protected System.DateTime mGetDate = System.DateTime.Now;

    /// <summary>
    /// 卡片ID
    /// </summary>
    public System.Int64 ID
    {
        get { return mID; }
        set { mID = value; }
    }

    /// <summary>
    /// 最大HP
    /// </summary>
    public int MaxHP
    {
        get { return mMaxHP; }
        set { mMaxHP = value; }
    }

    /// <summary>
    /// 最大MP
    /// </summary>
    public int MaxMP
    {
        get { return mMaxMP; }
        set { mMaxMP = value; }
    }

    /// <summary>
    /// 攻击力
    /// </summary>
    public int Atk
    {
        get { return mAtk; }
        set { mAtk = value; }
    }

    /// <summary>
    /// 防御力
    /// </summary>
    public int Def
    {
        get { return mDef; }
        set { mDef = value; }
    }

    /// <summary>
    /// 速度
    /// </summary>
    public int Spd
    {
        get { return mSpd; }
        set { mSpd = value; }
    }

    /// <summary>
    /// 对应的卡片ID
    /// </summary>
    public int CardID
    {
        get { return mCardID; }
        set { mCardID = value; }
    }

    /// <summary>
    /// 当前等级
    /// </summary>
    public int Level
    {
        get { return mLevel; }
        set { mLevel = value; }
    }
    
    /// <summary>
    /// 当前技能等级
    /// </summary>
    public int SkillLevel
    {
        get { return mSkillLevel; }
        set { mSkillLevel = value; }
    }

    /// <summary>
    /// 卡片的经验
    /// </summary>
    public int EXP
    {
        get { return mExp; }
        set { mExp = value; }
    }

    /// <summary>
    /// 获得日期
    /// </summary>
    public System.DateTime GetDate
    {
        get { return mGetDate; }
        set { mGetDate = value; }
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

    private Dictionary<System.Int64, CharacterData> mCharacterDatas = new Dictionary<System.Int64, CharacterData>();

    public CharacterManager()
    {
        for (int i = 0; i < 6; i++)
        {
            CharacterData data = CreateRandomCharactor(i);
            mCharacterDatas[data.ID] = data;
        }
    }

    public CharacterData CreateRandomCharactor(System.Int64 _id)
    {
        CharacterData data = new CharacterData();
        data.ID = _id;
        data.CardID = Random.Range(1, 18);
        data.Level = 10;
        data.SkillLevel = 1;
        data.GetDate = System.DateTime.Now;

        CardBaseData carddata = CardManager.Instance.GetCard(data.CardID);
        data.MaxHP = GrowingManager.Instance.GetGrowing(carddata.GrowingType).GetHP(data.Level);
        data.MaxMP = GrowingManager.Instance.GetGrowing(carddata.GrowingType).GetMP(data.Level);
        data.Atk = GrowingManager.Instance.GetGrowing(carddata.GrowingType).GetATK(data.Level);
        data.Def = GrowingManager.Instance.GetGrowing(carddata.GrowingType).GetDEF(data.Level);
        data.Spd = GrowingManager.Instance.GetGrowing(carddata.GrowingType).GetSPD(data.Level);

        return data;
    }

    /// <summary>
    /// 获取角色数据
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public CharacterData GetCharacter(System.Int64 _id)
    {
        if (!mCharacterDatas.ContainsKey(_id))
        {
            return null;
        }

        return mCharacterDatas[_id];
    }
}