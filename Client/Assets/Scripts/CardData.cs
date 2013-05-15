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
    NPC,        // ����й䣬��������Χ�ڵĵ��ˣ���׷����޶���
    Retarded,   // ����й䣬���ʹ�����Χ�ڵĵ��ˣ���׷����޶���
    Slime,      // ����й䣬������Χ�ڵĵ��ˣ���׷����޶���
    Goblin,     // ����й䣬������Χ�ڵĵ��ˣ�׷����޶���
    Pillar,     // ԭ�ز�������������Χ�ڵĵ��ˣ���׷����޶���
    Cannon,     // ԭ�ز�����������Χ�ڵĵ��ˣ���׷����޶���
    Guard,      // ԭ�ز�����������Χ�ڵĵ��ˣ�׷����޶���
    Assailant,  // �����ӽ���������Χ�ڵĵ��ˣ���׷����޶���
    Leader,     // �����ӽ���������Χ�ڵĵ��ˣ�׷����޶���
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
    /// �����������е�ID
    /// </summary>
    public int ID
    {
        get
        {
            return mID;
        }
    }

    /// <summary>
    /// ���HP
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
    /// ��ǰHP
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
    /// ���MP
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
    /// ��ǰMP
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
    /// �ж�����
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
    /// �ж���Χ
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
    /// ����
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
    /// ����
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
    /// ��Ƭ����
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
    /// ������
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
    /// ������
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
    /// �ٶ�
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
    /// ������ɵĳ��
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
    /// X����
    /// </summary>
    public int X
    {
        get
        {
            return mX;
        }
    }

    /// <summary>
    /// Y����
    /// </summary>
    public int Y
    {
        get
        {
            return mY;
        }
    }

    /// <summary>
    /// �Ƿ�������
    /// </summary>
    public bool Death
    {
        get
        {
            return mDeath;
        }
    }

    /// <summary>
    /// AI����
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
    /// ��޶������򷵻�-1
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
    /// ��޶���ĳ��ֵ
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
