using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

public class CardLogic : MonoBehaviour {
    public Animation CardAnimation;
    
    CharAnimationState mAnimationState = CharAnimationState.NotInit;
    AttackAnimType mActionState = AttackAnimType.None;

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
            if (mActionState == AttackAnimType.NormalAttack && mTargetObj.Count > 0)
            {
                DoDamage();
                NeedEndCalculate = true;
            }
        }
        else if (_event == "StartSkill")
        {
            switch (mActionState)
            {
                case AttackAnimType.HPHealth:
                    if (mTargetObj.Count > 0)
                    {
                        int heal = 20;
                        foreach (CardLogic logic in mTargetObj)
                        {
                            logic.Data.HP += heal;
                            logic.CreateHealEffect(heal);
                        }
                        NeedEndCalculate = true;
                    }
                    break;
                case AttackAnimType.Arrow:
                    if (mTargetObj.Count > 0)
                    {
                        mWaitingDamage = true;
                        foreach (CardLogic logic in mTargetObj)
                        {
                            CreateArrowEffect(logic.gameObject.transform, 0.5f);
                        }
                        Invoke("DoDamage", 0.5f);
                        NeedEndCalculate = true;
                    }
                    break;
                case AttackAnimType.FireBall:
                    if (mTargetObj.Count > 0)
                    {
                        mWaitingDamage = true;
                        foreach (CardLogic logic in mTargetObj)
                        {
                            CreateFireBallEffect(logic.gameObject.transform, 0.85f);
                        }
                        Invoke("DoDamage", 0.85f);
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

    void CreateTAnimation(string _animation)
    {
        GameObject perfab = Resources.Load("Cards/Perfabs/TAnimation", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 0, -1);
        obj.transform.localScale = Vector3.one;

        obj.GetComponent<TAnimObject>().Name = _animation;
    }
    
    /// <summary>
    /// 创建攻击动画
    /// </summary>
    void CreateStartAttackEffect()
    {
        //GameObject perfab = Resources.Load("Cards/Perfabs/AttackStart", typeof(GameObject)) as GameObject;
        //GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        //obj.transform.parent = gameObject.transform.parent;
        //obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 0, -1);
        //obj.transform.localScale = Vector3.one;

        //AudioSource.PlayClipAtPoint(Resources.Load("Sounds/Blow1", typeof(AudioClip)) as AudioClip, Vector3.zero);

        CreateTAnimation("Attack");
    }

    /// <summary>
    /// 创建被击动画
    /// </summary>
    /// <param name="_Damage"></param>
    void CreateHitEffect(int _Damage)
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

    /// <summary>
    /// 创建被治疗动画
    /// </summary>
    /// <param name="_Health"></param>
    void CreateHealEffect(int _Health)
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

    /// <summary>
    /// 创建射箭动画
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="_flyTime"></param>
    void CreateArrowEffect(Transform _target, float _flyTime)
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

    /// <summary>
    /// 创建死亡动画
    /// </summary>
    public void CreateDeathEffect()
    {
        PlayAnimation(CharAnimationState.Death);
    }

    /// <summary>
    /// 创建火球动画
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="_flyTime"></param>
    void CreateFireBallEffect(Transform _target, float _flyTime)
    {
        GameObject perfab = Resources.Load("Cards/Perfabs/FireBall", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 0, -1);
        obj.transform.localScale = new Vector3(58, 70, 1);

        Vector3 to = _target.localPosition + new Vector3(0, 50, 0);

        TweenPositionEx.Begin(obj, _flyTime, gameObject.transform.localPosition + new Vector3(Random.Range(0, 2) == 0 ? -300 : 300, Random.Range(0, 2) == 0 ? -300 : 300, -1), to + new Vector3(0, 0, -1), 0.75f).method = UITweener.Method.EaseInOut;
        Destroy(obj, _flyTime);

        AudioSource.PlayClipAtPoint(Resources.Load("Sounds/Fire1", typeof(AudioClip)) as AudioClip, Vector3.zero);
    }

    public System.Action<CardLogic> ActionFinishEvent;

    List<CardLogic> mTargetObj = new List<CardLogic>();
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
        if (Data.Phase == PhaseType.Charactor)
        {
            DoAction();
        }
        else
        {
            switch (Data.EnemyAI)
            {
                case AIType.NPC:
                case AIType.Retarded:
                case AIType.Slime:
                case AIType.Goblin:
                case AIType.Pillar:
                case AIType.Cannon:
                case AIType.Guard:
                case AIType.Assailant:
                case AIType.Leader:
                    DoAction();
                    break;
                default:
                    NeedEndCalculate = true;
                    break;
            }
        }
    }

    void EndCalculate()
    {
        //print("End calculatorAI: " + gameObject.GetInstanceID().ToString());

        mActionState = AttackAnimType.None;
        mTargetObj.Clear();
        mCalculatorAI = false;
        if (ActionFinishEvent != null)
        {
            ActionFinishEvent(this);
        }
    }

    /// <summary>
    /// 决定释放技能还是普通攻击
    /// </summary>
    void DoAction()
    {
        if (Data.Skill != null && Data.Skill.ManaCost <= Data.MP)
        {
            // 释放技能
            if (DoSkill(Data.Skill))
            {
                Data.MP -= Data.Skill.ManaCost;
            }
        }
        else
        {
            // 普通攻击
            DoSkill(Data.NormalAttack);
        }
    }

    int mCurrentSkillID = -1;

    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="_skill"></param>
    /// <returns></returns>
    bool DoSkill(SkillData _skill)
    {
        bool result = false;

        mCurrentSkillID = _skill.ID;
        mTargetObj.Clear();

        switch (_skill.AttackAnim)
        {
            case AttackAnimType.NormalAttack:
                {
                    CardData[] list = GetTargets(FindTargetConditionType.LowHP, _skill);
                    if (list.Length > 0)
                    {
                        int count = 0;
                        for (int i = 0; i < list.Length; i++)
                        {
                            mActionState = AttackAnimType.NormalAttack;
                            mTargetObj.Add(list[i].Logic);
                            PlayAnimation(CharAnimationState.Attack);
                            count += 1;

                            if (count >= _skill.Count)
                            {
                                break;
                            }
                        }
                        result = true;
                    }
                    else
                    {
                        NeedEndCalculate = true;
                    }
                }
                break;
            case AttackAnimType.Arrow:
                {
                    CardData[] list = GetTargets(FindTargetConditionType.LowHP, _skill);
                    if (list.Length > 0)
                    {
                        int count = 0;
                        for (int i = 0; i < list.Length; i++)
                        {
                            mActionState = AttackAnimType.Arrow;
                            mTargetObj.Add(list[i].Logic);
                            PlayAnimation(CharAnimationState.Skill);
                            count += 1;

                            if (count >= _skill.Count)
                            {
                                break;
                            }
                        }
                        result = true;
                    }
                    else
                    {
                        NeedEndCalculate = true;
                    }
                }
                break;
            case AttackAnimType.FireBall:
                {
                    CardData[] list = GetTargets(FindTargetConditionType.LowHP, _skill);
                    if (list.Length > 0)
                    {
                        int count = 0;
                        for (int i = 0; i < list.Length; i++)
                        {
                            mActionState = AttackAnimType.FireBall;
                            mTargetObj.Add(list[i].Logic);
                            PlayAnimation(CharAnimationState.Skill);
                            count += 1;

                            if (count >= _skill.Count)
                            {
                                break;
                            }
                        }
                        result = true;
                    }
                    else
                    {
                        NeedEndCalculate = true;
                    }
                }
                break;
            case AttackAnimType.IceBall:
            case AttackAnimType.WindBall:
            case AttackAnimType.StoneBall:
            case AttackAnimType.LightBall:
            case AttackAnimType.DarkBall:
            case AttackAnimType.CannonBall:
                NeedEndCalculate = true;
                break;
            case AttackAnimType.HPHealth:
                {
                    CardData[] list = GetTargets(FindTargetConditionType.BeingHurt, _skill);
                    if (list.Length > 0)
                    {
                        int count = 0;
                        for (int i = 0; i < list.Length; i++)
                        {
                            mActionState = AttackAnimType.HPHealth;
                            mTargetObj.Add(list[i].Logic);
                            PlayAnimation(CharAnimationState.Skill);
                            count += 1;

                            if (count >= _skill.Count)
                            {
                                break;
                            }
                        }
                        result = true;
                    }
                    else
                    {
                        NeedEndCalculate = true;
                    }
                }
                break;
            case AttackAnimType.MPHealth:
                NeedEndCalculate = true;
                break;
        }

        return result;
    }

    bool mWaitingDamage = false;

    /// <summary>
    /// 产生伤害
    /// </summary>
    void DoDamage()
    {
        if (mTargetObj.Count == 0 || mCurrentSkillID == -1)
        {
            return;
        }

        SkillData skilldata = SkillManager.Instance.GetSkill(mCurrentSkillID);

        // 计算伤害
        foreach (CardLogic logic in mTargetObj)
        {
            int damage = EquationTool.CalculateDamage(Data, logic.GetComponent<CardData>());
            logic.Data.HP -= damage;

            // 记录仇恨
            if (logic.Data.LastAttackerID == Data.ID)
            {
                // 上次也是哥揍的
                if (logic.Data.AttackerHatred < skilldata.Hatred)
                {
                    logic.Data.AttackerHatred = skilldata.Hatred;
                }
                else
                {
                    logic.Data.AttackerHatred += (int)(skilldata.Hatred * 0.2f);
                    if (logic.Data.AttackerHatred > skilldata.Hatred * 3)
                    {
                        logic.Data.AttackerHatred = skilldata.Hatred * 3;
                    }
                }
                logic.Data.LastAttackerID = Data.ID;
            }
            else if (logic.Data.LastAttackerID != -1)
            {
                // 上次是被其他人揍的
                logic.Data.AttackerHatred -= skilldata.Hatred;
                if (logic.Data.AttackerHatred <= 0)
                {
                    logic.Data.AttackerHatred = skilldata.Hatred;
                    logic.Data.LastAttackerID = Data.ID;
                }
            }
            else
            {
                // 还没被人揍过
                logic.Data.AttackerHatred = skilldata.Hatred;
                logic.Data.LastAttackerID = Data.ID;
            }

            AudioSource.PlayClipAtPoint(Resources.Load("Sounds/Slash10", typeof(AudioClip)) as AudioClip, Vector3.zero);
            logic.CreateHitEffect(damage);
        }

        mWaitingDamage = false;
    }

    /// <summary>
    /// CardData实现按HP排序
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    static int CompareCardHP(CardData a, CardData b)
    {
        return (a.HP.CompareTo(b.HP));
    }

    /// <summary>
    ///  CardData实现按Element排序
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    static int CompareCardElement(CardData a, CardData b)
    {
        return (a.Element.CompareTo(b.Element));
    }

    CardData[] GetTargets(FindTargetConditionType _type, SkillData _skill)
    {
        List<CardData> tempCardList = new List<CardData>();
        foreach (CardData getChar in GameLogic.Instance.GetActionTargets(Data, _skill))
        {
            switch (_type)
            {
                case FindTargetConditionType.Random:
                    if (tempCardList.Count == 0)
                    {
                        tempCardList.Add(getChar);
                    }
                    else
                    {
                        tempCardList.Insert(Random.Range(0, tempCardList.Count), getChar);
                    }
                    break;
                case FindTargetConditionType.BeingHurt:
                    if (getChar.HP < getChar.HPMax)
                    {
                        tempCardList.Add(getChar);
                    }
                    break;
                case FindTargetConditionType.LowHP:
                    tempCardList.Add(getChar);
                    break;
                case FindTargetConditionType.HighHP:
                    tempCardList.Add(getChar);
                    break;
                case FindTargetConditionType.DiElement:
                    tempCardList.Add(getChar);
                    break;
                default:
                    break;
            }
        }

        switch (_type)
        {
            case FindTargetConditionType.Random:
                break;
            case FindTargetConditionType.BeingHurt:
                tempCardList.Sort(CompareCardHP);
                break;
            case FindTargetConditionType.LowHP:
                tempCardList.Sort(CompareCardHP);
                break;
            case FindTargetConditionType.HighHP:
                tempCardList.Sort(CompareCardHP);
                tempCardList.Reverse();
                break;
            case FindTargetConditionType.DiElement:
                {
                    List<CardData> temp = new List<CardData>();
                    for (int i = 0; i < tempCardList.Count; i++ )
                    {
                        if (tempCardList[i].Element != Data.FoElement)
                        {
                            temp.Add(tempCardList[i]);
                        }
                    }

                    foreach (CardData data in temp)
                    {
                        tempCardList.Remove(data);
                    }
                    foreach (CardData data in temp)
                    {
                        tempCardList.Add(data);
                    }
                    temp.Clear();
                    temp = null;
                }
                break;
            default:
                break;
        }

        return tempCardList.ToArray();
    }
}
