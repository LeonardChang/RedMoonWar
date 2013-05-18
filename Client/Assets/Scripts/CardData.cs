using UnityEngine;
using System.Collections;

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
    /// �����������е�ID
    /// </summary>
    public System.Int64 ID
    {
        get
        {
            return mCharacterData.ID;
        }
    }

    /// <summary>
    /// ���HP
    /// </summary>
    public int HPMax
    {
        get
        {
            return mCharacterData.MaxHP;
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
                Logic.CreateDeathEffect();
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
            return mCharacterData.MaxMP;
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
    /// ��ͨ����
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
    /// ����
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
    /// ���ܵȼ�
    /// </summary>
    public int SkillLevel
    {
        get
        {
            return mCharacterData.SkillLevel;
        }
    }

    /// <summary>
    /// ������
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
            CardBaseData carddata = CardManager.Instance.GetCard(mCharacterData.CardID);
            return carddata.Element;
        }
    }

    /// <summary>
    /// �����Ƶ�����
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
    /// ���Ƶ�����
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
    /// ������
    /// </summary>
    public int Atk
    {
        get
        {
            return mCharacterData.Atk;
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public int Def
    {
        get
        {
            return mCharacterData.Def;
        }
    }

    /// <summary>
    /// �ٶ�
    /// </summary>
    public int Spd
    {
        get
        {
            return mCharacterData.Spd;
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
    }

    public void SetPosition(int _x, int _y)
    {
        mX = _x;
        mY = _y;
    }
}
