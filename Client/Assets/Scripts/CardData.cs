using UnityEngine;
using System.Collections;

public enum PhaseType : int
{
    Charactor = 0,
    Enemy,
}

public enum ElementType : int
{
    Fire = 0,
    Water,
    Wind,
    Ground,
    Light,
    Dark,
}

public enum AIType : int 
{
    NPC,        // 随机闲逛，不攻击范围内的敌人，不追击仇恨对象
    Retarded,   // 随机闲逛，概率攻击范围内的敌人，不追击仇恨对象
    Slime,      // 随机闲逛，攻击范围内的敌人，不追击仇恨对象
    Goblin,     // 随机闲逛，攻击范围内的敌人，追击仇恨对象
    Pillar,     // 原地不动，不攻击范围内的敌人，不追击仇恨对象
    Cannon,     // 原地不动，攻击范围内的敌人，不追击仇恨对象
    Guard,      // 原地不动，攻击范围内的敌人，追击仇恨对象
    Assailant,  // 主动接近，攻击范围内的敌人，不追击仇恨对象
    Leader,     // 主动接近，攻击范围内的敌人，追击仇恨对象
}

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
                Logic.RefreshToDeath();
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
    /// 攻击力
    /// </summary>
    public int Atk
    {
        get
        {
            return mCharacterData.Atk;
        }
    }

    /// <summary>
    /// 防御力
    /// </summary>
    public int Def
    {
        get
        {
            return mCharacterData.Def;
        }
    }

    /// <summary>
    /// 速度
    /// </summary>
    public int Spd
    {
        get
        {
            return mCharacterData.Spd;
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
        gameObject.transform.FindChild("CardSprite").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = carddata.CardSprite;

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
    }

    public void SetPosition(int _x, int _y)
    {
        mX = _x;
        mY = _y;
    }
}
