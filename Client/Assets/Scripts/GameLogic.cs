using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {
    private static GameLogic mInstance = null;
    public static GameLogic Instance
    {
        get
        {
            return mInstance;
        }
    }

    public Chessboard GameChessboard;

    public UISprite MultipleBtnBack;

    bool mLockControl = false;

    void Awake()
    {
        mInstance = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        GameChessboard.RefreshFinishEvent += GameChessboardRefreshFinish;
    }

    void OnDisable()
    {
        GameChessboard.RefreshFinishEvent -= GameChessboardRefreshFinish;
    }

    void ClickUp()
    {
        if (mLockControl)
        {
            return;
        }

        mLockControl = true;
        GameChessboard.GoUp();
    }

    void ClickDown()
    {
        if (mLockControl)
        {
            return;
        }

        mLockControl = true;
        GameChessboard.GoDown();
    }

    void ClickLeft()
    {
        if (mLockControl)
        {
            return;
        }

        mLockControl = true;
        GameChessboard.GoLeft();
    }

    void ClickRight()
    {
        if (mLockControl)
        {
            return;
        }

        mLockControl = true;
        GameChessboard.GoRight();
    }

    void ClickUpLeft()
    {
        if (mLockControl)
        {
            return;
        }

        mLockControl = true;
        GameChessboard.GoUpLeft();
    }

    void ClickDownLeft()
    {
        if (mLockControl)
        {
            return;
        }

        mLockControl = true;
        GameChessboard.GoDownLeft();
    }

    void ClickUpRight()
    {
        if (mLockControl)
        {
            return;
        }

        mLockControl = true;
        GameChessboard.GoUpRight();
    }

    void ClickDownRight()
    {
        if (mLockControl)
        {
            return;
        }

        mLockControl = true;
        GameChessboard.GoDownRight();
    }

    void ClickAll()
    {
        if (IsAnyUnselect())
        {
            SelectAll();
        }
        else if (IsAllSelect())
        {
            UnselectAll();
        }
    }

    void SelectAll()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Charactor"))
        {
            obj.GetComponent<CardLogic>().Select(true);
        }
    }

    public void UnselectAll()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Charactor"))
        {
            obj.GetComponent<CardLogic>().Select(false);
        }
    }

    bool IsAllSelect()
    {
        return !IsAnyUnselect();
    }

    bool IsAnyUnselect()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Charactor"))
        {
            if (!obj.GetComponent<CardLogic>().IsSelect)
            {
                return true;
            }
        }

        return false;
    }

    void ClickStay()
    {
        if (mLockControl)
        {
            return;
        }

        mLockControl = true;
        CalculatorAI();
    }

    private bool mIsMultiple = false;
    public bool IsMultiple
    {
        get { return mIsMultiple; }
    }

    void ClickMultiple()
    {
        mIsMultiple = !mIsMultiple;
        MultipleBtnBack.spriteName = mIsMultiple ? "UIText06" : "UIText07";
    }

    void GameChessboardRefreshFinish(int _bottomLine)
    {
        CalculatorAI();
    }

    List<GameObject> mAIActionList = new List<GameObject>();
    void CalculatorAI()
    {
        mAIActionList.Clear();

        Dictionary<int, GameObject> list = GameChessboard.mChessList;
        foreach (int id in list.Keys)
        {
            GameObject obj = list[id];
            mAIActionList.Add(obj);
        }

        NextAI();
    }

    void NextAI()
    {
        int fastID = GetFastCharactor();
        if (fastID != -1)
        {
            CardLogic data = mAIActionList[fastID].GetComponent<CardLogic>();
            mAIActionList.RemoveAt(fastID);
            data.ActionFinishEvent += AIActionFinishCallback;
            data.CalculatorAI();

            fastID = GetFastCharactor();
        }
        else
        {
            mLockControl = false;
        }
    }

    void AIActionFinishCallback(CardLogic _logic)
    {
        _logic.ActionFinishEvent -= AIActionFinishCallback;
        NextAI();
    }

    int GetFastCharactor()
    {
        if (mAIActionList.Count <= 0)
        {
            return -1;
        }

        int fastID = -1;
        for (int i = 0; i < mAIActionList.Count; i++ )
        {
            CardData data = mAIActionList[i].GetComponent<CardData>();
            if (fastID == -1)
            {
                fastID = i;
            }
            else if (data.Spd > mAIActionList[fastID].GetComponent<CardData>().Spd)
            {
                fastID = i;
            }
        }

        return fastID;
    }

    public CardData GetActionTarget(CardData _selfData, PhaseType _targetPhase)
    {
        for (int i = _selfData.X - _selfData.ActionRange; i <= _selfData.X + _selfData.ActionRange; i++)
        {
            for (int j = _selfData.Y - _selfData.ActionRange; j <= _selfData.Y + _selfData.ActionRange; j++)
            {
                if (i == _selfData.X && j == _selfData.Y)
                {
                    continue;
                }

                GameObject chess = GameChessboard.GetChess(i, j);
                if (chess != null
                    && chess.GetComponent<CardData>().Phase == _targetPhase
                    && !chess.GetComponent<CardData>().Death)
                {
                    return chess.GetComponent<CardData>();
                }
            }
        }

        return null;
    }
}
