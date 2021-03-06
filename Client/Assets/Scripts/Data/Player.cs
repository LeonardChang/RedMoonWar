﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 玩家数据
/// </summary>
[System.Serializable]
public class Player 
{
    private static volatile Player instance;
    private static object syncRoot = new System.Object();

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Player();
                    }
                }
            }
            return instance;
        }
    }

    private string mUserName; // 帐号
    private string mPassword; // 密码
    private string mNickName; // 玩家名字
    private System.Int64 mPlayerID; // 玩家ID
    private int mCoin; // 金币数量
    private int mGem; // 充值币数量
    private int mEnergy; // 体力
    private int mEnergyMax; // 体力上限
    private float mEnergyAddTime; // 下一次回复体力的剩余时间（秒）
    private int mLevel; // 等级
    private int mEXP; // 经验值
    private int mEXPMax; // 升级所需经验
    private int mFriendshipPoint; // 友情点数
    private int mAddFriendRequest; // 未处理的好友请求数
    private int mCost; // 统御力

    /// <summary>
    /// 帐号
    /// </summary>
    public string UserName
    {
        get { return mUserName; }
        set { mUserName = value; }
    }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password
    {
        get { return mPassword; }
        set { mPassword = value; }
    }

    /// <summary>
    /// 玩家名字
    /// </summary>
    public string NickName
    {
        get { return mNickName; }
        set { mNickName = value; }
    }

    /// <summary>
    /// 玩家ID
    /// </summary>
    public System.Int64 PlayerID
    {
        get { return mPlayerID; }
        set { mPlayerID = value; }
    }

    /// <summary>
    /// 金币数量
    /// </summary>
    public int Coin
    {
        get { return mCoin; }
        set { mCoin = value; }
    }

    /// <summary>
    /// 宝石币数量
    /// </summary>
    public int Gem
    {
        get { return mGem; }
        set { mGem = value; }
    }

    /// <summary>
    /// 体力
    /// </summary>
    public int Energy
    {
        get { return mEnergy; }
        set { mEnergy = value; }
    }

    /// <summary>
    /// 体力上限
    /// </summary>
    public int EnergyMax
    {
        get { return mEnergyMax; }
        set { mEnergyMax = value; }
    }

    /// <summary>
    /// 下一次回复体力的剩余时间（秒）
    /// </summary>
    public float EnergyAddTime
    {
        get { return mEnergyAddTime; }
        set { mEnergyAddTime = value; }
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
    /// 经验值
    /// </summary>
    public int EXP
    {
        get { return mEXP; }
        set { mEXP = value; }
    }

    /// <summary>
    /// 升级所需经验
    /// </summary>
    public int EXPMax
    {
        get { return mEXPMax; }
        set { mEXPMax = value; }
    }

    /// <summary>
    /// 友情点数
    /// </summary>
    public int FriendshipPoint
    {
        get { return mFriendshipPoint; }
        set { mFriendshipPoint = value; }
    }

    /// <summary>
    /// 未处理的好友请求数
    /// </summary>
    public int AddFriendRequest
    {
        get { return mAddFriendRequest; }
        set { mAddFriendRequest = value; }
    }

    /// <summary>
    /// 统御力
    /// </summary>
    public int Cost
    {
        get { return mCost; }
        set { mCost = value; }
    }
}
