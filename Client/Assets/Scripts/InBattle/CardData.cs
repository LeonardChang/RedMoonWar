using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardData : MonoBehaviour {
    private int mHP = 100;
    private int mMP = 0;
    private PhaseType mPhase = PhaseType.Charactor;

    private int mX = 0;
    private int mY = 0;

    private bool mDeath = false;

    private AIType mEnemyAI = AIType.Slime;

    private System.Int64 mLastAttackerID = -1;
    private int mAttackerHatred = 0;

    private bool mAutoSkill = true;
    private bool mIsLeader = false;

    private Dictionary<int, RealBuffData> mBuffList = new Dictionary<int, RealBuffData>();

    /// <summary>
    /// 棋子在棋盘中的ID
    /// </summary>
    public System.Int64 ID
    {
        get
        {
            return mCharacterData.ID;
        }
    }

    /// <summary>
    /// 最大HP
    /// </summary>
    public int HPMax
    {
        get
        {
            return mCharacterData.MaxHP;
        }
    }

    /// <summary>
    /// 当前HP
    /// </summary>
    public int HP
    {
        get
        {
            return mHP;
        }
        set
        {
            if (value < mHP)
            {
                // HP伤害，如果睡着了就醒来
                RemoveBuff((int)BuffEnum.Sleep);
            }

            mHP = value;
            if (mHP < 0)
            {
                mHP = 0;
            }
            if (mHP > HPMax)
            {
                mHP = HPMax;
            }

            UI.Blood = (float)mHP / HPMax;

            if (mHP == 0)
            {
                mDeath = true;
                Logic.CreateDeathEffect();
                ClearAllDebuff();
            }
        }
    }

    /// <summary>
    /// 最大MP
    /// </summary>
    public int MPMax
    {
        get
        {
            return mCharacterData.MaxMP;
        }
    }

    /// <summary>
    /// 当前MP
    /// </summary>
    public int MP
    {
        get
        {
            return mMP;
        }
        set
        {
            mMP = value;
            if (mMP < 0)
            {
                mMP = 0;
            }
            if (mMP > MPMax)
            {
                mMP = MPMax;
            }

            UI.Mana = (float)mMP / MPMax;
        }
    }

    /// <summary>
    /// 普通攻击
    /// </summary>
    public SkillData NormalAttack
    {
        get
        {
            CardBaseData carddata = CardManager.Instance.GetCard(mCharacterData.CardID);
            return SkillManager.Instance.GetSkill(carddata.NormalAttackSkillID);
        }
    }

    /// <summary>
    /// 技能
    /// </summary>
    public SkillData Skill
    {
        get
        {
            CardBaseData carddata = CardManager.Instance.GetCard(mCharacterData.CardID);
            return SkillManager.Instance.GetSkill(carddata.SkillID);
        }
    }

    /// <summary>
    /// 技能等级
    /// </summary>
    public int SkillLevel
    {
        get
        {
            return mCharacterData.SkillLevel;
        }
    }

    /// <summary>
    /// 主将技
    /// </summary>
    public SkillData LeaderSkill
    {
        get
        {
            CardBaseData carddata = CardManager.Instance.GetCard(mCharacterData.CardID);
            return SkillManager.Instance.GetSkill(carddata.LeaderSkillID);
        }
    }

    /// <summary>
    /// 势力
    /// </summary>
    public PhaseType Phase
    {
        get
        {
            return mPhase;
        }
        set
        {
            mPhase = value;
            UI.Phase = mPhase;
        }
    }

    /// <summary>
    /// 属性
    /// </summary>
    public ElementType Element
    {
        get
        {
            CardBaseData carddata = CardManager.Instance.GetCard(mCharacterData.CardID);
            return carddata.Element;
        }
    }

    /// <summary>
    /// 被克制的属性
    /// </summary>
    public ElementType BeElement
    {
        get
        {
            switch (Element)
            {
                case ElementType.Fire:
                    return ElementType.Water;
                case ElementType.Water:
                    return ElementType.Earth;
                case ElementType.Wind:
                    return ElementType.Fire;
                case ElementType.Earth:
                    return ElementType.Wind;
                case ElementType.Light:
                    return ElementType.Dark;
                case ElementType.Dark:
                    return ElementType.Light;
            }

            return ElementType.Dark;
        }
    }

    /// <summary>
    /// 克制的属性
    /// </summary>
    public ElementType FoElement
    {
        get
        {
            switch (Element)
            {
                case ElementType.Fire:
                    return ElementType.Wind;
                case ElementType.Water:
                    return ElementType.Fire;
                case ElementType.Wind:
                    return ElementType.Earth;
                case ElementType.Earth:
                    return ElementType.Water;
                case ElementType.Light:
                    return ElementType.Dark;
                case ElementType.Dark:
                    return ElementType.Light;
            }

            return ElementType.Dark;
        }
    }

    /// <summary>
    /// 攻击力
    /// </summary>
    public int Atk
    {
        get
        {
            int addAtk = 0;
            foreach (RealBuffData buff in CurrentBuff)
            {
                addAtk += buff.mAddAtk;
            }
            return mCharacterData.Atk + addAtk;
        }
    }

    /// <summary>
    /// 防御力
    /// </summary>
    public int Def
    {
        get
        {
            int addDef = 0;
            foreach (RealBuffData buff in CurrentBuff)
            {
                addDef += buff.mAddDef;
            }
            return mCharacterData.Def + addDef;
        }
    }

    /// <summary>
    /// 速度
    /// </summary>
    public int Spd
    {
        get
        {
            int addSpd = 0;
            foreach (RealBuffData buff in CurrentBuff)
            {
                addSpd += buff.mAddSpd;
            }
            return mCharacterData.Spd + addSpd;
        }
    }

    /// <summary>
    /// X坐标
    /// </summary>
    public int X
    {
        get
        {
            return mX;
        }
    }

    /// <summary>
    /// Y坐标
    /// </summary>
    public int Y
    {
        get
        {
            return mY;
        }
    }

    /// <summary>
    /// 是否已死亡
    /// </summary>
    public bool Death
    {
        get
        {
            return mDeath;
        }
    }

    /// <summary>
    /// AI类型
    /// </summary>
    public AIType EnemyAI
    {
        get
        {
            return mEnemyAI;
        }
        set
        {
            mEnemyAI = value;
        }
    }
    
    /// <summary>
    /// 仇恨对象，无则返回-1
    /// </summary>
    public System.Int64 LastAttackerID
    {
        get
        {
            return mLastAttackerID;
        }
        set
        {
            mLastAttackerID = value;
        }
    }

    /// <summary>
    /// 仇恨对象的仇恨值
    /// </summary>
    public int AttackerHatred
    {
        get
        {
            return mAttackerHatred;
        }
        set
        {
            mAttackerHatred = value;
        }
    }

    /// <summary>
    /// buff总数
    /// </summary>
    public int CurrentBuffCount
    {
        get
        {
            return mBuffList.Keys.Count;
        }
    }

    /// <summary>
    /// Debuff总数
    /// </summary>
    public int CurrentBadBuffCount
    {
        get
        {
            int count = 0;
            foreach (int buffID in mBuffList.Keys)
            {
                RealBuffData buff = mBuffList[buffID];
                BuffData data = BuffManager.Instance.GetBuff(buff.mBuffID);
                if (data.BuffType == BuffType.Bad)
                {
                    count += 1;
                }
            }
            return count;
        }
    }

    /// <summary>
    /// 清除所有不良状态
    /// </summary>
    public void ClearAllDebuff()
    {
        List<int> temp = new List<int>();
        foreach (int buffID in mBuffList.Keys)
        {
            RealBuffData buff = mBuffList[buffID];
            BuffData data = BuffManager.Instance.GetBuff(buff.mBuffID);
            if (data.BuffType == BuffType.Bad)
            {
                temp.Add(buffID);
                //mBuffList.Remove(buffID);
            }
        }

        foreach (int id in temp)
        {
            mBuffList.Remove(id);
        }
        temp.Clear();

        RefreshBuffIcon();
    }

    /// <summary>
    /// 经过一回合，去掉时间已到的buff
    /// </summary>
    public void BuffPastOntRound()
    {
        List<int> temp = new List<int>();
        foreach (int buffID in mBuffList.Keys)
        {
            RealBuffData buff = mBuffList[buffID];
            buff.mLeftRound -= 1;

            if (buff.mLeftRound <= 0)
            {
                temp.Add(buffID);
                //mBuffList.Remove(buffID);
            }
        }

        foreach (int id in temp)
        {
            mBuffList.Remove(id);
        }
        temp.Clear();

        RefreshBuffIcon();
    }

    /// <summary>
    /// 增加某种buff
    /// </summary>
    /// <param name="_buffID"></param>
    /// <param name="_attacker"></param>
    public void AddNewBuff(int _buffID, CardData _attacker)
    {
        RealBuffData buff = new RealBuffData(_buffID, _attacker);
        mBuffList[_buffID] = buff;

        RefreshBuffIcon();
    }

    /// <summary>
    /// 去掉某种buff
    /// </summary>
    /// <param name="_buffID"></param>
    public void RemoveBuff(int _buffID)
    {
        foreach (int buffID in mBuffList.Keys)
        {
            RealBuffData buff = mBuffList[buffID];
            if (buff.mBuffID == _buffID)
            {
                mBuffList.Remove(buffID);
                break;
            }
        }

        RefreshBuffIcon();
    }

    /// <summary>
    /// 尝试检测是否存在特定buff
    /// </summary>
    /// <param name="_buffID"></param>
    public RealBuffData GetBuff(int _buffID)
    {
        foreach (int buffID in mBuffList.Keys)
        {
            RealBuffData buff = mBuffList[buffID];
            if (buff.mBuffID == _buffID)
            {
                return buff;
            }
        }

        return null;
    }

    void RefreshBuffIcon()
    {
        if (mBuffList.Keys.Count <= 0)
        {
            UI.Buff = "";
            return;
        }

        int show = Random.Range(0, mBuffList.Keys.Count);
        foreach (int buffID in mBuffList.Keys)
        {
            show -= 1;
            if (show <= 0)
            {
                UI.Buff = BuffManager.Instance.GetBuff(mBuffList[buffID].mBuffID).SpriteName;
                break;
            }
        }
    }

    /// <summary>
    /// 当前所有buff列表
    /// </summary>
    public Dictionary<int, RealBuffData>.ValueCollection CurrentBuff
    {
        get
        {
            return mBuffList.Values;
        }
    }

    CardLogic mLogic = null;
    public CardLogic Logic
    {
        get
        {
            if (mLogic == null)
            {
                mLogic = gameObject.GetComponent<CardLogic>();
            }
            return mLogic;
        }
    }

    CardUI mUI = null;
    public CardUI UI
    {
        get
        {
            if (mUI == null)
            {
                mUI = gameObject.GetComponent<CardUI>();
            }
            return mUI;
        }
    }

    CharacterData mCharacterData = null;

    public CardBaseData BaseData
    {
        get
        {
            if (mCharacterData == null)
            {
                return null;
            }

            return CardManager.Instance.GetCard(mCharacterData.CardID);
        }
    }

    public int Level
    {
        get
        {
            if (mCharacterData == null)
            {
                return 1;
            }

            return mCharacterData.Level;
        }
    }

    /// <summary>
    /// 自动释放技能
    /// </summary>
    public bool AutoSkill
    {
        get
        {
            return mAutoSkill;
        }
        set
        {
            mAutoSkill = value;
        }
    }

    public bool IsLeader
    {
        get
        {
            return mIsLeader;
        }
        set
        {
            mIsLeader = value;
            UI.IsLeader = mIsLeader;            
        }
    }

    protected int mBuyPrice = 0;
    protected int mDropCard = 0;
    protected int mDropCoin = 0;

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

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void ResetAllData(PhaseType _phase, CharacterData _data)
    {
        mCharacterData = _data;
        Phase = _phase;
        HP = _data.MaxHP;
        
        CardBaseData carddata = CardManager.Instance.GetCard(_data.CardID);
        UI.SpriteName = carddata.CardSprite;
        UI.Element = carddata.Element;

        switch (Phase)
        {
            case PhaseType.Charactor:
                gameObject.tag = "Charactor";
                MP = 0;
                break;
            case PhaseType.Enemy:
                gameObject.tag = "Enemy";
                MP = MPMax;
                break;
        }

        UI.ForceReset();
    }

    public void SetPosition(int _x, int _y)
    {
        mX = _x;
        mY = _y;
    }
}

/// <summary>
/// Buff的实体数据
/// </summary>
public class RealBuffData
{
    public int mBuffID = 0;
    public int mLeftRound = 1;

    public int mAddHP = 0;
    public int mAddMP = 0;

    public int mAddAtk = 0;
    public int mAddDef = 0;
    public int mAddSpd = 0;

    public bool mDisableSkill = false;
    public bool mCanMiss = false;
    public bool mSleep = false;
    public bool mDizziness = false;
    public bool mGreatDamage = false;

    public RealBuffData(int _id, CardData _attacker)
    {
        BuffData data = BuffManager.Instance.GetBuff(_id);

        mBuffID = data.ID;
        mLeftRound = Random.Range(0, data.RoundMax - data.RoundMin + 1) + data.RoundMin;

        mAddHP = 0;
        mAddMP = 0;
        mAddAtk = 0;
        mAddDef = 0;
        mAddSpd = 0;
        mDisableSkill = false;
        mCanMiss = false;
        mSleep = false;
        mDizziness = false;
        mGreatDamage = false;

        switch ((BuffEnum)mBuffID)
        {
            case BuffEnum.BrokeDef:
                mAddDef = -Mathf.FloorToInt(_attacker.Def * 0.5f);
                break;
            case BuffEnum.DisableSkill:
                mDisableSkill = true;
                break;
            case BuffEnum.CanMiss:
                mCanMiss = true;
                break;
            case BuffEnum.Sleep:
                mSleep = true;
                break;
            case BuffEnum.Dizziness:
                mDizziness = true;
                break;
            case BuffEnum.GreatDamage:
                mGreatDamage = true;
                break;
            case BuffEnum.Poison:
                mAddHP = -_attacker.Atk;
                break;
            case BuffEnum.AddAtk:
                mAddAtk = Mathf.FloorToInt(_attacker.Atk * 0.5f);
                break;
            case BuffEnum.AddDef:
                mAddDef = Mathf.FloorToInt(_attacker.Def * 0.5f);
                break;
            case BuffEnum.AddSpd:
                mAddSpd = Mathf.FloorToInt(_attacker.Spd * 0.5f);
                break;
            case BuffEnum.AddHPPerround:
                mAddHP = _attacker.Atk;
                break;
            case BuffEnum.AddMPPerround:
                mAddMP = Mathf.FloorToInt(_attacker.MPMax * 0.15f);
                break;
            case BuffEnum.AddAllPerround:
                mAddHP = Mathf.FloorToInt(_attacker.HPMax * 0.2f);
                mAddMP = Mathf.FloorToInt(_attacker.MPMax * 0.2f);
                break;
        }
    }
}