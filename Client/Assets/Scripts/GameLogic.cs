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
    public UILabel RoundLabel;

    int mRound = -1;
    bool mCalculating = false;

    void Awake()
    {
        mInstance = this;
    }

	// Use this for initialization
	void Start () {
        BattleStart();
	}
	
	// Update is called once per frame
	void Update () {
        if (mClickBuff > 0)
        {
            mClickBuff -= Time.deltaTime;
            if (mClickBuff < 0)
            {
                mClickBuff = 0;
            }
        }
        
	    if (Input.GetMouseButtonDown(0))
	    {
            if (mClickBuff <= 0)
            {
                mClickBuff = 0.2f;
            }
            else
            {
                mClickBuff = 0;
                ClickAll();
            }
	    }
	}

    float mClickBuff = 0;

    void BattleStart()
    {
        Round = 1;
        SelectAll();
    }

    int Round
    {
        get
        {
            return mRound;
        }
        set 
        {
            mRound = value;
            RoundLabel.text = "Round:" + mRound.ToString();
        }
    }

    void OnEnable()
    {
        GameChessboard.RefreshFinishEvent += GameChessboardRefreshFinish;
    }

    void OnDisable()
    {
        GameChessboard.RefreshFinishEvent -= GameChessboardRefreshFinish;
    }

    /// <summary>
    /// 向上
    /// </summary>
    public void ClickUp()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        mClickBuff = 0;
        mCalculating = true;
        GameChessboard.GoUp();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// 向下
    /// </summary>
    public void ClickDown()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        mClickBuff = 0;
        mCalculating = true;
        GameChessboard.GoDown();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// 向左
    /// </summary>
    public void ClickLeft()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        mClickBuff = 0;
        mCalculating = true;
        GameChessboard.GoLeft();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// 向右
    /// </summary>
    public void ClickRight()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        mClickBuff = 0;
        mCalculating = true;
        GameChessboard.GoRight();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// 向左上
    /// </summary>
    public void ClickUpLeft()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        mClickBuff = 0;
        mCalculating = true;
        GameChessboard.GoUpLeft();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// 向左下
    /// </summary>
    public void ClickDownLeft()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        mClickBuff = 0;
        mCalculating = true;
        GameChessboard.GoDownLeft();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// 向右上
    /// </summary>
    public void ClickUpRight()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        mClickBuff = 0;
        mCalculating = true;
        GameChessboard.GoUpRight();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// 向右下
    /// </summary>
    public void ClickDownRight()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        mClickBuff = 0;
        mCalculating = true;
        GameChessboard.GoDownRight();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// 全选或全取消
    /// </summary>
    public void ClickAll()
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

    /// <summary>
    /// 全选
    /// </summary>
    public void SelectAll()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Charactor"))
        {
            obj.GetComponent<CardLogic>().Select(true);
        }
    }

    /// <summary>
    /// 全取消
    /// </summary>
    public void UnselectAll()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Charactor"))
        {
            obj.GetComponent<CardLogic>().Select(false);
        }
    }

    public bool IsAllSelect()
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

    bool IsAnySelect()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Charactor"))
        {
            if (obj.GetComponent<CardLogic>().IsSelect)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 待机一回合
    /// </summary>
    public void ClickStay()
    {
        if (mCalculating)
        {
            return;
        }

        mClickBuff = 0;
        mCalculating = true;
        GameChessboard.AllEmemyMove();
        //CalculateAI();
    }

    private bool mIsMultiple = false;
    public bool IsMultiple
    {
        get { return mIsMultiple; }
    }

    void ClickMultiple()
    {
        mClickBuff = 0;
        mIsMultiple = !mIsMultiple;
        MultipleBtnBack.spriteName = mIsMultiple ? "BtnIcon07" : "BtnIcon06";
        MultipleBtnBack.MakePixelPerfect();
    }

    void GameChessboardRefreshFinish(int _bottomLine)
    {
        CalculateAI();
    }

    List<CardLogic> mAIActionList = new List<CardLogic>();
    void CalculateAI()
    {
        mAIActionList.Clear();

        Dictionary<int, CardLogic> list = GameChessboard.mChessList;
        foreach (int id in list.Keys)
        {
            CardLogic obj = list[id];
            mAIActionList.Add(obj);
        }

        NextAI();
    }

    void NextAI()
    {
        int fastID = GetFastCharactor();
        if (fastID != -1)
        {
            CardLogic data = mAIActionList[fastID];
            mAIActionList.RemoveAt(fastID);

            data.ActionFinishEvent += AIActionFinishCallback;
            data.CalculateAI();

            fastID = GetFastCharactor();
        }
        else
        {
            EndCalculateAI();
        }
    }

    void EndCalculateAI()
    {
        Dictionary<int, CardLogic> list = GameChessboard.mChessList;
        foreach (int id in list.Keys)
        {
            CardLogic obj = list[id];
            int halfMP = obj.Data.MPMax / 2;
            if (obj.Data.MP < halfMP)
            {
                obj.Data.MP += 5;
                if (obj.Data.MP > halfMP)
                {
                    obj.Data.MP = halfMP;
                }
            }
        }

        mCalculating = false;
        Round += 1;
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
            CardData data = mAIActionList[i].Data;
            if (fastID == -1)
            {
                fastID = i;
            }
            else if (data.Spd > mAIActionList[fastID].Data.Spd)
            {
                fastID = i;
            }
        }

        return fastID;
    }

    public CardData GetActionTarget(CardData _selfData, SkillData _skill)
    {
        for (int i = _selfData.X - _skill.Range; i <= _selfData.X + _skill.Range; i++)
        {
            for (int j = _selfData.Y - _skill.Range; j <= _selfData.Y + _skill.Range; j++)
            {
                CardLogic chess = GameChessboard.GetChess(i, j);
                switch (_skill.TargetPhase)
                {
                    case AttackTargetType.Ememy:
                        if (chess != null && !chess.Data.Death && chess.Data.Phase != _selfData.Phase)
                        {
                            return chess.Data;
                        }
                        break;
                    case AttackTargetType.Self:
                        if (chess != null && !chess.Data.Death && chess.Data.ID != _selfData.ID)
                        {
                            return chess.Data;
                        }
                        break;
                    case AttackTargetType.Team:
                        if (chess != null && !chess.Data.Death && chess.Data.Phase == _selfData.Phase)
                        {
                            return chess.Data;
                        }
                        break;
                    case AttackTargetType.All:
                        if (chess != null && !chess.Data.Death)
                        {
                            return chess.Data;
                        }
                        break;
                }
            }
        }

        return null;
    }

    public IEnumerable<CardData> GetActionTargets(CardData _selfData, SkillData _skill)
    {
        for (int i = _selfData.X - _skill.Range; i <= _selfData.X + _skill.Range; i++)
        {
            for (int j = _selfData.Y - _skill.Range; j <= _selfData.Y + _skill.Range; j++)
            {
                CardLogic chess = GameChessboard.GetChess(i, j);
                switch (_skill.TargetPhase)
                {
                    case AttackTargetType.Ememy:
                        if (chess != null && !chess.Data.Death && chess.Data.Phase != _selfData.Phase)
                        {
                            yield return chess.Data;
                        }
                        break;
                    case AttackTargetType.Self:
                        if (chess != null && !chess.Data.Death && chess.Data.ID != _selfData.ID)
                        {
                            yield return chess.Data;
                        }
                        break;
                    case AttackTargetType.Team:
                        if (chess != null && !chess.Data.Death && chess.Data.Phase == _selfData.Phase)
                        {
                            yield return chess.Data;
                        }
                        break;
                    case AttackTargetType.All:
                        if (chess != null && !chess.Data.Death)
                        {
                            yield return chess.Data;
                        }
                        break;
                }
            }
        }
    }
}
