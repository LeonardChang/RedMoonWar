using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 棋子的详细数据
/// </summary>
[System.Serializable]
public class BattleCharacterData : CharacterData
{
    protected int mInitX = 0;
    protected int mInitY = 0;

    /// <summary>
    /// 初始化x位置
    /// </summary>
    public int InitX
    {
        get
        {
            return mInitX;
        }
        set
        {
            mInitX = value;
        }
    }

    /// <summary>
    /// 初始化y位置
    /// </summary>
    public int InitY
    {
        get
        {
            return mInitY;
        }
        set
        {
            mInitY = value;
        }
    }
}

/// <summary>
/// 敌方棋子的详细数据
/// </summary>
[System.Serializable]
public class BattleEnemyData : BattleCharacterData
{
    protected int mBuyPrice = 0;
    protected int mDropCard = 0;
    protected int mDropCoin = 0;
    protected AIType mAI = AIType.Retarded;

    /// <summary>
    /// 收买价格，0为不可收买
    /// </summary>
    public int BuyPrice
    {
        get
        {
            return mBuyPrice;
        }
        set
        {
            mBuyPrice = value;
        }
    }

    /// <summary>
    /// 掉落卡牌数量
    /// </summary>
    public int DropCard
    {
        get
        {
            return mDropCard;
        }
        set
        {
            mDropCard = value;
        }
    }

    /// <summary>
    /// 掉落金币数量
    /// </summary>
    public int DropCoin
    {
        get
        {
            return mDropCoin;
        }
        set
        {
            mDropCoin = value;
        }
    }

    /// <summary>
    /// 怪物的AI
    /// </summary>
    public AIType AI
    {
        get { return mAI; }
        set { mAI = value; }
    }
}

/// <summary>
/// 战场配置数据
/// </summary>
[System.Serializable]
public class Stage 
{
    private static volatile Stage instance;
    private static object syncRoot = new System.Object();

    public static Stage Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Stage();
                    }
                }
            }
            return instance;
        }
    }

    private System.Int64 mStageID = 0; // 服务器分配的StageID

    private int mWidth = 6; // 地图的宽
    private int mHeight = 50; // 地图的高
    private int mScene = 0; // 地图的场景ID

    private List<BattleCharacterData> mPlayerTeam = new List<BattleCharacterData>(); //玩家队伍列表，1位置为玩家队长，2位置为好友队长
    private List<BattleEnemyData> mEnemyTeam = new List<BattleEnemyData>(); // 敌人队伍列表

    public System.Int64 StageID
    {
        get
        {
            return mStageID;
        }
        set
        {
            mStageID = value;
        }
    }

    public int Width
    {
        get
        {
            return mWidth;
        }
        set
        {
            mWidth = value;
        }
    }

    public int Height
    {
        get
        {
            return mHeight;
        }
        set
        {
            mHeight = value;
        }
    }

    public int Scene
    {
        get
        {
            return mScene;
        }
        set
        {
            mScene = value;
        }
    }

    public BattleCharacterData[] Players
    {
        get
        {
            return mPlayerTeam.ToArray();
        }
    }

    public BattleEnemyData[] Enemys
    {
        get
        {
            return mEnemyTeam.ToArray();
        }
    }

    public Stage()
    {
        mWidth = 6;
        mHeight = 50;
        mScene = Random.Range(0, 5);

        mPlayerTeam.Clear();
        for (int i = 0; i < 6; i++)
        {
            BattleCharacterData data = new BattleCharacterData();
            CreateRandomCharactor(ref data, i);
            data.InitX = 0 + i;
            data.InitY = 1;

            mPlayerTeam.Add(data);
        }

        mEnemyTeam.Clear();
        for (int i = 0; i < 13; i++)
        {
            BattleEnemyData data = new BattleEnemyData();
            CreateRandomEnemy(ref data, i + 10000);
            data.InitX = Random.Range(0, 6);
            data.InitY = 10 + i * 3;
            data.BuyPrice = 0;
            data.DropCard = 0;
            data.DropCoin = 0;
            data.AI = AIType.Guard;

            mEnemyTeam.Add(data);
        }
    }

    private void CreateRandomCharactor(ref BattleCharacterData _data, int _id)
    {
        CharacterData data = _data;
        data.ID = _id;
        data.CardID = 150; //Random.Range(1, 190);
        data.Level = 10;
        data.SkillLevel = 1;
        data.GetDate = System.DateTime.Now;

        CardBaseData carddata = CardManager.Instance.GetCard(data.CardID);
        data.MaxHP = GrowingManager.Instance.GetGrowing(carddata.GrowingType).GetHP(data.Level);
        data.MaxMP = GrowingManager.Instance.GetGrowing(carddata.GrowingType).GetMP(data.Level);
        data.Atk = GrowingManager.Instance.GetGrowing(carddata.GrowingType).GetATK(data.Level);
        data.Def = GrowingManager.Instance.GetGrowing(carddata.GrowingType).GetDEF(data.Level);
        data.Spd = GrowingManager.Instance.GetGrowing(carddata.GrowingType).GetSPD(data.Level);
    }

    private void CreateRandomEnemy(ref BattleEnemyData _data, int _id)
    {
        CharacterData data = _data;
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
    }
}
