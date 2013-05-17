using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// �����Ա����Ϣ
/// </summary>
[System.Serializable]
public class FormationData
{
    private System.Int64 mCharacterID;
    private int mX;
    private int mY;

    /// <summary>
    /// ��ɫ��ID
    /// </summary>
    public System.Int64 CharacterID
    {
        get
        {
            return mCharacterID;
        }
        set
        {
            mCharacterID = value;
        }
    }

    /// <summary>
    /// ���ڱ���е�xλ��
    /// </summary>
    public int X
    {
        get
        {
            return mX;
        }
        set
        {
            mX = value;
        }
    }

    /// <summary>
    /// ���ڱ���е�yλ��
    /// </summary>
    public int Y
    {
        get
        {
            return mY;
        }
        set
        {
            mY = value;
        }
    }
}

/// <summary>
/// ��Ҷ������ݹ�����
/// </summary>
[System.Serializable]
public class Formation
{
    private static volatile Formation instance;
    private static object syncRoot = new System.Object();

    public static Formation Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Formation();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<System.Int64, CharacterData> mTeam = new Dictionary<System.Int64, CharacterData>(); // ������п�Ƭ�б�
    private List<FormationData> mFormation = new List<FormationData>(); // ��ұ�ӿ�Ƭ�б���һ��Ϊ�ӳ���

    private int mFriendPosition;

    /// <summary>
    /// ���ѵĿ��ƴ��ڱ���е�λ��
    /// </summary>
    public int FriendPosition
    {
        get
        {
            return mFriendPosition;
        }
        set
        {
            mFriendPosition = value;
        }
    }

    /// <summary>
    /// ��ȡĳ����ɫ������
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public CharacterData GetCharacter(System.Int64 _id)
    {
        if (mTeam.ContainsKey(_id))
        {
            return mTeam[_id];
        }

        return null;
    }

    static int CompareCharHP(CharacterData a, CharacterData b)
    {
        return (a.MaxHP.CompareTo(b.MaxHP));
    }

    static int CompareCharElement(CharacterData a, CharacterData b)
    {
        return (CardManager.Instance.GetCard(a.CardID).Element.CompareTo(CardManager.Instance.GetCard(b.CardID).Element));
    }

    static int CompareCharATK(CharacterData a, CharacterData b)
    {
        return (a.Atk.CompareTo(b.Atk));
    }

    static int CompareCharDef(CharacterData a, CharacterData b)
    {
        return (a.Def.CompareTo(b.Def));
    }

    static int CompareCharSpd(CharacterData a, CharacterData b)
    {
        return (a.Spd.CompareTo(b.Spd));
    }

    static int CompareCharGetDate(CharacterData a, CharacterData b)
    {
        return (a.GetDate.CompareTo(b.GetDate));
    }

    /// <summary>
    /// ������������ȡ�����б�
    /// </summary>
    /// <param name="_sequence"></param>
    /// <returns></returns>
    public IEnumerable<CharacterData> GetAllCharacter(CharacterSequenceType _sequence)
    {
        List<CharacterData> list = new List<CharacterData>();
        foreach (System.Int64 id in mTeam.Keys)
        {
            list.Add(mTeam[id]);
        }

        switch (_sequence)
        {
            case CharacterSequenceType.Element:
                list.Sort(CompareCharElement);
                break;
            case CharacterSequenceType.HP:
                list.Sort(CompareCharHP);
                list.Reverse();
                break;
            case CharacterSequenceType.ATK:
                list.Sort(CompareCharATK);
                list.Reverse();
                break;
            case CharacterSequenceType.DEF:
                list.Sort(CompareCharDef);
                list.Reverse();
                break;
            case CharacterSequenceType.SPD:
                list.Sort(CompareCharSpd);
                list.Reverse();
                break;
            case CharacterSequenceType.GetTime:
                list.Sort(CompareCharGetDate);
                break;
        }

        foreach (CharacterData data in list)
        {
            yield return data;
        }
        list.Clear();
    }

    /// <summary>
    /// ��ȡ���
    /// </summary>
    /// <returns></returns>
    public IEnumerable<FormationData> GetFormation()
    {
        foreach (FormationData data in mFormation)
        {
            yield return data;
        }
    }

    /// <summary>
    /// ��ȡ����е����н�ɫ
    /// </summary>
    /// <returns></returns>
    public IEnumerable<CharacterData> GetFormationCharacter()
    {
        foreach (FormationData data in mFormation)
        {
            yield return GetCharacter(data.CharacterID);
        }
    }
}
