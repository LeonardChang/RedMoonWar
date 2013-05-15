using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ��ɫ����
/// </summary>
public class CharacterData 
{
    private System.Int64 mID = 0;

    private int mMaxHP = 100;
    private int mMaxMP = 100;
    private int mAtk = 100;
    private int mDef = 100;
    private int mSpd = 100;

    private int mCardID = 0;

    private int mLevel = 1;
    private int mSkillLevel = 1;

    private System.DateTime mGetDate = System.DateTime.Now;

    /// <summary>
    /// ��ƬID
    /// </summary>
    public System.Int64 ID
    {
        get { return mID; }
        set { mID = value; }
    }

    /// <summary>
    /// ���HP
    /// </summary>
    public int MaxHP
    {
        get { return mMaxHP; }
        set { mMaxHP = value; }
    }

    /// <summary>
    /// ���MP
    /// </summary>
    public int MaxMP
    {
        get { return mMaxMP; }
        set { mMaxMP = value; }
    }

    /// <summary>
    /// ������
    /// </summary>
    public int Atk
    {
        get { return mAtk; }
        set { mAtk = value; }
    }

    /// <summary>
    /// ������
    /// </summary>
    public int Def
    {
        get { return mDef; }
        set { mDef = value; }
    }

    /// <summary>
    /// �ٶ�
    /// </summary>
    public int Spd
    {
        get { return mSpd; }
        set { mSpd = value; }
    }

    /// <summary>
    /// ��Ӧ�Ŀ�ƬID
    /// </summary>
    public int CardID
    {
        get { return mCardID; }
        set { mCardID = value; }
    }

    /// <summary>
    /// ��ǰ�ȼ�
    /// </summary>
    public int Level
    {
        get { return mLevel; }
        set { mLevel = value; }
    }
    
    /// <summary>
    /// ��ǰ���ܵȼ�
    /// </summary>
    public int SkillLevel
    {
        get { return mSkillLevel; }
        set { mSkillLevel = value; }
    }

    /// <summary>
    /// �������
    /// </summary>
    public System.DateTime GetDate
    {
        get { return mGetDate; }
        set { mGetDate = value; }
    }
}

/// <summary>
/// ��ɫ������
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
        for (int i = 0; i < 5; i++ )
        {
            CharacterData data = CreateRandomCharactor(i);
            mCharacterDatas[data.ID] = data;
        }
    }

    public CharacterData CreateRandomCharactor(System.Int64 _id)
    {
        CharacterData data = new CharacterData();
        data.ID = _id;
        data.CardID = Random.Range(1, 7);
        data.Level = 1;
        data.SkillLevel = 1;
        data.GetDate = System.DateTime.Now;
        
        CardBaseData carddata = CardManager.Instance.GetCard(data.CardID);
        data.MaxHP = (int)carddata.BaseHP;
        data.MaxMP = (int)carddata.BaseMP;
        data.Atk = (int)carddata.BaseAtk;
        data.Def = (int)carddata.BaseDef;
        data.Spd = (int)carddata.BaseSpd;
        
        return data;
    }

    /// <summary>
    /// ��ȡ��ɫ����
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