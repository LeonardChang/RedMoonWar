using UnityEngine;
using System.Collections;

public enum CharAnimationState
{
    NotInit,
    Idle,
    Select,
    Attack,
    Skill,
    Death,
    Join,
}

public enum ActionState
{
    None,
    Attack,
    Health,
    ArrowAttack,
}

public class CardLogic : MonoBehaviour {
    public Animation CardAnimation;
    
    CharAnimationState mAnimationState = CharAnimationState.NotInit;
    ActionState mActionState = ActionState.None;

	// Use this for initialization
	void Start () {
        mSelect = false;
        mNeedRefresh = true;
        PlayAnimation(CharAnimationState.Idle);

        CardAnimation.gameObject.GetComponent<CardAnimation>().AnimationEvent += AnimationEventCall;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (!CardAnimation.isPlaying)
	    {
            if (mSelect)
            {
                PlayAnimation(CharAnimationState.Select);
            }
            else
            {
                PlayAnimation(CharAnimationState.Idle);
            }
	    }
	}

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    bool mSelect = false;
    bool mNeedRefresh = false;

    public bool IsSelect
    {
        get
        {
            return mSelect;
        }
    }

    public void OnClick()
    {
        if (!GameLogic.Instance.IsMultiple)
        {
            if (GameLogic.Instance.IsAllSelect())
            {
                GameLogic.Instance.UnselectAll();
                Select(true);
            }
            else
            {
                bool sel = mSelect;
                GameLogic.Instance.UnselectAll();
                Select(!sel);
            }
        }
        else
        {
            Select(!mSelect);
        }
    }

    public void Select(bool _select)
    {
        if (gameObject.tag == "Enemy")
        {
            return;
        }

        mSelect = _select;
        mNeedRefresh = true;

        if (mSelect)
        {
            PlayAnimation(CharAnimationState.Select);
        }
        else
        {
            PlayAnimation(CharAnimationState.Idle);
        }
    }

    void LateUpdate()
    {
        if (mNeedRefresh)
        {
            mNeedRefresh = false;
            Refresh();
        }

        if (NeedEndCalculate && !mAnimationNotFinish && !mWaitingDamage)
        {
            mNeedEndCalculate = false;
            EndCalculate();
        }
    }

    void Refresh()
    {
        UI.Select = mSelect;
    }

    public void PlayAnimation(CharAnimationState _state)
    {
        if (mAnimationState == _state)
        {
            return;
        }

        switch (_state)
        {
            case CharAnimationState.Idle:
                CardAnimation.Play("Idle");
                break;
            case CharAnimationState.Select:
                CardAnimation.Play("Idle2");
                break;
            case CharAnimationState.Attack:
                CardAnimation.Play("Attack");
                mAnimationNotFinish = true;
                break;
            case CharAnimationState.Skill:
                CardAnimation.Play("Skill");
                mAnimationNotFinish = true;
                break;
            case CharAnimationState.Death:
                CardAnimation.Play("Death");
                mAnimationNotFinish = true;
                break;
            case CharAnimationState.Join:
                CardAnimation.Play("Join");
                mAnimationNotFinish = true;
                break;
        }
    }

    bool mAnimationNotFinish = false;
    void AnimationEventCall(string _event)
    {
        if (_event == "StartAttack")
        {
            CreateStartAttackEffect();
        }
        else if (_event == "Attacking")
        {
            if (mActionState == ActionState.Attack && mTargetObj != null)
            {
                DoDamage();
                NeedEndCalculate = true;
            }
        }
        else if (_event == "StartSkill")
        {
            switch (mActionState)
            {
                case ActionState.Health:
                    if (mTargetObj != null)
                    {
                        int heal = 20;
                        mTargetObj.Data.HP += heal;
                        mTargetObj.CreateHealEffect(heal);
                        NeedEndCalculate = true;
                    }
                    break;
                case ActionState.ArrowAttack:
                    if (mTargetObj != null)
                    {
                        mWaitingDamage = true;
                        CreateArrowEffect(mTargetObj.gameObject.transform, 0.35f);
                        Invoke("DoDamage", 0.35f);
                        NeedEndCalculate = true;
                    }
                    break;
            }
        }
        else if (_event == "Joining")
        {
        }
        else if (_event == "Finish")
        {
            mAnimationNotFinish = false;
            NeedEndCalculate = true;

            if (Data.Death)
            {
                gameObject.SetActiveRecursively(false);
            }
        }
    }

    bool mWaitingDamage = false;
    void DoDamage()
    {
        if (mTargetObj == null)
        {
            return;
        }

        // 计算伤害
        int damage = CalculateDamage(Data, mTargetObj.GetComponent<CardData>());
        mTargetObj.Data.HP -= damage;

        // 记录仇恨
        if (mTargetObj.Data.LastAttackerID == Data.ID)
        {
            // 上次也是哥揍的
            if (mTargetObj.Data.AttackerHatred < Data.Hatred)
            {
                mTargetObj.Data.AttackerHatred = Data.Hatred;
            }
            else
            {
                mTargetObj.Data.AttackerHatred += (int)(Data.Hatred * 0.2f);
            }
            mTargetObj.Data.LastAttackerID = Data.ID;
        }
        else if (mTargetObj.Data.LastAttackerID != -1)
        {
            // 上次是被其他人揍的
            mTargetObj.Data.AttackerHatred -= Data.Hatred;
            if (mTargetObj.Data.AttackerHatred <= 0)
            {
                mTargetObj.Data.AttackerHatred = Data.Hatred;
                mTargetObj.Data.LastAttackerID = Data.ID;
            }
        }
        else
        {
            // 还没被人揍过
            mTargetObj.Data.AttackerHatred = Data.Hatred;
            mTargetObj.Data.LastAttackerID = Data.ID;
        }

        AudioSource.PlayClipAtPoint(Resources.Load("Sounds/Slash10", typeof(AudioClip)) as AudioClip, Vector3.zero);

        mTargetObj.CreateHitEffect(damage);
        mWaitingDamage = false;
    }

    CardData mData = null;
    public CardData Data
    {
        get
        {
            if (mData == null)
            {
                mData = gameObject.GetComponent<CardData>();
            }
            return mData;
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

    public void CreateStartAttackEffect()
    {
        GameObject perfab = Resources.Load("Cards/Perfabs/AttackStart", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 0, -1);
        obj.transform.localScale = Vector3.one;

        AudioSource.PlayClipAtPoint(Resources.Load("Sounds/Blow1", typeof(AudioClip)) as AudioClip, Vector3.zero);
    }

    public void CreateHitEffect(int _Damage)
    {
        GameObject perfab = Resources.Load("Cards/Perfabs/HitEffect", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 0, -1);
        obj.transform.localScale = Vector3.one;

        perfab = Resources.Load("Cards/Perfabs/BloodLabel", typeof(GameObject)) as GameObject;
        obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 70, -2);
        obj.transform.localScale = new Vector3(50, 50, 1);
        UILabel label = obj.GetComponent<UILabel>();
        label.text = "-" + _Damage;
        label.color = new Color(1, 0, 0);

        iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0), 0.25f);
    }

    public void CreateHealEffect(int _Health)
    {
        GameObject perfab = Resources.Load("Cards/Perfabs/HealEffect", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 0, -1);
        obj.transform.localScale = Vector3.one * 2;

        perfab = Resources.Load("Cards/Perfabs/BloodLabel", typeof(GameObject)) as GameObject;
        obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 70, -2);
        obj.transform.localScale = new Vector3(50, 50, 1);
        UILabel label = obj.GetComponent<UILabel>();
        label.text = "+" + _Health;
        label.color = new Color(0.2f, 1, 0.2f);

        AudioSource.PlayClipAtPoint(Resources.Load("Sounds/Heal3", typeof(AudioClip)) as AudioClip, Vector3.zero);
    }

    public void CreateArrowEffect(Transform _target, float _flyTime)
    {
        GameObject perfab = Resources.Load("Cards/Perfabs/Arrow", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 0, -1);
        obj.transform.localScale = new Vector3(9, 50, 1);

        Vector3 from = transform.localPosition + new Vector3(0, 50, 0);
        Vector3 to = _target.localPosition + new Vector3(0, 50, 0);
        Vector3 dir = to - from;
        dir.Normalize();
        obj.transform.up = dir;

        TweenPosition.Begin(obj, _flyTime, to + new Vector3(0, 0, -1));
        Destroy(obj, _flyTime);

        AudioSource.PlayClipAtPoint(Resources.Load("Sounds/Bow1", typeof(AudioClip)) as AudioClip, Vector3.zero);
    }

    public System.Action<CardLogic> ActionFinishEvent;

    CardLogic mTargetObj = null;
    bool mCalculatorAI = false;
    bool mNeedEndCalculate = false;
    bool NeedEndCalculate
    {
        get { return mNeedEndCalculate; }
        set 
        {
            if (mCalculatorAI)
            {
                mNeedEndCalculate = value;
            }
            else
            {
                mNeedEndCalculate = false;
            }
        }
    }

    public void CalculateAI()
    {
        if (Data.Death)
        {
            EndCalculate();
            return;
        }

        //print("Start calculatorAI: " + gameObject.GetInstanceID().ToString());

        mCalculatorAI = true;
        CardData data = Data;
        if (data.Phase == PhaseType.Charactor)
        {
            switch (data.CardAction)
            {
                case ActionType.NormalAttack:
                case ActionType.MagicAttack:
                    {
                        CardData enemy = GameLogic.Instance.GetActionTarget(Data, PhaseType.Enemy);
                        if (enemy != null)
                        {
                            mActionState = ActionState.Attack;
                            mTargetObj = enemy.Logic;
                            PlayAnimation(CharAnimationState.Attack);
                        }
                        else
                        {
                            NeedEndCalculate = true;
                        }
                    }
                    break;
                case ActionType.ArrowAttack:
                    {
                        CardData enemy = GameLogic.Instance.GetActionTarget(Data, PhaseType.Enemy);
                        if (enemy != null)
                        {
                            mActionState = ActionState.ArrowAttack;
                            mTargetObj = enemy.Logic;
                            PlayAnimation(CharAnimationState.Skill);
                        }
                        else
                        {
                            NeedEndCalculate = true;
                        }
                    }
                    break;
                case ActionType.Health:
                    {
                        CardData target = null;
                        foreach (CardData getChar in GameLogic.Instance.GetActionTargets(Data, PhaseType.Charactor, true))
                        {
                            if (getChar.HP >= getChar.HPMax)
                            {
                                continue;
                            }

                            if (target == null || target.HP > getChar.HP)
                            {
                                target = getChar;
                            }
                        }

                        if (target != null)
                        {
                            mActionState = ActionState.Health;
                            mTargetObj = target.Logic;
                            PlayAnimation(CharAnimationState.Skill);
                        }
                        else
                        {
                            NeedEndCalculate = true;
                        }
                    }
                    break;
                default:
                    NeedEndCalculate = true;
                    break;
            }
        }
        else
        {
            if (Data.EnemyAI == AIType.Slime)
            {
                CardData enemy = GameLogic.Instance.GetActionTarget(Data, PhaseType.Charactor);
                if (enemy != null)
                {
                    mActionState = ActionState.Attack;
                    mTargetObj = enemy.Logic;
                    PlayAnimation(CharAnimationState.Attack);
                }
                else
                {
                    NeedEndCalculate = true;
                }
            }
            else
            {
                NeedEndCalculate = true;
            }
        }
    }

    void EndCalculate()
    {
        //print("End calculatorAI: " + gameObject.GetInstanceID().ToString());

        mActionState = ActionState.None;
        mTargetObj = null;
        mCalculatorAI = false;
        if (ActionFinishEvent != null)
        {
            ActionFinishEvent(this);
        }
    }

    /// <summary>
    /// 伤害计算公式
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_target"></param>
    /// <returns></returns>
    public static int CalculateDamage(CardData _from, CardData _target)
    {
        
        int damage = (int)((_from.Atk - _target.Def * 0.5f) * 2);

        if (damage < 1)
        {
            damage = 1;
        }

        if ((_from.Element == ElementType.Fire && _target.Element == ElementType.Wind)
            || (_from.Element == ElementType.Wind && _target.Element == ElementType.Ground)
            || (_from.Element == ElementType.Ground && _target.Element == ElementType.Water)
            || (_from.Element == ElementType.Water && _target.Element == ElementType.Fire)
            || (_from.Element == ElementType.Light && _target.Element == ElementType.Dark)
            || (_from.Element == ElementType.Dark && _target.Element == ElementType.Light))
        {
            damage *= 2;
        }

        return damage;
    }

    public void RefreshToDeath()
    {
        PlayAnimation(CharAnimationState.Death);
    }
}
