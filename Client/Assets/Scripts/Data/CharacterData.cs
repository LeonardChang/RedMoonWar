using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ��ɫ����
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
    /// ��ƬID
    /// </summary>
    public int ID
    {
        get { return mID; }
    }

    /// <summary>
    /// ���HP
    /// </summary>
    public int MaxHP
    {
        get { return mMaxHP; }
    }

    /// <summary>
    /// ���MP
    /// </summary>
    public int MaxMP
    {
        get { return mMaxMP; }
    }

    /// <summary>
    /// ������
    /// </summary>
    public int Atk
    {
        get { return mAtk; }
    }

    /// <summary>
    /// ������
    /// </summary>
    public int Def
    {
        get { return mDef; }
    }

    /// <summary>
    /// �ٶ�
    /// </summary>
    public int Spd
    {
        get { return mSpd; }
    }

    /// <summary>
    /// ��Ӧ�Ŀ�ƬID
    /// </summary>
    public int CardID
    {
        get { return mCardID; }
    }

    /// <summary>
    /// ��ǰ�ȼ�
    /// </summary>
    public int Level
    {
        get { return mLevel; }
    }
    
    /// <summary>
    /// ��ǰ���ܵȼ�
    /// </summary>
    public int SkillLevel
    {
        get { return mSkillLevel; }
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

    private Dictionary<int, CharacterData> mCharacterDatas = new Dictionary<int, CharacterData>();

    public CharacterManager()
    {

    }

    /// <summary>
    /// ��ȡ��ɫ����
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