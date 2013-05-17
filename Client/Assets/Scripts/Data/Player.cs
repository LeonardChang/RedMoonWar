using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// �������
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

    private string mUserName; // �ʺ�
    private string mPassword; // ����
    private string mNickName; // �������
    private System.Int64 mPlayerID; // ���ID
    private int mCoin; // �������
    private int mGem; // ��ʯ������
    private int mPower; // ����
    private int mPowerMax; // ��������
    private float mPowerAddSpeed; // �����ָ��ٶȣ��룩
    private int mLevel; // �ȼ�
    private int mEXP; // ����ֵ
    private int mEXPMax; // �������辭��
    private int mFriendshipPoint; // �������
    private int mAddFriendRequest; // δ����ĺ���������

    /// <summary>
    /// �ʺ�
    /// </summary>
    public string UserName
    {
        get { return mUserName; }
        set { mUserName = value; }
    }

    /// <summary>
    /// ����
    /// </summary>
    public string Password
    {
        get { return mPassword; }
        set { mPassword = value; }
    }

    /// <summary>
    /// �������
    /// </summary>
    public string NickName
    {
        get { return mNickName; }
        set { mNickName = value; }
    }

    /// <summary>
    /// ���ID
    /// </summary>
    public System.Int64 PlayerID
    {
        get { return mPlayerID; }
        set { mPlayerID = value; }
    }

    /// <summary>
    /// �������
    /// </summary>
    public int Coin
    {
        get { return mCoin; }
        set { mCoin = value; }
    }

    /// <summary>
    /// ��ʯ������
    /// </summary>
    public int Gem
    {
        get { return mGem; }
        set { mGem = value; }
    }

    /// <summary>
    /// ����
    /// </summary>
    public int Power
    {
        get { return mPower; }
        set { mPower = value; }
    }

    /// <summary>
    /// ��������
    /// </summary>
    public int PowerMax
    {
        get { return mPowerMax; }
        set { mPowerMax = value; }
    }

    /// <summary>
    /// �����ָ��ٶȣ��룩
    /// </summary>
    public float PowerAddSpeed
    {
        get { return mPowerAddSpeed; }
        set { mPowerAddSpeed = value; }
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
    /// ����ֵ
    /// </summary>
    public int EXP
    {
        get { return mEXP; }
        set { mEXP = value; }
    }

    /// <summary>
    /// �������辭��
    /// </summary>
    public int EXPMax
    {
        get { return mEXPMax; }
        set { mEXPMax = value; }
    }

    /// <summary>
    /// �������
    /// </summary>
    public int FriendshipPoint
    {
        get { return mFriendshipPoint; }
        set { mFriendshipPoint = value; }
    }

    /// <summary>
    /// δ����ĺ���������
    /// </summary>
    public int AddFriendRequest
    {
        get { return mAddFriendRequest; }
        set { mAddFriendRequest = value; }
    }
}
