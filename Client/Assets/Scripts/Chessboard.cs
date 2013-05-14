using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chessboard : MonoBehaviour {
    private int[,] mChessboardData = null;
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

        {
            GameObject card = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
            card.transform.parent = CardRoot;
            card.transform.localScale = Vector3.one;
            int id = mChessList.Keys.Count;
            CardLogic logic = card.GetComponent<CardLogic>();
            logic.Data.ResetAllData(id, ClassType.WaterSaber, PhaseType.Charactor);

            mChessList.Add(id, logic);
            InitlizeChess(1, 0, id);
        }
        {
            GameObject card = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
            card.transform.parent = CardRoot;
            card.transform.localScale = Vector3.one;
            int id = mChessList.Keys.Count;
            CardLogic logic = card.GetComponent<CardLogic>();
            logic.Data.ResetAllData(id, ClassType.SLMGirl, PhaseType.Charactor);

            mChessList.Add(id, logic);
            InitlizeChess(2, 0, id);
        }
        {
            GameObject card = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
            card.transform.parent = CardRoot;
            card.transform.localScale = Vector3.one;
            int id = mChessList.Keys.Count;
            CardLogic logic = card.GetComponent<CardLogic>();
            logic.Data.ResetAllData(id, ClassType.LightPastor, PhaseType.Charactor);

            mChessList.Add(id, logic);
            InitlizeChess(3, 0, id);
        }
        {
            GameObject card = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
            card.transform.parent = CardRoot;
            card.transform.localScale = Vector3.one;
            int id = mChessList.Keys.Count;
            CardLogic logic = card.GetComponent<CardLogic>();
            logic.Data.ResetAllData(id, ClassType.GreenArrow, PhaseType.Charactor);

            mChessList.Add(id, logic);
            InitlizeChess(4, 0, id);
        }

        for (int i = 0; i < 10; i++ )
        {
            GameObject card = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
            card.transform.parent = CardRoot;
            card.transform.localScale = Vector3.one;
            int id = mChessList.Keys.Count;
            CardLogic logic = card.GetComponent<CardLogic>();
            logic.Data.ResetAllData(id, (ClassType)Random.Range(0, (int)ClassType.Max), PhaseType.Enemy);

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
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                mChessboardData[i, j] = -1;
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

                if (x < 0 || x >= mWidth || y < 0 || y >= mHeight || y < mBottomLine || y > mBottomLine + 7)
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
                if (obj.Data.EnemyAI == AIType.Slime)
                {
                    CardData target = GameLogic.Instance.GetActionTarget(obj.Data, PhaseType.Charactor);
                    if (target == null && Random.Range(0, 100) < 33)
                    {
                        EnemyTryMove(obj, Random.Range(-1, 2), Random.Range(-1, 2));
                    }
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
            Invoke("RefreshFinish", 0.25f);
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
}
