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

public class CardData : MonoBehaviour {
    private int mHPMax = 100;
    private int mHP = 100;

    private ActionType mCardAction = ActionType.NormalAttack;
    private int mActionRange = 1;
    private PhaseType mPhase = PhaseType.Charactor;
    private ElementType mElement = ElementType.Fire;
    private ClassType mClass = ClassType.WaterSaber;

    private int mAtk = 100;
    private int mDef = 100;
    private int mSpd = 100;

    private int mX = 0;
    private int mY = 0;

    private bool mDeath = false;

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

            gameObject.GetComponent<CardLogic>().RefreshBloodBar((float)mHP / HPMax);

            if (mHP == 0)
            {
                mDeath = true;
                gameObject.GetComponent<CardLogic>().RefreshToDeath();
            }
        }
    }

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

    public PhaseType Phase
    {
        get
        {
            return mPhase;
        }
        set
        {
            mPhase = value;
            gameObject.GetComponent<CardLogic>().RefreshPhase(mPhase);
        }
    }

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

    public int X
    {
        get
        {
            return mX;
        }
    }

    public int Y
    {
        get
        {
            return mY;
        }
    }

    public bool Death
    {
        get
        {
            return mDeath;
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
    
    public void ResetAllData(ClassType _class, PhaseType _phase)
    {
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

                gameObject.transform.FindChild("Card").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = "Card001";
                break;
            case ClassType.FireMagic:
                CardAction = ActionType.MagicAttack;
                HPMax = 79;
                ActionRange = 2;
                Element = ElementType.Fire;
                Atk = 15;
                Def = 17;
                Spd = 56;

                gameObject.transform.FindChild("Card").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = "Card002";
                break;
            case ClassType.GreenArrow:
                HPMax = 86;
                CardAction = ActionType.ArrowAttack;
                ActionRange = 2;
                Element = ElementType.Wind;
                Atk = 18;
                Def = 18;
                Spd = 59;

                gameObject.transform.FindChild("Card").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = "Card003";
                break;
            case ClassType.SLMGirl:
                HPMax = 88;
                CardAction = ActionType.NormalAttack;
                ActionRange = 1;
                Element = ElementType.Ground;
                Atk = 20;
                Def = 23;
                Spd = 56;

                gameObject.transform.FindChild("Card").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = "Card004";
                break;
            case ClassType.LightPastor:
                HPMax = 84;
                CardAction = ActionType.Health;
                ActionRange = 1;
                Element = ElementType.Light;
                Atk = 16;
                Def = 18;
                Spd = 58;

                gameObject.transform.FindChild("Card").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = "Card005";
                break;
            case ClassType.DarkGhost:
                HPMax = 83;
                CardAction = ActionType.NormalAttack;
                ActionRange = 1;
                Element = ElementType.Dark;
                Atk = 19;
                Def = 18;
                Spd = 60;

                gameObject.transform.FindChild("Card").FindChild("Sprite").gameObject.GetComponent<UISprite>().spriteName = "Card006";
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
