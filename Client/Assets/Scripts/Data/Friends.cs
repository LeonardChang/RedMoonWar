using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    private List<int> friendIds = new List<int>();
    private List<int> strengerIds = new List<int>(); 

    public List<int> MyFriends
    {
        get { return friendIds; }
    }

    public List<int> Strangers
    {
        get { return strengerIds; }
    }

    /// <summary>
    /// 获取所有好友
    /// </summary>
    /// <returns></returns>
    public List<PlayerFeedBack> GetFriends()
    {
		List<PlayerFeedBack> players = new List<PlayerFeedBack>(); 
        foreach (int id in friendIds)
        {
            players.Add(ServerDatas.playerDatas[id]);
        }
		return players;
    }

    /// <summary>
    /// 通过id获取某个好友
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public PlayerFeedBack GetFriend(int id)
    {
       return ServerDatas.playerDatas[id];
    }

    /// <summary>
    /// 获取所有陌生人
    /// </summary>
    /// <returns></returns>
    public List<PlayerFeedBack> GetStrangers()
    {
        List<PlayerFeedBack> players = new List<PlayerFeedBack>(); 
        foreach (int id in strengerIds)
        {
            players.Add(ServerDatas.playerDatas[id]);
        }
		return players;
    }

    /// <summary>
    /// 通过id获取某个陌生热
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public PlayerFeedBack GetStranger(int id)
    {
        return ServerDatas.playerDatas[id];
    }

    /// <summary>
    /// 当前陌生人数量
    /// </summary>
    public int StrangerCount
    {
        get
        {
            return strengerIds.Count;
        }
    }
	
	public int FriendCount
	{
		get
		{
			return friendIds.Count;
		}
	}
}
