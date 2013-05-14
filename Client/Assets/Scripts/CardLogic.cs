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
    public GameObject SelectSprite;
    public UISlider BloodBar;
    public Animation CardAnimation;
    public UISprite BloodBarBackground;
    
    CharAnimationState mAnimationState = CharAnimationState.NotInit;
    ActionState mActionState = ActionState.None;

	// Use this for initialization
	void Start () {
        mSelect = false;
        mNeedRefresh = true;
        PlayAnimation(CharAnimationState.Idle);
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
        CardAnimation.gameObject.GetComponent<CardAnimation>().AnimationEvent += AnimationEventCall;
    }

    void OnDisable()
    {
        CardAnimation.gameObject.GetComponent<CardAnimation>().AnimationEvent -= AnimationEventCall;
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
        if (!mSelect && !GameLogic.Instance.IsMultiple)
        {
            GameLogic.Instance.UnselectAll();
        }

        Select(!mSelect);
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
        Refresh();

        if (NeedEndCalculate && !mAnimationNotFinish && !mWaitingDamage)
        {
            mNeedEndCalculate = false;
            EndCalculate();
        }
    }

    void Refresh()
    {
        if (!mNeedRefresh)
        {
            return;
        }
        mNeedRefresh = false;

        SelectSprite.active = mSelect;
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
                        CreateArrowEffect(mTargetObj.gameObject.transform.localPosition, 3.35f, mTargetObj.gameObject);
                        Invoke("DoDamage", 3.35f);
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

    public void CreateStartAttackEffect()
    {
        GameObject perfab = Resources.Load("Cards/Perfabs/AttackStart", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 0, -1);
        obj.transform.localScale = Vector3.one;
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
    }

    public void CreateArrowEffect(Vector3 _target, float _flyTime,GameObject _targetObj)
    {
        GameObject perfab = Resources.Load("Cards/Perfabs/Arrow", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 0, -1);
        obj.transform.localScale = new Vector3(9, 50, 1);

        Vector3 position = transform.localPosition;
        Vector3 tar = _targetObj.transform.localPosition;
        Vector3 dir = tar - position;
        dir.Normalize();

        obj.transform.up = dir;

        obj.GetComponent<UISprite>().depth = 99;
        TweenPosition.Begin(obj, _flyTime, _target + new Vector3(0, 0, -1));
        Destroy(obj, _flyTime);
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
            NeedEndCalculate = true;
            return;
        }

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
        //print("End calculatorAI: " + gameObject.name);

        mActionState = ActionState.None;
        mTargetObj = null;
        mCalculatorAI = false;
        if (ActionFinishEvent != null)
        {
            ActionFinishEvent(this);
        }
    }

    int CalculateDamage(CardData _from, CardData _target)
    {
        int damage = (int)((_from.Atk - _target.Def * 0.5f) * 2);
        if (damage < 1)
        {
            damage = 1;
        }
        return damage;
    }

    public void RefreshBloodBar(float _progress)
    {
        BloodBar.sliderValue = _progress;
    }

    public void RefreshToDeath()
    {
        PlayAnimation(CharAnimationState.Death);
    }

    public void RefreshPhase(PhaseType _phase)
    {
        BloodBarBackground.spriteName = _phase == PhaseType.Charactor ? "Bloodbar01" : "Bloodbar02";
    }
}
