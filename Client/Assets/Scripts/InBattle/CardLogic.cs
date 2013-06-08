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
            EndCalculate(mNeedWaitAtEnd);
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
    public GameObject CreateTAnimation(string _animation)
    {
        if (string.IsNullOrEmpty(_animation))
        {
            return null;
        }

        GameObject perfab = Resources.Load("Cards/Perfabs/TAnimation", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 40, -1);
        obj.transform.localScale = Vector3.one;

        obj.GetComponent<TAnimObject>().Name = _animation;

        return obj;
    }
    
    /// <summary>
    /// 创建被击动画
    /// </summary>
    /// <param name="_Damage"></param>
    public GameObject CreateHitNumber(int _Damage, bool _double)
    {
        AudioCenter.Instance.PlaySound("Slash10");

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

        return obj;
    }

    /// <summary>
    /// 创建被治疗动画
    /// </summary>
    /// <param name="_Health"></param>
    public GameObject CreateHealthNumber(int _Health)
    {
        GameObject perfab = Resources.Load("Cards/Perfabs/BloodLabel", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 70, -2);
        obj.transform.localScale = new Vector3(50, 50, 1);
        UILabel label = obj.GetComponent<UILabel>();
        label.text = "+" + _Health;
        label.color = new Color(0.2f, 1, 0.2f);

        AudioCenter.Instance.PlaySound("Heal3");

        return obj;
    }

    /// <summary>
    /// 创建被加魔动画
    /// </summary>
    /// <param name="_Health"></param>
    public GameObject CreateHealManaNumber(int _Health)
    {
        GameObject perfab = Resources.Load("Cards/Perfabs/BloodLabel", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 70, -2);
        obj.transform.localScale = new Vector3(50, 50, 1);
        UILabel label = obj.GetComponent<UILabel>();
        label.text = "+" + _Health;
        label.color = new Color(0.2f, 0.2f, 1);

        AudioCenter.Instance.PlaySound("Heal3");

        return obj;
    }

    /// <summary>
    /// 创建地面碎裂效果
    /// </summary>
    public void CreateCrack()
    {
        int type = Random.Range(1, 3);

        GameObject perfab = Resources.Load("Cards/Perfabs/Crack" + type.ToString(), typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 30, 0);
        obj.transform.localScale = Vector3.one * Random.Range(1.0f, 1.5f);
        obj.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }

    /// <summary>
    /// 创建墓碑
    /// </summary>
    public void CreateGrave()
    {
        GameObject perfab = Resources.Load("Cards/Perfabs/Grave", typeof(GameObject)) as GameObject;
        GameObject obj = GameObject.Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.parent = gameObject.transform.parent;
        obj.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 40, 0);
        obj.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 创建死亡动画
    /// </summary>
    public void CreateDeathEffect()
    {
        PlayAnimation(CharAnimationState.Death);
        CreateGrave();
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

        AudioCenter.Instance.PlaySound(_data.FlySound);
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
    bool mNeedWaitAtEnd = false;

    /// <summary>
    /// 开始计算AI
    /// </summary>
    public void CalculateAI()
    {
        mNeedWaitAtEnd = false;
        mCalculatorAI = true;

        if (Data.Phase == PhaseType.Enemy && (Data.Y > GameLogic.Instance.BottomLine + 10 || Data.Y < GameLogic.Instance.BottomLine - 5))
        {
            EndCalculate(false);
            return;
        }

        if (Data.Death)
        {
            EndCalculate(false);
            return;
        }

        // 睡眠或眩晕中
        foreach (RealBuffData buff in Data.CurrentBuff)
        {
            if (buff.mSleep)
            {
                EndCalculate(false);
                CreateTAnimation("Sleep");
                return;
            }

            if (buff.mDizziness)
            {
                EndCalculate(false);
                CreateTAnimation("Yun");
                return;
            }
        }

        if (Data.Phase == PhaseType.Enemy)
        {
            switch (Data.EnemyAI)
            {
                case AIType.NPC:
                case AIType.Pillar:
                    EndCalculate(false);
                    break;
                case AIType.Retarded:
                    if (Random.Range(0, 5) > 5)
                    {
                        DoAction();
                    }
                    else
                    {
                        EndCalculate(false);
                    }
                    break;
                default:
                    DoAction();
                    break;
            }
        }
        else
        {
            DoAction();
        }
    }

    void EndCalculate(bool _wait)
    {
        bool wait = _wait;

        mActionState = AttackAnimType.None;
        mTargetObj.Clear();
        mCalculatorAI = false;
		
		if (!Data.Death)
		{
	        foreach (RealBuffData buff in Data.CurrentBuff)
	        {
	            Data.HP += buff.mAddHP;
	            Data.MP += buff.mAddMP;
	
	            switch ((BuffEnum)buff.mBuffID)
	            {
	                case BuffEnum.Poison:
	                    CreateTAnimation("Poisoning");
	                    CreateHitNumber(-buff.mAddHP, false);
	                    break;
	                case BuffEnum.AddHPPerround:
	                    CreateTAnimation("HealthHP");
	                    CreateHealthNumber(buff.mAddHP);
	                    break;
	                case BuffEnum.AddMPPerround:
	                    CreateTAnimation("HealthMP");
	                    CreateHealManaNumber(buff.mAddMP);
	                    break;
                    case BuffEnum.AddAllPerround:
                        {
                            CreateTAnimation("HealthHP");
                            CreateHealthNumber(buff.mAddHP);

                            CreateTAnimation("HealthMP");
                            GameObject obj = CreateHealManaNumber(buff.mAddMP);
                            if (buff.mAddHP != 0)
                            {
                                obj.GetComponent<Blood>().YOffset = 30;
                            }
                        }
                        break;
	            }
	
	            wait = true;
	        }
		}

        // data数据经过一回合
        Data.PastOneRound();

        if (wait)
        {
            // 如行动过则稍微等一会儿再结束
            Invoke("CallActionFinishEvent", 0.5f);
        }
        else
        {
            CallActionFinishEvent();
        }
    }

    void CallActionFinishEvent()
    {
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
        bool disableSkill = false;
        foreach (RealBuffData buff in Data.CurrentBuff)
        {
            if (buff.mDisableSkill)
            {
                disableSkill = true;
                break;
            }
        }

        if (!disableSkill && Data.Skill != null && Data.IsSkillReady && Data.AutoSkill && Data.Skill.GetManaCost(Data.SkillLevel) <= Data.MP)
        {
            // 释放技能
            if (DoSkill(Data.Skill, false))
            {
                Data.MP -= Data.Skill.GetManaCost(Data.SkillLevel);
                UI.ShowTalk(Data.Skill.Name);
                mNeedWaitAtEnd = true;
                Data.StartSkillColdDown();
            }
        }
        else
        {
            // 普通攻击
            if (DoSkill(Data.NormalAttack, Data.NormalAttack.AttackAnim == AttackAnimType.NormalAttack))
            {
                mNeedWaitAtEnd = true;
            }
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

                if (count >= _skill.Count && _skill.ID != (int)SpecialSkillID.AttackAll1 && _skill.ID != (int)SpecialSkillID.AttackAll2)
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
        switch ((SpecialSkillID)mActionState)
        {
            case SpecialSkillID.HealDebuff1:
            case SpecialSkillID.HealDebuff2:
                if (mCurrentSkillID == (int)SpecialSkillID.HealDebuff1 || mCurrentSkillID == (int)SpecialSkillID.HealDebuff2)
                {
                    foreach (CardLogic logic in mTargetObj)
                    {
                        logic.Data.ClearAllDebuff();
                    }
                }
                break;
            case SpecialSkillID.HealToMax:
                foreach (CardLogic logic in mTargetObj)
                {
                    int heal = Data.HPMax;
                    logic.Data.HP += heal;
                    logic.CreateHealthNumber(heal);

                    heal = Data.MPMax;
                    logic.Data.MP += heal;
                    GameObject obj = logic.CreateHealManaNumber(heal);
                    obj.GetComponent<Blood>().YOffset = 30;
                }
                break;
            case SpecialSkillID.HealHP1:
            case SpecialSkillID.HealHP2:
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
            case SpecialSkillID.HealMP:
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
            default:
                switch (mActionState)
                {
                    case AttackAnimType.AtkUp:
                    case AttackAnimType.DefUp:
                    case AttackAnimType.SpdUp:
                    case AttackAnimType.HPUp:
                    case AttackAnimType.MPUp:
                    case AttackAnimType.GodHeal:
                        break;
                    default:
                        DoDamage();
                        break;
                }
                break;
        }

        // 技能会附加的buff
        foreach (CardLogic logic in mTargetObj)
        {
            SkillData skilldata = SkillManager.Instance.GetSkill(mCurrentSkillID);
            if (skilldata.AddBuff != 0)
            {
                logic.Data.AddNewBuff(skilldata.AddBuff, Data);
            }
        }
        
        if (!string.IsNullOrEmpty(data.HitTAnimation))
        {
            foreach (CardLogic logic in mTargetObj)
            {
                GameObject obj = logic.CreateTAnimation(data.HitTAnimation);
                if (obj != null)
                {
                    obj.transform.localScale *= data.HitTAnimationScale;
                }
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
            bool doubledamage = false;
            int damage = 0;
            bool canFixDamage = true;
            switch ((SpecialSkillID)mCurrentSkillID)
            {
                case SpecialSkillID.SexDream:
                    damage = Mathf.FloorToInt(logic.Data.HP * 0.33f);
                    if (damage < 1)
                    {
                        damage = 1;
                    }
                    break;
                case SpecialSkillID.Dead1:
                    if (Random.Range(0, 100) < 10)
                    {
                        damage = logic.Data.HP;
                        canFixDamage = false;
                    }
                    else
                    {
                        damage = EquationTool.CalculateDamage(Data, logic.Data, skilldata, ref doubledamage);
                    }
                    break;
                case SpecialSkillID.Dead2:
                    if (Random.Range(0, 100) < 20)
                    {
                        damage = logic.Data.HP;
                        canFixDamage = false;
                    }
                    else
                    {
                        damage = EquationTool.CalculateDamage(Data, logic.Data, skilldata, ref doubledamage);
                    }
                    break;
                case SpecialSkillID.Dead3:
                    if (Random.Range(0, 100) < 40)
                    {
                        damage = logic.Data.HP;
                        canFixDamage = false;
                    }
                    else
                    {
                        damage = EquationTool.CalculateDamage(Data, logic.Data, skilldata, ref doubledamage);
                    }
                    break;
                default:
                    damage = EquationTool.CalculateDamage(Data, logic.Data, skilldata, ref doubledamage);
                    break;
            }

            if (canFixDamage)
            {
                // 重伤状态，1.5倍伤害
                foreach (RealBuffData buff in logic.Data.CurrentBuff)
                {
                    if (buff.mGreatDamage)
                    {
                        damage = Mathf.FloorToInt(damage * 1.5f);
                        break;
                    }
                }

                // 致盲状态，伤害50%几率归1
                foreach (RealBuffData buff in Data.CurrentBuff)
                {
                    if (buff.mCanMiss)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            damage = doubledamage ? 2 : 1;
                        }
                        break;
                    }
                }

                // 主将技能减伤影响
                if (logic.Data.Phase == PhaseType.Charactor)
                {
                    LeaderSkillData skill1 = LeaderSkillManager.Instance.GetSkill(GameLogic.Instance.LeaderSkill1);
                    LeaderSkillData skill2 = LeaderSkillManager.Instance.GetSkill(GameLogic.Instance.LeaderSkill2);
                    if (skill1 != null
                        && (skill1.Element == ElementType.None || skill1.Element == Data.Element))
                    {
                        damage = Mathf.RoundToInt((float)damage * skill1.DamageDown);
                    }
                    if (skill2 != null
                        && (skill2.Element == ElementType.None || skill2.Element == Data.Element))
                    {
                        damage = Mathf.RoundToInt((float)damage * skill2.DamageDown);
                    }
                }
            }

            logic.CreateHitNumber(damage, doubledamage);
            logic.Data.HP -= damage;

            if ((doubledamage && damage >= 100) || (!doubledamage && damage >= 500))
            {
                logic.CreateCrack();
            }

            if (doubledamage)
            {
                GameLogic.Instance.ShakeMap(1);
            }

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
        return (a.CurrentBuffCount.CompareTo(b.CurrentBuffCount));
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
                    if (getChar.CurrentBadBuffCount > 0)
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
                    tempCardList.Add(getChar);
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
                        if (tempCardList[i].BeElement != Data.Element)
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
