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
        Initlize(6, 50);

        mChessList.Clear();
        Object perfab = Resources.Load("Cards/Perfabs/Card");

        for (int i = 0; i < 5; i++)
        {
            CharacterData data = CharacterManager.Instance.GetCharacter(i);

            GameObject card = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
            card.name = "Char" + data.ID.ToString();

            card.transform.parent = CardRoot;
            card.transform.localScale = Vector3.one;
            int id = mChessList.Keys.Count;
            CardLogic logic = card.GetComponent<CardLogic>();
            logic.Data.ResetAllData(PhaseType.Charactor, data);

            mChessList.Add(id, logic);
            InitlizeChess(0 + i, 1, id);
        }

        {
            CharacterData data = CharacterManager.Instance.CreateRandomCharactor(10000);

            GameObject card = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
            card.name = "Char" + data.ID.ToString();

            card.transform.parent = CardRoot;
            card.transform.localScale = Vector3.one;
            int id = mChessList.Keys.Count;
            CardLogic logic = card.GetComponent<CardLogic>();
            logic.Data.ResetAllData(PhaseType.Charactor, data);

            mChessList.Add(id, logic);
            InitlizeChess(5, 1, id);
        }

        for (int i = 0; i < 13; i++)
        {
            CharacterData data = CharacterManager.Instance.CreateRandomCharactor(10001 + i);

            GameObject card = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
            card.name = "Char" + data.ID.ToString();

            card.transform.parent = CardRoot;
            card.transform.localScale = Vector3.one;
            int id = mChessList.Keys.Count;
            CardLogic logic = card.GetComponent<CardLogic>();
            logic.Data.ResetAllData(PhaseType.Enemy, data);

            mChessList.Add(id, logic);
            InitlizeChess(Random.Range(0, 6), 10 + i * 3, id);
        }

        mBottomLine = 0;
        Refresh(true);
    }

	// Use this for initialization
	void Start () {
	
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

    void Initlize(int _width, int _height)
    {
        mWidth = _width;
        mHeight = _height;
        mChessboardData = new int[_width, _height];
        mItemData = new Food[_width, _height];
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                mChessboardData[i, j] = -1;
                mItemData[i, j] = null;
            }
        }

        TopRoad.localPosition = new Vector3(0, (mHeight - 1) * RectSize, 0);
        Road.localPosition = TopRoad.localPosition;
        Road.localScale = new Vector3(640, (mHeight + 10) * RectSize, 1);
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
                    case AIType.Retarded:
                    case AIType.Slime:
                        {
                            CardData target = GameLogic.Instance.GetActionTarget(obj.Data, obj.Data.NormalAttack);
                            if (target == null && Random.Range(0, 100) < 33)
                            {
                                EnemyTryMove(obj, Random.Range(-1, 2), Random.Range(-1, 2));
                            }
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
                    if (mItemData[i, j] == null)
                    {
                        GameObject obj = null;

                        int type = Random.Range(0, 500);
                        if (type < 2)
                        {
                            GameObject perfab = Resources.Load("Perfabs/HPFood1") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 3)
                        {
                            GameObject perfab = Resources.Load("Perfabs/HPFood2") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 4)
                        {
                            GameObject perfab = Resources.Load("Perfabs/HPFood3") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 6)
                        {
                            GameObject perfab = Resources.Load("Perfabs/MPFood1") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 7)
                        {
                            GameObject perfab = Resources.Load("Perfabs/MPFood2") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 8)
                        {
                            GameObject perfab = Resources.Load("Perfabs/MPFood3") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 9)
                        {
                            GameObject perfab = Resources.Load("Perfabs/Chest1") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 10)
                        {
                            GameObject perfab = Resources.Load("Perfabs/Chest3") as GameObject;
                            obj = Instantiate(perfab) as GameObject;
                        }
                        else if (type < 11)
                        {
                            GameObject perfab = Resources.Load("Perfabs/Chest1") as GameObject;
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

        //for (int i = 0; i < mWidth; i++)
        //{
        //    for (int j = 0; j < mHeight; j++)
        //    {
        //        if (mItemData[i, j] != null && Random.Range(0, 10) == 0)
        //        {
        //            TweenScale.Begin(mItemData[i, j].gameObject, 0.25f, new Vector3(0.1f, 0.1f, 1)).from = new Vector3(2, 2, 1);
        //            Destroy(mItemData[i, j].gameObject, 0.25f);
        //            mItemData[i, j] = null;
        //        }
        //    }
        //}
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
        iTween.ShakePosition(GameMap.gameObject, new Vector3(0.05f, 0.05f, 0) * level, 0.5f * level);
    }
}
