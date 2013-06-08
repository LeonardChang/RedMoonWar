using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chessboard : MonoBehaviour {
    private int[,] mChessboardData = null;
    private Food[,] mItemData = null;

    private int mWidth;
    private int mHeight;

    public Transform CardRoot;
    public Transform Road;
    public Transform TopRoad;

    public Transform GameMap;

    [HideInInspector]
    public Dictionary<int, CardLogic> mChessList = new Dictionary<int, CardLogic>();

    private int mBottomLine = 0;

    public System.Action<int> RefreshFinishEvent;

    void Awake()
    {
    }

	// Use this for initialization
	void Start () {
        //CacheLoader.Instance.CheckAllCache();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    bool mNeedRefresh = false;
    void LateUpdate()
    {
        if (mNeedRefresh)
        {
            mNeedRefresh = false;
            Refresh(false);
        }
    }

    public void Initlize(Stage _stage)
    {
        mWidth = _stage.Width;
        mHeight = _stage.Height;
        mChessboardData = new int[mWidth, mHeight];
        mItemData = new Food[mWidth, mHeight];
        for (int i = 0; i < mWidth; i++)
        {
            for (int j = 0; j < mHeight; j++)
            {
                mChessboardData[i, j] = -1;
                mItemData[i, j] = null;
            }
        }

        GameObject groundPerfab = Resources.Load("Backgrounds/Ground", typeof(GameObject)) as GameObject;
        GameObject magmaPerfab = Resources.Load("Backgrounds/Magma", typeof(GameObject)) as GameObject;
        GameObject magmaDownPerfab = Resources.Load("Backgrounds/GroundDown", typeof(GameObject)) as GameObject;
        for (int i = 0; i < mWidth; i++)
        {
            for (int j = -5; j < mHeight; j++)
            {
                if (j < 0)
                {
                    GameObject ground = Instantiate(magmaPerfab) as GameObject;
                    ground.transform.parent = Road.transform.parent;
                    ground.transform.localPosition = new Vector3((float)(i - 2) * RectSize - RectSize * 0.5f + 3, j * RectSize, 0);
                    ground.transform.localScale = new Vector3(82, 82, 1);
                }
                else
                {
                    GameObject ground = Instantiate(groundPerfab) as GameObject;
                    ground.transform.parent = Road.transform.parent;
                    ground.transform.localPosition = new Vector3((float)(i - 2) * RectSize - RectSize * 0.5f + 3, j * RectSize, 0);

                    int gt = Random.Range(1, 20);
                    if (gt > 5)
                    {
                        gt = 1;
                    }
                    gt += _stage.Scene * 5;
                    UISprite sprite = ground.GetComponent<UISprite>();
                    sprite.spriteName = "Ground" + gt.ToString("00");
                    sprite.MakePixelPerfect();
                }

                if (j == 0)
                {
                    GameObject ground = Instantiate(magmaDownPerfab) as GameObject;
                    ground.transform.parent = Road.transform.parent;
                    ground.transform.localPosition = new Vector3((float)(i - 2) * RectSize - RectSize * 0.5f + 3, j * RectSize, -1);
                    ground.transform.localScale = new Vector3(82, 32, 1);
                }
            }
        }

        TopRoad.localPosition = new Vector3(0, (mHeight - 1) * RectSize, 0);
        Road.localPosition = TopRoad.localPosition;
        Road.localScale = new Vector3(640, (mHeight + 10) * RectSize, 1);

        GameObject shadowPerfab = null;
        switch (_stage.Scene)
        {
            case 0:
                shadowPerfab = Resources.Load("Backgrounds/LineShadow") as GameObject;
                break;
            case 1:
                shadowPerfab = Resources.Load("Backgrounds/CloudShadow") as GameObject;
                break;
            case 2:
                shadowPerfab = Resources.Load("Backgrounds/LeafShadow") as GameObject;
                break;
            case 3:
                shadowPerfab = Resources.Load("Backgrounds/DeadwoodShadow") as GameObject;
                break;
            case 4:
                shadowPerfab = Resources.Load("Backgrounds/IceShadow") as GameObject;
                break;
            default:
                break;
        }
        if (shadowPerfab != null)
        {
            GameObject shadow = Instantiate(shadowPerfab) as GameObject;
            shadow.transform.parent = Road.transform.parent;
            shadow.transform.localPosition = Road.localPosition + new Vector3(0, 300, -1);
            Vector3 scale = Road.localScale + new Vector3(0, 500, 0);
            shadow.transform.localScale = scale;
            shadow.GetComponent<UITexture>().material.mainTextureScale = new Vector2(1, scale.y / scale.x);
        }

        mChessList.Clear();
        Object perfab = Resources.Load("Cards/Perfabs/Card");

        for (int i = 0; i < _stage.Players.Length; i++ )
        {
            BattleCharacterData data = _stage.Players[i];

            GameObject card = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
            card.name = "Char" + data.ID.ToString();

            card.transform.parent = CardRoot;
            card.transform.localScale = Vector3.one;
            CardLogic logic = card.GetComponent<CardLogic>();
            logic.Data.ResetAllData(PhaseType.Charactor, data);

            logic.Data.BuyPrice = 0;
            logic.Data.DropCard = 0;
            logic.Data.DropCoin = 0;
            logic.Data.IsLeader = i <= 1;

            int id = mChessList.Keys.Count;
            mChessList.Add(id, logic);
            InitlizeChess(data.InitX, data.InitY, id);
        }

        foreach (BattleEnemyData data in _stage.Enemys)
        {
            GameObject card = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
            card.name = "Enemy" + data.ID.ToString();

            card.transform.parent = CardRoot;
            card.transform.localScale = Vector3.one;
            CardLogic logic = card.GetComponent<CardLogic>();
            logic.Data.ResetAllData(PhaseType.Enemy, data);

            logic.Data.BuyPrice = data.BuyPrice;
            logic.Data.DropCard = data.DropCard;
            logic.Data.DropCoin = data.DropCoin;
            logic.Data.IsLeader = false;
            logic.Data.EnemyAI = data.AI;

            int id = mChessList.Keys.Count;
            mChessList.Add(id, logic);
            InitlizeChess(data.InitX, data.InitY, id);
        }

        mBottomLine = 0;
        Refresh(true);
    }

    bool InitlizeChess(int _x, int _y, int _id)
    {
        if (mChessboardData == null || _x < 0 || _x >= mWidth || _y < 0 || _y >= mHeight)
        {
            return false;
        }

        mChessboardData[_x, _y] = _id;
        mChessList[_id].Data.SetPosition(_x, _y);

        return true;
    }

    Dictionary<int, CardLogic> mTempChessList = new Dictionary<int, CardLogic>();
    void GetAllSelectCharactor()
    {
        mTempChessList.Clear();
        foreach (int id in mChessList.Keys)
        {
            CardLogic obj = mChessList[id];
            CardData data = obj.Data;
            if (data.Phase == PhaseType.Charactor && !data.Death && obj.IsSelect)
            {
                mTempChessList.Add(id, obj);
            }
        }
    }

    void GetAllEnemt()
    {
        mTempChessList.Clear();
        foreach (int id in mChessList.Keys)
        {
            CardLogic obj = mChessList[id];
            CardData data = obj.Data;
            if (data.Phase == PhaseType.Enemy && !data.Death)
            {
                mTempChessList.Add(id, obj);
            }
        }
    }

    public void GoUp()
    {
        TryMove(0, 1);
    }

    public void GoDown()
    {
        TryMove(0, -1);
    }

    public void GoLeft()
    {
        TryMove(-1, 0);
    }

    public void GoRight()
    {
        TryMove(1, 0);
    }

    public void GoUpLeft()
    {
        TryMove(-1, 1);
    }

    public void GoDownLeft()
    {
        TryMove(-1, -1);
    }

    public void GoUpRight()
    {
        TryMove(1, 1);
    }

    public void GoDownRight()
    {
        TryMove(1, -1);
    }

    void TryMove(int _xOffset, int _yOffset)
    {
        GetAllSelectCharactor();
        while (mTempChessList.Keys.Count > 0)
        {
            int count = mTempChessList.Keys.Count;
            int notDealNumber = 0;
            List<int> needDeletes = new List<int>();

            foreach (int id in mTempChessList.Keys)
            {
                CardLogic obj = mChessList[id];

                CardData data = obj.Data;
                int x = data.X + _xOffset;
                int y = data.Y + _yOffset;

                if (x < 0 || x >= mWidth || y < 0 || y >= mHeight)
                {
                    needDeletes.Add(id);
                }
                else if (mChessboardData[x, y] == -1 || mChessList[mChessboardData[x, y]].Data.Death)
                {
                    mChessboardData[x, y] = mChessboardData[data.X, data.Y];
                    mChessboardData[data.X, data.Y] = -1;
                    data.SetPosition(x, y);

                    needDeletes.Add(id);

                    CalculatorBottomLine();
                }
                else
                {
                    notDealNumber += 1;
                }
            }

            foreach (int id in needDeletes)
            {
                mTempChessList.Remove(id);
            }
            needDeletes.Clear();

            if (count == notDealNumber)
            {
                break;
            }
        }

        mTempChessList.Clear();
        mNeedRefresh = true;
    }

    void EnemyTryMove(CardLogic _logic, int _xOffset, int _yOffset)
    {
        if (_xOffset == 0 && _yOffset == 0)
        {
            return;
        }

        CardData data = _logic.Data;
        if (data.Y > mBottomLine + 10 || data.Y < mBottomLine - 5)
        {
            return;
        }
        
        int x = data.X + _xOffset;
        int y = data.Y + _yOffset;

        if (x < 0 || x >= mWidth || y < 0 || y >= mHeight)
        {
        }
        else if (mChessboardData[x, y] == -1 || mChessList[mChessboardData[x, y]].Data.Death)
        {
            mChessboardData[x, y] = mChessboardData[data.X, data.Y];
            mChessboardData[data.X, data.Y] = -1;
            data.SetPosition(x, y);
        }
    }

    public void AllEmemyMove()
    {
        GetAllEnemt();
        while (mTempChessList.Keys.Count > 0)
        {
            int count = mTempChessList.Keys.Count;
            int notDealNumber = 0;
            List<int> needDeletes = new List<int>();

            foreach (int id in mTempChessList.Keys)
            {
                CardLogic obj = mChessList[id];
                switch (obj.Data.EnemyAI)
                {
                    case AIType.NPC:
                        if (Random.Range(0, 10000) > 6666)
                        {
                            TryRandomMove(obj);
                        }
                        break;
                    case AIType.Retarded:
                        if (Random.Range(0, 10) > 7)
                        {
                            TryMoveToPrey(obj);
                        }
                        else if (Random.Range(0, 10) > 5)
                        {
                            TryRandomMove(obj);
                        }
                        break;
                    case AIType.Slime:
                        if (!TryMoveToPrey(obj))
                        {
                            if (Random.Range(0, 10) > 5)
                            {
                                TryRandomMove(obj);
                            }
                        }
                        break;
                    case AIType.Goblin:
                        if (obj.Data.LastAttackerID != -1)
                        {
                            CardLogic target = GetChess(obj.Data.LastAttackerID);
                            TryMoveTo(obj, target);
                        }
                        else if (!TryMoveToPrey(obj))
                        {
                            if (Random.Range(0, 10) > 5)
                            {
                                TryRandomMove(obj);
                            }
                        }
                        break;
                    case AIType.Pillar:
                        break;
                    case AIType.Cannon:
                        TryMoveToPrey(obj);
                        break;
                    case AIType.Guard:
                        if (obj.Data.LastAttackerID != -1)
                        {
                            CardLogic target = GetChess(obj.Data.LastAttackerID);
                            TryMoveTo(obj, target);
                        }
                        else
                        {
                            TryMoveToPrey(obj);
                        }
                        break;
                    case AIType.Assailant:
                        if (!TryMoveToPrey(obj))
                        {
                            CardLogic target = GetClosestPlayer(obj.Data, PhaseType.Charactor);
                            TryMoveTo(obj, target);
                        }
                        break;
                    case AIType.Leader:
                        if (obj.Data.LastAttackerID != -1)
                        {
                            CardLogic target = GetChess(obj.Data.LastAttackerID);
                            TryMoveTo(obj, target);
                        }
                        else if (!TryMoveToPrey(obj))
                        {
                            CardLogic target = GetClosestPlayer(obj.Data, PhaseType.Charactor);
                            TryMoveTo(obj, target);
                        }
                        break;
                }

                needDeletes.Add(id);
            }

            foreach (int id in needDeletes)
            {
                mTempChessList.Remove(id);
            }
            needDeletes.Clear();

            if (count == notDealNumber)
            {
                break;
            }
        }

        mTempChessList.Clear();
        mNeedRefresh = true;
    }

    void TryRandomMove(CardLogic _self)
    {
        EnemyTryMove(_self, Random.Range(-1, 2), Random.Range(-1, 2));
    }

    void TryMoveTo(CardLogic _self, CardLogic _target)
    {
        int tx = 0, ty = 0;
        if (_target.Data.X > _self.Data.X)
        {
            tx = 1;
        }
        else if (_target.Data.X < _self.Data.X)
        {
            tx = -1;
        }

        if (_target.Data.Y > _self.Data.Y)
        {
            ty = 1;
        }
        else if (_target.Data.Y < _self.Data.Y)
        {
            ty = -1;
        }
        EnemyTryMove(_self, tx, ty);
    }

    bool TryMoveToPrey(CardLogic _self)
    {
        CardData data = GameLogic.Instance.GetActionTarget(_self.Data, _self.Data.NormalAttack, 1);
        if (data != null)
        {
            TryMoveTo(_self, data.Logic);
            return true;
        }

        return false;
    }

    const float RectSize = 82.0f;
    void Refresh(bool _init)
    {
        CalculatorItems();

        for (int i = 0; i < mWidth; i++)
        {
            for (int j = 0; j < mHeight; j++)
            {
                if (mChessboardData[i, j] != -1)
                {
                    int id = mChessboardData[i, j];
                    GameObject chess = mChessList[id].gameObject;

                    Vector3 target = new Vector3((float)(i - 2) * RectSize - RectSize * 0.5f + 3, (j + 3) * RectSize + 3, 0);
                    if (_init)
                    {
                        chess.transform.localPosition = target;
                    }
                    else
                    {
                        TweenPosition.Begin(chess, 0.25f, target);
                    }
                }
            }
        }

        CalculatorBottomLine();
        Vector3 rollMap = new Vector3(0, (-mBottomLine - 2) * RectSize, 0);
        if (_init)
        {
            GameMap.localPosition = rollMap;
        }
        else
        {
            TweenPosition.Begin(GameMap.gameObject, 0.25f, rollMap);
        }

        if (_init)
        {
            if (RefreshFinishEvent != null)
            {
                RefreshFinishEvent(mBottomLine);
            }
        }
        else
        {
            Invoke("RefreshFinish", 0.4f);
        }
    }

    void CalculatorBottomLine()
    {
        int topline = -1;
        int bottomline = 99999;
        foreach (int id in mChessList.Keys)
        {
            CardLogic obj = mChessList[id];
            CardData data = obj.Data;
            if (data.Phase == PhaseType.Charactor && !data.Death)
            {
                if (data.Y > topline)
                {
                    topline = data.Y;
                }

                if (data.Y < bottomline)
                {
                    bottomline = data.Y;
                }
            }
        }

        mBottomLine = bottomline;
    }

    void CalculatorItems()
    {
        int count = GameObject.FindGameObjectsWithTag("Food").Length;
        if (count < 5)
        {
            for (int i = 0; i < mWidth; i++)
            {
                for (int j = 0; j < mHeight; j++)
                {
                    if (mItemData[i, j] == null && Random.Range(0, 100) > 95)
                    {
                        GameObject obj = null;
                        int type = Random.Range(0, 51);
                        if (type < 15)
                        {
                            GameObject perfab = Resources.Load("Perfabs/HPFood1") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 20)
                        {
                            GameObject perfab = Resources.Load("Perfabs/HPFood2") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 21)
                        {
                            GameObject perfab = Resources.Load("Perfabs/HPFood3") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 36)
                        {
                            GameObject perfab = Resources.Load("Perfabs/MPFood1") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 41)
                        {
                            GameObject perfab = Resources.Load("Perfabs/MPFood2") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 42)
                        {
                            GameObject perfab = Resources.Load("Perfabs/MPFood3") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 47)
                        {
                            GameObject perfab = Resources.Load("Perfabs/Chest1") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 50)
                        {
                            GameObject perfab = Resources.Load("Perfabs/Chest2") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else
                        {
                            GameObject perfab = Resources.Load("Perfabs/Chest3") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }

                        if (obj != null)
                        {
                            obj.transform.parent = CardRoot;
                            obj.transform.localScale = Vector3.one;
                            TweenScale.Begin(obj, 0.25f, new Vector3(2, 2, 1)).from = new Vector3(0.1f, 0.1f, 1);

                            Vector3 target = new Vector3((float)(i - 2) * RectSize - RectSize * 0.5f + 3, (j + 3) * RectSize + 3, 0);
                            obj.transform.localPosition = target;
                            mItemData[i, j] = obj.GetComponent<Food>();
                        }
                    }
                }
            }
        }
    }

    public MapItemType TryEatItem(int _x, int _y)
    {
        if (mItemData[_x, _y] != null)
        {
            MapItemType type = mItemData[_x, _y].FoodType;

            TweenScale.Begin(mItemData[_x, _y].gameObject, 0.25f, new Vector3(0.1f, 0.1f, 1)).from = new Vector3(2, 2, 1);
            Destroy(mItemData[_x, _y].gameObject, 0.25f);
            mItemData[_x, _y] = null;

            return type;
        }

        return MapItemType.None;
    }

    void RefreshFinish()
    {
        if (RefreshFinishEvent != null)
        {
            RefreshFinishEvent(mBottomLine);
        }
    }

    public CardLogic GetChess(int _x, int _y)
    {
        if (_x < 0 || _x >= mWidth || _y < 0 || _y >= mHeight)
        {
            return null;
        }

        int id = mChessboardData[_x, _y];
        return id == -1 ? null : mChessList[id];
    }

    public void ShakeMap(int _level)
    {
        int level = _level;
        if (level < 1)
        {
            level = 1;
        }
        iTween.ShakePosition(Road.parent.gameObject, new Vector3(0.05f, 0.05f, 0) * level, 0.5f * level);
    }

    public int BottomLine
    {
        get { return mBottomLine; }
    }

    public CardLogic GetChess(System.Int64 _id)
    {
        foreach (CardLogic logic in mChessList.Values)
        {
            if (logic.Data.ID == _id)
            {
                return logic;
            }
        }

        return null;
    }

    public CardLogic GetClosestPlayer(CardData _self, PhaseType _phase)
    {
        float xDis = 99999;
        float yDis = 99999;
        CardLogic result = null;
        foreach (CardLogic logic in mChessList.Values)
        {
            if (logic.Data.Phase != _phase)
            {
                continue;
            }

            float cxdis = Mathf.Abs((float)logic.Data.X - (float)_self.X) * 1.5f;
            float cydis = Mathf.Abs((float)logic.Data.Y - (float)_self.Y);
            if (cxdis + cydis < xDis + yDis)
            {
                result = logic;
            }
        }

        return result;
    }
}
