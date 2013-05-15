using UnityEngine;
using System.Collections;

public enum ActionType : int
{
    NormalAttack = 0,
    MagicAttack,
    ArrowAttack,
    Health,
}

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

public enum ClassType : int
{
    WaterSaber = 0,
    FireMagic,
    GreenArrow,
    SLMGirl,
    LightPastor,
    DarkGhost,

    Max,
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
    private int mID = -1;

    private int mHPMax = 100;
    private int mHP = 100;

    private int mMPMax = 100;
    private int mMP = 0;

    private ActionType mCardAction = ActionType.NormalAttack;
    private int mActionRange = 1;
    private PhaseType mPhase = PhaseType.Charactor;
    private ElementType mElement = ElementType.Fire;
    private ClassType mClass = ClassType.WaterSaber;

    private int mAtk = 100;
    private int mDef = 100;
    private int mSpd = 100;
    private int mHatred = 100;

    private int mX = 0;
    private int mY = 0;

    private bool mDeath = false;

    private AIType mEnemyAI = AIType.Slime;

    private int mLastAttackerID = -1;
    private int mAttackerHatred = 0;

    /// <summary>
    /// 棋子在棋盘中的ID
    /// </summary>
    public int ID
    {
        get
        {
            return mID;
        }
    }

    /// <summary>
    /// 最大HP
    /// </summary>
    public int HPMax
    {
        get
        {
            return mHPMax;
        }
        set
        {
            mHPMax = value;
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
            return mMPMax;
        }
        set
        {
            mMPMax = value;
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
    /// 行动类型
    /// </summary>
    public ActionType CardAction
    {
        get
        {
            return mCardAction;
        }
        set
        {
            mCardAction = value;
        }
    }

    /// <summary>
    /// 行动范围
    /// </summary>
    public int ActionRange
    {
        get
        {
            return mActionRange;
        }
        set
        {
            mActionRange = value;
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
            return mElement;
        }
        set
        {
            mElement = value;
        }
    }

    /// <summary>
    /// 卡片类型
    /// </summary>
    public ClassType Class
    {
        get
        {
            return mClass;
        }
        set
        {
            mClass = value;
        }
    }

    /// <summary>
    /// 攻击力
    /// </summary>
    public int Atk
    {
        get
        {
            return mAtk;
        }
        set
        {
            mAtk = value;
        }
    }

    /// <summary>
    /// 防御力
    /// </summary>
    public int Def
    {
        get
        {
            return mDef;
        }
        set
        {
            mDef = value;
        }
    }

    /// <summary>
    /// 速度
    /// </summary>
    public int Spd
    {
        get
        {
            return mSpd;
        }
        set
        {
            mSpd = value;
        }
    }

    /// <summary>
    /// 攻击造成的仇恨
    /// </summary>
    public int Hatred
    {
        get
        {
            return mHatred;
        }
        set
        {
            mHatred = value;
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
    public int LastAttackerID
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

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void ResetAllData(int _id, ClassType _class, PhaseType _phase)
    {
        mID = _id;

        Class = _class;
        Phase = _phase;

        switch (_class)
        {
            case ClassType.WaterSaber:
                HPMax = 84;
                CardAction = ActionType.NormalAttack;
                ActionRange = 1;
                Element = ElementType.Water;
                Atk = 23;
                Def = 21;
                Spd = 58;
                mHatred = 50;

                gameObject.transform.FindChild("CardSprite").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = "Card001";
                break;
            case ClassType.FireMagic:
                CardAction = ActionType.MagicAttack;
                HPMax = 79;
                ActionRange = 2;
                Element = ElementType.Fire;
                Atk = 15;
                Def = 17;
                Spd = 56;
                mHatred = 50;

                gameObject.transform.FindChild("CardSprite").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = "Card002";
                break;
            case ClassType.GreenArrow:
                HPMax = 86;
                CardAction = ActionType.ArrowAttack;
                ActionRange = 2;
                Element = ElementType.Wind;
                Atk = 18;
                Def = 18;
                Spd = 59;
                mHatred = 50;

                gameObject.transform.FindChild("CardSprite").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = "Card003";
                break;
            case ClassType.SLMGirl:
                HPMax = 88;
                CardAction = ActionType.NormalAttack;
                ActionRange = 1;
                Element = ElementType.Ground;
                Atk = 20;
                Def = 23;
                Spd = 56;
                mHatred = 100;

                gameObject.transform.FindChild("CardSprite").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = "Card004";
                break;
            case ClassType.LightPastor:
                HPMax = 84;
                CardAction = ActionType.Health;
                ActionRange = 1;
                Element = ElementType.Light;
                Atk = 16;
                Def = 18;
                Spd = 58;
                mHatred = 50;

                gameObject.transform.FindChild("CardSprite").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = "Card005";
                break;
            case ClassType.DarkGhost:
                HPMax = 83;
                CardAction = ActionType.NormalAttack;
                ActionRange = 1;
                Element = ElementType.Dark;
                Atk = 19;
                Def = 18;
                Spd = 60;
                mHatred = 50;

                gameObject.transform.FindChild("CardSprite").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = "Card006";
                break;
        }

        HP = HPMax;

        switch (Phase)
        {
            case PhaseType.Charactor:
                gameObject.tag = "Charactor";
                break;
            case PhaseType.Enemy:
                gameObject.tag = "Enemy";
                break;
        }
    }

    public void SetPosition(int _x, int _y)
    {
        mX = _x;
        mY = _y;
    }
}
