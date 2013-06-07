using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 好友的数据
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
    /// 玩家ID
    /// </summary>
    public System.Int64 PlayerID
    {
        get { return mPlayerID; }
        set { mPlayerID = value; }
    }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName
    {
        get { return mNickName; }
        set { mNickName = value; }
    }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level
    {
        get { return mLevel; }
        set { mLevel = value; }
    }

    /// <summary>
    /// 队长卡牌
    /// </summary>
    public CharacterData LeaderCharacter
    {
        get { return mLeader; }
        set { mLeader = value; }
    }

    /// <summary>
    /// 最后登录日期
    /// </summary>
    public System.DateTime LastLoginTime
    {
        get { return mLastLoginTime; }
        set { mLastLoginTime = value; }
    }

    /// <summary>
    /// 剩余的友情点
    /// </summary>
    public int TodayFriendshipPoint
    {
        get { return mTodayFriendshipPoint; }
        set { mTodayFriendshipPoint = value; }
    }
}

/// <summary>
/// 好友管理器
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

    private Dictionary<System.Int64, FriendData> mFriends = new Dictionary<System.Int64, FriendData>(); // 好友列表
    private Dictionary<System.Int64, FriendData> mStrangers = new Dictionary<System.Int64, FriendData>(); // 请求加好友的陌生人

    public Dictionary<System.Int64, FriendData> MyFriends
    {
        get { return mFriends; }
    }

    public Dictionary<System.Int64, FriendData> Strangers
    {
        get { return mStrangers; }
    }

    /// <summary>
    /// 获取所有好友
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
    /// 通过id获取某个好友
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
    /// 获取所有陌生人
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
    /// 通过id获取某个陌生热
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
    /// 当前陌生人数量
    /// </summary>
    public int StrangerCount
    {
        get
        {
            return mStrangers.Values.Count;
        }
    }
}
