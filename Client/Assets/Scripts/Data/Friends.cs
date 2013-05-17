using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ���ѵ�����
/// </summary>
[System.Serializable]
public class FriendData
{
    private System.Int64 mPlayerID;
    private string mNickName;
    private int mLevel;
    private CharacterData mLeader;
    private System.DateTime mLastLoginTime;
    private int mTodayFriendshipPoint;

    /// <summary>
    /// ���ID
    /// </summary>
    public System.Int64 PlayerID
    {
        get { return mPlayerID; }
        set { mPlayerID = value; }
    }

    /// <summary>
    /// �ǳ�
    /// </summary>
    public string NickName
    {
        get { return mNickName; }
        set { mNickName = value; }
    }

    /// <summary>
    /// �ȼ�
    /// </summary>
    public int Level
    {
        get { return mLevel; }
        set { mLevel = value; }
    }

    /// <summary>
    /// �ӳ�����
    /// </summary>
    public CharacterData LeaderCharacter
    {
        get { return mLeader; }
        set { mLeader = value; }
    }

    /// <summary>
    /// ����¼����
    /// </summary>
    public System.DateTime LastLoginTime
    {
        get { return mLastLoginTime; }
        set { mLastLoginTime = value; }
    }

    /// <summary>
    /// ʣ��������
    /// </summary>
    public int TodayFriendshipPoint
    {
        get { return mTodayFriendshipPoint; }
        set { mTodayFriendshipPoint = value; }
    }
}

/// <summary>
/// ���ѹ�����
/// </summary>
[System.Serializable]
public class Friends
{
    private static volatile Friends instance;
    private static object syncRoot = new System.Object();

    public static Friends Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Friends();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<System.Int64, FriendData> mFriends = new Dictionary<System.Int64, FriendData>(); // �����б�
    private Dictionary<System.Int64, FriendData> mStrangers = new Dictionary<System.Int64, FriendData>(); // ����Ӻ��ѵ�İ����

    /// <summary>
    /// ��ȡ���к���
    /// </summary>
    /// <returns></returns>
    public IEnumerable<FriendData> GetFriends()
    {
        foreach (System.Int64 id in mFriends.Keys)
        {
            yield return mFriends[id];
        }
    }

    /// <summary>
    /// ͨ��id��ȡĳ������
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public FriendData GetFriend(System.Int64 _id)
    {
        if (mFriends.ContainsKey(_id))
        {
            return mFriends[_id];
        }

        return null;
    }

    /// <summary>
    /// ��ȡ����İ����
    /// </summary>
    /// <returns></returns>
    public IEnumerable<FriendData> GetStrangers()
    {
        foreach (System.Int64 id in mStrangers.Keys)
        {
            yield return mStrangers[id];
        }
    }

    /// <summary>
    /// ͨ��id��ȡĳ��İ����
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public FriendData GetStranger(System.Int64 _id)
    {
        if (mStrangers.ContainsKey(_id))
        {
            return mStrangers[_id];
        }

        return null;
    }

    /// <summary>
    /// ��ǰİ��������
    /// </summary>
    public int StrangerCount
    {
        get
        {
            return mStrangers.Values.Count;
        }
    }
}
