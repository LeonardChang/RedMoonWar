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
    public GameObject InfomationPanel;

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
                CleanClickBuff();
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
                CleanClickBuff();
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
    /// ����
    /// </summary>
    public void ClickUp()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoUp();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// ����
    /// </summary>
    public void ClickDown()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoDown();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// ����
    /// </summary>
    public void ClickLeft()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoLeft();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// ����
    /// </summary>
    public void ClickRight()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoRight();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// ������
    /// </summary>
    public void ClickUpLeft()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoUpLeft();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// ������
    /// </summary>
    public void ClickDownLeft()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoDownLeft();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// ������
    /// </summary>
    public void ClickUpRight()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoUpRight();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// ������
    /// </summary>
    public void ClickDownRight()
    {
        if (mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoDownRight();
        GameChessboard.AllEmemyMove();
    }

    /// <summary>
    /// ȫѡ��ȫȡ��
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
    /// ȫѡ
    /// </summary>
    public void SelectAll()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Charactor"))
        {
            obj.GetComponent<CardLogic>().Select(true);
        }
    }

    /// <summary>
    /// ȫȡ��
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
    /// ����һ�غ�
    /// </summary>
    public void ClickStay()
    {
        if (mCalculating)
        {
            return;
        }

        CleanClickBuff();
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
        CleanClickBuff();
        mIsMultiple = !mIsMultiple;
        MultipleBtnBack.spriteName = mIsMultiple ? "BtnIcon07" : "BtnIcon06";
        MultipleBtnBack.MakePixelPerfect();
    }

    public void CleanClickBuff()
    {
        mClickBuff = 0;
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
        // ������ÿ�غϻ�5��Mana
        Dictionary<int, CardLogic> list = GameChessboard.mChessList;
        foreach (int id in list.Keys)
        {
            list[id].Data.MP += 5;
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

    public void ShowInfomation(CardData _data)
    {
        InfomationPanel.GetComponent<UIInformation>().StoreData = _data;
        InfomationPanel.SetActiveRecursively(true);

        TweenScale ts = TweenScale.Begin(InfomationPanel, 0.15f, Vector3.one);
        ts.from = new Vector3(1, 0.01f, 1);
        ts.eventReceiver = null;
        ts.callWhenFinished = "";
    }

    public void HideInfomation()
    {
        TweenScale ts = TweenScale.Begin(InfomationPanel, 0.15f, new Vector3(1, 0.01f, 1));
        ts.eventReceiver = gameObject;
        ts.callWhenFinished = "RealHideInfomation";
    }

    void RealHideInfomation()
    {
        InfomationPanel.SetActiveRecursively(false);
    }
}