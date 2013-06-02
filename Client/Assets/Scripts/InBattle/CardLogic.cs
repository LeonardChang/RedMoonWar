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

    float mClickBuff = 0;

    CardData mData = null;

    /// <summary>
    /// 获取Data组件
    /// </summary>
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

    /// <summary>
    /// 获取UI组件
    /// </summary>
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

        if (mClickBuff > 0)
        {
            mClickBuff -= Time.deltaTime;
            if (mClickBuff < 0)
            {
                mClickBuff = 0;
            }
        }
	}

    bool mSelect = false;
    bool mNeedRefresh = false;

    /// <summary>
    /// 卡片是否是选中状态
    /// </summary>
    public bool IsSelect
    {
        get
        {
            return mSelect;
        }
    }

    void OnPress(bool isPressed)
    {
        if (isPressed)
        {
            GameLogic.Instance.CleanClickBuff();

            if (mClickBuff <= 0)
            {
                mClickBuff = 0.3f;
            }
            else
            {
                mClickBuff = 0;
                GameLogic.Instance.ShowInfomation(Data);
            }
        }
    }

    void OnClick()
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

    /// <summary>
    /// 选中此卡
    /// </summary>
    /// <param name="_select"></param>
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
            if (!mCalculatorAI)
            {
                PlayAnimation(CharAnimationState.Select);
            }
        }
        else
        {
            if (!mCalculatorAI)
            {
                PlayAnimation(CharAnimationState.Idle);
            }
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

    void PlayAnimation(CharAnimationState _state)
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
            CreateTAnimation("Attacking");
        }
        else if (_event == "Attacking")
        {
            ActionStart();
        }
        else if (_event == "StartSkill")
        {
            ActionStart();
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
                gameObject.SetActive(false);
            }
        }
    }

    void ActionStart()
    {
        if (mTargetObj.Count > 0)
        {
            AttackAnimationData data = AttackAnimationManager.Instance.GetAttackAnimation(mActionState);
            CreateTAnimation(data.TAnimationName);

            mWaitingDamage = true;
            if (!string.IsNullOrEmpty(data.FlyPerfab))
            {
                foreach (CardLogic logic in mTargetObj)
                {
                    CreateFlyEffect(logic.gameObject.transform, data);
                }
            }
            Invoke("DoActionRusult", data.Delay);
            NeedEndCalculate = true;
        }
    }

    /// <summary>
    /// 创建RM动画
    /// </summary>
    /// <param name="_animation"></param>
    public void CreateTAnimation(string _animation)
    {
        if (string.IsNullOrEmpty(_animation))
        {
            return;
        }

        GameObject perfab = Resources.Load("Cards/Perfabs/TAnimation", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 40, -1);
        obj.transform.localScale = Vector3.one;

        obj.GetComponent<TAnimObject>().Name = _animation;
    }
    
    /// <summary>
    /// 创建被击动画
    /// </summary>
    /// <param name="_Damage"></param>
    public void CreateHitNumber(int _Damage, bool _double)
    {
        CreateTAnimation("Attack");
        AudioSource.PlayClipAtPoint(Resources.Load("Sounds/Slash10", typeof(AudioClip)) as AudioClip, Vector3.zero);

        GameObject perfab = Resources.Load("Cards/Perfabs/BloodLabel", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 70, -2);
        obj.transform.localScale = new Vector3(50, 50, 1);
        UILabel label = obj.GetComponent<UILabel>();
        if (_double)
        {
            label.text = "-" + _Damage.ToString() + "×2";
        }
        else
        {
            label.text = "-" + _Damage.ToString();
        }
        label.color = new Color(1, 0, 0);

        iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0), 0.25f);
    }

    /// <summary>
    /// 创建被治疗动画
    /// </summary>
    /// <param name="_Health"></param>
    public void CreateHealthNumber(int _Health)
    {
        CreateTAnimation("HealthHP");

        GameObject perfab = Resources.Load("Cards/Perfabs/BloodLabel", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 70, -2);
        obj.transform.localScale = new Vector3(50, 50, 1);
        UILabel label = obj.GetComponent<UILabel>();
        label.text = "+" + _Health;
        label.color = new Color(0.2f, 1, 0.2f);

        AudioSource.PlayClipAtPoint(Resources.Load("Sounds/Heal3", typeof(AudioClip)) as AudioClip, Vector3.zero);
    }

    /// <summary>
    /// 创建被加魔动画
    /// </summary>
    /// <param name="_Health"></param>
    public void CreateHealManaNumber(int _Health)
    {
        CreateTAnimation("HealthHP");

        GameObject perfab = Resources.Load("Cards/Perfabs/BloodLabel", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 70, -2);
        obj.transform.localScale = new Vector3(50, 50, 1);
        UILabel label = obj.GetComponent<UILabel>();
        label.text = "+" + _Health;
        label.color = new Color(0.2f, 0.2f, 1);

        AudioSource.PlayClipAtPoint(Resources.Load("Sounds/Heal3", typeof(AudioClip)) as AudioClip, Vector3.zero);
    }

    /// <summary>
    /// 创建死亡动画
    /// </summary>
    public void CreateDeathEffect()
    {
        PlayAnimation(CharAnimationState.Death);
    }

    public void CreateFlyEffect(Transform _target, AttackAnimationData _data)
    {
        switch (_data.ID)
        {
            case AttackAnimType.Arrow:
            case AttackAnimType.FireArrow:
            case AttackAnimType.IceArrow:
            case AttackAnimType.WindArrow:
            case AttackAnimType.EarthArrow:
            case AttackAnimType.LightArrow:
            case AttackAnimType.DarkArrow:
                {
                    GameObject perfab = Resources.Load("Cards/Perfabs/" + _data.FlyPerfab, typeof(GameObject)) as GameObject;
                    GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
                    obj.transform.parent = gameObject.transform.parent;
                    obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 50, -1);
                    obj.transform.localScale = new Vector3(9, 50, 1);

                    Vector3 from = transform.localPosition + new Vector3(0, 50, 0);
                    Vector3 to = _target.localPosition + new Vector3(0, 50, 0);
                    Vector3 dir = to - from;
                    dir.Normalize();
                    obj.transform.up = dir;

                    TweenPosition.Begin(obj, _data.Delay, to + new Vector3(0, 0, -1)).from = from;

                    ParticleKiller pk = obj.transform.FindChild("Star").gameObject.GetComponent<ParticleKiller>();
                    if (pk != null)
                    {
                        pk.KillTime = _data.Delay;
                        Destroy(obj, pk.DeathTime);
                    }
                    else
                    {
                        Destroy(obj, _data.Delay);
                    }
                }
                break;
            default:
                {
                    GameObject perfab = Resources.Load("Cards/Perfabs/" + _data.FlyPerfab, typeof(GameObject)) as GameObject;
                    GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
                    obj.transform.parent = gameObject.transform.parent;
                    obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 0, -1);
                    obj.transform.localScale = new Vector3(58, 70, 1);

                    Vector3 to = _target.localPosition + new Vector3(0, 50, 0);

                    TweenPositionEx.Begin(obj, _data.Delay, gameObject.transform.localPosition + new Vector3(Random.Range(0, 2) == 0 ? -300 : 300, Random.Range(0, 2) == 0 ? -300 : 300, -1), to + new Vector3(0, 0, -1), 0.75f).method = UITweener.Method.EaseInOut;
                    //Destroy(obj, _data.Delay);

                    ParticleKiller pk = obj.GetComponent<ParticleKiller>();
                    if (pk != null)
                    {
                        pk.KillTime = _data.Delay;
                    }
                    TrailKiller tk = obj.GetComponent<TrailKiller>();
                    if (tk != null)
                    {
                        tk.KillTime = _data.Delay;
                    }
                }
                break;
        }

        AudioSource.PlayClipAtPoint(Resources.Load("Sounds/" + _data.FlySound, typeof(AudioClip)) as AudioClip, Vector3.zero);
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

    /// <summary>
    /// 开始计算AI
    /// </summary>
    public void CalculateAI()
    {
        if (Data.Death)
        {
            EndCalculate();
            return;
        }
        
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
        if (Data.Skill != null && Data.AutoSkill && Data.Skill.GetManaCost(Data.SkillLevel) <= Data.MP)
        {
            // 释放技能
            if (DoSkill(Data.Skill, false))
            {
                Data.MP -= Data.Skill.GetManaCost(Data.SkillLevel);
                UI.ShowTalk(Data.Skill.Name);
            }
        }
        else
        {
            // 普通攻击
            DoSkill(Data.NormalAttack, Data.NormalAttack.AttackAnim == AttackAnimType.NormalAttack);
        }
    }

    int mCurrentSkillID = -1;

    /// <summary>
    /// 释放技能
    /// </summary>
    /// <param name="_skill"></param>
    /// <returns></returns>
    bool DoSkill(SkillData _skill, bool _isNormalAttack)
    {
        bool result = false;

        mCurrentSkillID = _skill.ID;
        mTargetObj.Clear();

        CardData[] list = GetTargets(_skill.SearchType, _skill);
        if (list.Length > 0)
        {
            int count = 0;
            for (int i = 0; i < list.Length; i++)
            {
                mActionState = _skill.AttackAnim;
                mTargetObj.Add(list[i].Logic);
                PlayAnimation(_isNormalAttack ? CharAnimationState.Attack : CharAnimationState.Skill);
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

        return result;
    }

    bool mWaitingDamage = false;

    void DoActionRusult()
    {
        mWaitingDamage = false;

        if (mTargetObj.Count == 0 || mCurrentSkillID == -1)
        {
            return;
        }

        AttackAnimationData data = AttackAnimationManager.Instance.GetAttackAnimation(mActionState);
        switch (mActionState)
        {
            case AttackAnimType.AtkUp:
            case AttackAnimType.DefUp:
            case AttackAnimType.SpdUp:
            case AttackAnimType.HPUp:
            case AttackAnimType.MPUp:
            case AttackAnimType.BuffClear:
                break;
            case AttackAnimType.HPHealth:
                {
                    SkillData skilldata = SkillManager.Instance.GetSkill(mCurrentSkillID);
                    int heal = skilldata.FixedDamage + (int)(Data.Atk * skilldata.MultiplyDamage);
                    foreach (CardLogic logic in mTargetObj)
                    {
                        logic.Data.HP += heal;
                        logic.CreateHealthNumber(heal);
                    }
                }
                break;
            case AttackAnimType.MPHealth:
                {
                    SkillData skilldata = SkillManager.Instance.GetSkill(mCurrentSkillID);
                    int heal = 30;
                    foreach (CardLogic logic in mTargetObj)
                    {
                        logic.Data.MP += heal;
                        logic.CreateHealManaNumber(heal);
                    }
                }
                break;
            case AttackAnimType.GodHeal:
                {
                    foreach (CardLogic logic in mTargetObj)
                    {
                        int heal = Data.HPMax;
                        logic.Data.HP += heal;
                        logic.CreateHealthNumber(heal);
                    }

                    foreach (CardLogic logic in mTargetObj)
                    {
                        int heal = Data.MPMax;
                        logic.Data.MP += heal;
                        logic.CreateHealManaNumber(heal);
                    }
                }
                break;
            default:
                DoDamage();
                break;
        }
        
        if (!string.IsNullOrEmpty(data.HitTAnimation))
        {
            foreach (CardLogic logic in mTargetObj)
            {
                logic.CreateTAnimation(data.HitTAnimation);
            }
        }
    }

    /// <summary>
    /// 产生伤害
    /// </summary>
    void DoDamage()
    {
        SkillData skilldata = SkillManager.Instance.GetSkill(mCurrentSkillID);

        // 计算伤害
        foreach (CardLogic logic in mTargetObj)
        {
            int damage = EquationTool.CalculateDamage(Data, logic.Data, skilldata);
            bool doubledamage = false;
            if (Data.FoElement == logic.Data.Element)
            {
                doubledamage = true;
            }
            logic.CreateHitNumber(damage, doubledamage);
            logic.Data.HP -= damage * (doubledamage ? 2 : 1);

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

            //switch (mActionState)
            //{
            //    case AttackAnimType.FireBall:
            //        logic.CreateFireBallEndEffect();
            //        break;
            //    case AttackAnimType.LightBall:
            //        logic.CreateLightBallEndEffect();
            //        break;
            //    case AttackAnimType.DarkBall:
            //        logic.CreateDarkBallEndEffect();
            //        break;
            //    default:
            //        break;
            //}
        }
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

    static int CompareCardBuffCount(CardData a, CardData b)
    {
        return (a.CurrentBuff.Length.CompareTo(b.CurrentBuff.Length));
    }

    static int CompareCardMP(CardData a, CardData b)
    {
        return (a.MP.CompareTo(b.MP));
    }

    /// <summary>
    /// 搜索目标
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_skill"></param>
    /// <returns></returns>
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
                case FindTargetConditionType.HasDebuff:
                    if (getChar.CurrentBuff.Length > 0)
                    {
                        tempCardList.Add(getChar);
                    }
                    break;
                case FindTargetConditionType.NoBuff:
                    tempCardList.Add(getChar);
                    break;
                case FindTargetConditionType.LowMP:
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
            case FindTargetConditionType.HasDebuff:
                break;
            case FindTargetConditionType.NoBuff:
                tempCardList.Sort(CompareCardBuffCount);
                break;
            case FindTargetConditionType.LowMP:
                tempCardList.Sort(CompareCardMP);
                break;
            default:
                break;
        }

        return tempCardList.ToArray();
    }
}
