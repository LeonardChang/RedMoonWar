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
    public GameObject InfomationPanel;
    public GameObject BigButtonPanel;
    public GameObject MenuPanel;

    public UILabel CoinLabel;
    public UILabel CardLabel;
    public UILabel AppleLabel;

    public UIImageButton StayBtn;
    public UIImageButton MultiBtn;
    public UISprite BigBtn;

    public UILabel SoundBtnLabel;
    public UILabel MusicBtnLabel;

    public UIPanel LoseResultPanel;
    public UIPanel WinResultPanel;
    public Transform WinLoseEffectRoot;

    int mRound = -1;
    int mGetCoin = 0;
    int mGetCard = 0;
    int mApple = 10000;
    bool mCalculating = false;

    private int mLeaderSkill1 = 0; // 主将技能1
    private int mLeaderSkill2 = 0; // 主将技能2
    private int mLeaderSkillAddHP_SP = 0;

    float mClickBuff = 0;
    bool mBigButtonOpen = false;

    public int LeaderSkill1
    {
        get
        {
            return mLeaderSkill1;
        }
    }

    public int LeaderSkill2
    {
        get
        {
            return mLeaderSkill2;
        }
    }

    bool mGameStart = true;

    void Awake()
    {
        mInstance = this;
    }

	// Use this for initialization
	void Start () {
        //CacheLoader.Instance.CheckAllCache();
        //Invoke("BattleStart", 5);

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
                mClickBuff = 0.3f;
            }
            else
            {
                CleanClickBuff();
                ClickAll();
            }
	    }
	}

    void OnSwipe(SwipeGesture gesture)
    {
        switch (gesture.Direction)
        {
            case FingerGestures.SwipeDirection.Up:
                if (!mBigButtonOpen && gesture.StartPosition.y <= Screen.height * 0.11f)
                {
                    OpenBigButton();
                }
                else
                {
                    ClickUp();
                }
                break;
            case FingerGestures.SwipeDirection.Down:
                if (mBigButtonOpen && gesture.StartPosition.y <= Screen.height * 0.33f)
                {
                    HideBigButton();
                }
                else
                {
                    ClickDown();
                }
                break;
            case FingerGestures.SwipeDirection.Left:
                ClickLeft();
                break;
            case FingerGestures.SwipeDirection.Right:
                ClickRight();
                break;
            case FingerGestures.SwipeDirection.UpperLeftDiagonal:
                ClickUpLeft();
                break;
            case FingerGestures.SwipeDirection.UpperRightDiagonal:
                ClickUpRight();
                break;
            case FingerGestures.SwipeDirection.LowerLeftDiagonal:
                ClickDownLeft();
                break;
            case FingerGestures.SwipeDirection.LowerRightDiagonal:
                ClickDownRight();
                break;
        }
    }
    
    void OpenBigButton()
    {
        if (mBigButtonOpen)
        {
            return;
        }

        mBigButtonOpen = true;
        TweenPosition.Begin(BigButtonPanel, 0.25f, new Vector3(0, 0, 0));
    }

    void HideBigButton()
    {
        if (!mBigButtonOpen)
        {
            return;
        }

        mBigButtonOpen = false;
        TweenPosition.Begin(BigButtonPanel, 0.25f, new Vector3(0, -207, 0));
    }

    void BattleStart()
    {
        Round = 1;
        Apple = 1000;
        Coin = 0;
        Card = 0;

        Stage stage = Stage.Instance;
        mLeaderSkill1 = CardManager.Instance.GetCard(stage.Players[0].CardID).LeaderSkillID;
        mLeaderSkill2 = CardManager.Instance.GetCard(stage.Players[1].CardID).LeaderSkillID;

        mLeaderSkillAddHP_SP = 0;
        if (mLeaderSkill1 == (int)SpecialLeaderSkillID.GodHP)
        {
            mLeaderSkillAddHP_SP += Mathf.FloorToInt(stage.Players[0].Atk * LeaderSkillManager.Instance.GetSkill(mLeaderSkill1).Special);
        }
        if (mLeaderSkill2 == (int)SpecialLeaderSkillID.GodHP)
        {
            mLeaderSkillAddHP_SP += Mathf.FloorToInt(stage.Players[1].Atk * LeaderSkillManager.Instance.GetSkill(mLeaderSkill2).Special);
        }

        // 置入关卡数据 
        GameChessboard.Initlize(stage);

        SelectAll();

        mGameStart = true;
    }

    public int Round
    {
        get
        {
            return mRound;
        }
        set 
        {
            mRound = value;
        }
    }

    public int Coin
    {
        get
        {
            return mGetCoin;
        }
        set
        {
            LabelTweener.Begin(CoinLabel.gameObject, 0.5f, mGetCoin, value);
            mGetCoin = value;
        }
    }

    public int Card
    {
        get
        {
            return mGetCard;
        }
        set
        {
            LabelTweener.Begin(CardLabel.gameObject, 0.5f, mGetCard, value);
            mGetCard = value;
        }
    }

    public int Apple
    {
        get
        {
            return mApple;
        }
        set
        {
            LabelTweener.Begin(AppleLabel.gameObject, 0.5f, mApple, value);
            mApple = value;
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
        if (!mGameStart || mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoUp();
        GameChessboard.AllEmemyMove();

        EnableBigButtonPanel = false;
    }

    /// <summary>
    /// 向下
    /// </summary>
    public void ClickDown()
    {
        if (!mGameStart || mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoDown();
        GameChessboard.AllEmemyMove();

        EnableBigButtonPanel = false;
    }

    /// <summary>
    /// 向左
    /// </summary>
    public void ClickLeft()
    {
        if (!mGameStart || mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoLeft();
        GameChessboard.AllEmemyMove();

        EnableBigButtonPanel = false;
    }

    /// <summary>
    /// 向右
    /// </summary>
    public void ClickRight()
    {
        if (!mGameStart || mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoRight();
        GameChessboard.AllEmemyMove();

        EnableBigButtonPanel = false;
    }

    /// <summary>
    /// 向左上
    /// </summary>
    public void ClickUpLeft()
    {
        if (!mGameStart || mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoUpLeft();
        GameChessboard.AllEmemyMove();

        EnableBigButtonPanel = false;
    }

    /// <summary>
    /// 向左下
    /// </summary>
    public void ClickDownLeft()
    {
        if (!mGameStart || mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoDownLeft();
        GameChessboard.AllEmemyMove();

        EnableBigButtonPanel = false;
    }

    /// <summary>
    /// 向右上
    /// </summary>
    public void ClickUpRight()
    {
        if (!mGameStart || mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoUpRight();
        GameChessboard.AllEmemyMove();

        EnableBigButtonPanel = false;
    }

    /// <summary>
    /// 向右下
    /// </summary>
    public void ClickDownRight()
    {
        if (!mGameStart || mCalculating || !IsAnySelect())
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.GoDownRight();
        GameChessboard.AllEmemyMove();

        EnableBigButtonPanel = false;
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
        if (!mGameStart || mCalculating)
        {
            return;
        }

        CleanClickBuff();
        mCalculating = true;
        GameChessboard.AllEmemyMove();
        //CalculateAI();

        EnableBigButtonPanel = false;
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
        Dictionary<int, CardLogic> list = GameChessboard.mChessList;
        foreach (int id in list.Keys)
        {
            CardLogic obj = list[id];
            MapItemType type = GameChessboard.TryEatItem(obj.Data.X, obj.Data.Y);
            switch (type)
            {
                case MapItemType.HPFood1:
                    {
                        int value = Mathf.FloorToInt(obj.Data.HPMax * 0.1f);
                        obj.Data.HP += value;
                        obj.CreateTAnimation("HealthHP").transform.localScale *= 0.5f;
                        obj.CreateHealthNumber(value);
                    }
                    break;
                case MapItemType.HPFood2:
                    {
                        int value = Mathf.FloorToInt(obj.Data.HPMax * 0.5f);
                        obj.Data.HP += value;
                        obj.CreateTAnimation("HealthHP").transform.localScale *= 0.5f;
                        obj.CreateHealthNumber(value);
                    }
                    break;
                case MapItemType.HPFood3:
                    {
                        int value = obj.Data.HPMax;
                        obj.Data.HP += value;
                        obj.CreateTAnimation("HealthHP").transform.localScale *= 0.5f;
                        obj.CreateHealthNumber(value);
                    }
                    break;
                case MapItemType.MPFood1:
                    {
                        int value = 15;
                        obj.Data.MP += value;
                        obj.CreateTAnimation("HealthMP").transform.localScale *= 0.5f;
                        obj.CreateHealManaNumber(value);
                    }
                    break;
                case MapItemType.MPFood2:
                    {
                        int value = Mathf.FloorToInt(obj.Data.MPMax * 0.5f);
                        obj.Data.MP += value;
                        obj.CreateTAnimation("HealthMP").transform.localScale *= 0.5f;
                        obj.CreateHealManaNumber(value);
                    }
                    break;
                case MapItemType.MPFood3:
                    {
                        int value = obj.Data.MPMax;
                        obj.Data.MP += value;
                        obj.CreateTAnimation("HealthMP").transform.localScale *= 0.5f;
                        obj.CreateHealManaNumber(value);
                    }
                    break;
                case MapItemType.Chest1:
                    if (obj.Data.Phase == PhaseType.Charactor)
                    {
                        Coin += 100;
                        obj.CreateTAnimation("GetCoin");
                    }
                    break;
                case MapItemType.Chest2:
                    if (obj.Data.Phase == PhaseType.Charactor)
                    {
                        Coin += 1000;
                        obj.CreateTAnimation("GetCoin");
                    }
                    break;
                case MapItemType.Chest3:
                    if (obj.Data.Phase == PhaseType.Charactor)
                    {
                        Coin += 5000;
                        obj.CreateTAnimation("GetCoin");
                    }
                    break;
            }
        }
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
            // 自然回复
            list[id].Data.MP += 2;

            // 队长技能回复
            if (list[id].Data.Phase == PhaseType.Charactor)
            {
                LeaderSkillData skill1 = LeaderSkillManager.Instance.GetSkill(LeaderSkill1);
                LeaderSkillData skill2 = LeaderSkillManager.Instance.GetSkill(LeaderSkill2);
                if (skill1 != null 
                    && (skill1.Element == ElementType.None || skill1.Element == list[id].Data.Element))
                {
                    list[id].Data.HP += skill1.AddHP;
                    list[id].Data.MP += skill1.AddMP;
                }
                if (skill2 != null
                    && (skill2.Element == ElementType.None || skill2.Element == list[id].Data.Element))
                {
                    list[id].Data.HP += skill2.AddHP;
                    list[id].Data.MP += skill2.AddMP;
                }

                list[id].Data.HP += mLeaderSkillAddHP_SP;
            }
        }

        mCalculating = false;
        Round += 1;

        EnableBigButtonPanel = true;

        if (GameChessboard.AliveCharactorNumber <= 0)
        {
            GameLose();
        }
        else if (GameChessboard.AliveEnemyNumber <= 0)
        {
            GameWin();
        }
    }

    bool EnableBigButtonPanel
    {
        set 
        {
            if (value)
            {
                StayBtn.isEnabled = true;
                MultiBtn.isEnabled = true;
                BigBtn.spriteName = "ControlBtn";
            }
            else
            {
                StayBtn.isEnabled = false;
                MultiBtn.isEnabled = false;
                BigBtn.spriteName = "ControlBtn_disable";
            }
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
        return GetActionTarget(_selfData, _skill, 0);
    }

    public CardData GetActionTarget(CardData _selfData, SkillData _skill, int addRange)
    {
        int range = _skill.Range + addRange;
        for (int i = _selfData.X - range; i <= _selfData.X + range; i++)
        {
            for (int j = _selfData.Y - range; j <= _selfData.Y + range; j++)
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
        if (_skill.ID == (int)SpecialSkillID.AttackAll1 || _skill.ID == (int)SpecialSkillID.AttackAll2)
        {
            for (int i = 0; i <= GameChessboard.Width; i++)
            {
                for (int j = 0; j <= GameChessboard.Height; j++)
                {
                    CardLogic chess = GameChessboard.GetChess(i, j);
                    if (chess != null && !chess.Data.Death && chess.Data.Phase != _selfData.Phase)
                    {
                        yield return chess.Data;
                    }
                }
            }
        }
        else
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

    public void ShowInfomation(CardData _data)
    {
        InfomationPanel.GetComponent<UIInformation>().StoreData = _data;
        InfomationPanel.SetActive(true);

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
        InfomationPanel.GetComponent<UIInformation>().StoreData = null;
        InfomationPanel.SetActive(false);
    }

    public void ShowMenu()
    {
        MenuPanel.SetActive(true);

        SoundBtnLabel.text = Localization.instance.Get("Sound " + (AudioCenter.Instance.OpenSoundEffect ? "ON" : "OFF"));
        MusicBtnLabel.text = Localization.instance.Get("Music " + (AudioCenter.Instance.OpenMusicEffect ? "ON" : "OFF"));

        TweenScale ts = TweenScale.Begin(MenuPanel, 0.15f, Vector3.one);
        ts.from = new Vector3(1, 0.01f, 1);
        ts.eventReceiver = null;
        ts.callWhenFinished = "";
    }

    public void HideMenu()
    {
        TweenScale ts = TweenScale.Begin(MenuPanel, 0.15f, new Vector3(1, 0.01f, 1));
        ts.eventReceiver = gameObject;
        ts.callWhenFinished = "RealHideMenu";
    }

    void RealHideMenu()
    {
        MenuPanel.SetActive(false);
    }

    public void ClickSound()
    {
        AudioCenter.Instance.OpenSoundEffect = !AudioCenter.Instance.OpenSoundEffect;
        SoundBtnLabel.text = Localization.instance.Get("Sound " + (AudioCenter.Instance.OpenSoundEffect ? "ON" : "OFF"));
        MusicBtnLabel.text = Localization.instance.Get("Music " + (AudioCenter.Instance.OpenMusicEffect ? "ON" : "OFF"));
    }

    public void ClickMusic()
    {
        AudioCenter.Instance.OpenMusicEffect = !AudioCenter.Instance.OpenMusicEffect;
        SoundBtnLabel.text = Localization.instance.Get("Sound " + (AudioCenter.Instance.OpenSoundEffect ? "ON" : "OFF"));
        MusicBtnLabel.text = Localization.instance.Get("Music " + (AudioCenter.Instance.OpenMusicEffect ? "ON" : "OFF"));
    }

    public void ClickQuit()
    {
        HideMenu();
    }

    public void ShakeMap(int _level)
    {
        GameChessboard.ShakeMap(_level);
    }
   
    public int BottomLine
    {
        get { return GameChessboard.BottomLine; }
    }

    void GameWin()
    {
        mGameStart = false;

        WinResultPanel.gameObject.SetActive(true);
        TweenAlpha.Begin(WinResultPanel.gameObject, 0.25f, 1).from = 0.001f;

        GameObject prefab = Resources.Load("Perfabs/WinPanel") as GameObject;
        GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

        obj.transform.parent = WinLoseEffectRoot;
        obj.transform.localPosition = new Vector3(0, 0, -100);
        obj.transform.localScale = Vector3.one;
    }

    void GameLose()
    {
        mGameStart = false;

        LoseResultPanel.gameObject.SetActive(true);
        TweenAlpha.Begin(LoseResultPanel.gameObject, 0.25f, 1).from = 0.001f;

        GameObject prefab = Resources.Load("Perfabs/LosePanel") as GameObject;
        GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

        obj.transform.parent = WinLoseEffectRoot;
        obj.transform.localPosition = new Vector3(0, 0, -100);
        obj.transform.localScale = Vector3.one;
    }

    void ClickResurrection()
    {

    }

    void ClickQuitBattle()
    {
        Application.Quit();
    }
}
