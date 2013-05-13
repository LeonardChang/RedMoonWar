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

public class CardLogic : MonoBehaviour {
    public GameObject SelectSprite;
    public UISlider BloodBar;
    public Animation CardAnimation;
    public UISprite BloodBarBackground;
    
    CharAnimationState mAnimationState = CharAnimationState.NotInit;

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
                break;
            case CharAnimationState.Skill:
                CardAnimation.Play("Skill");
                break;
            case CharAnimationState.Death:
                CardAnimation.Play("Death");
                break;
            case CharAnimationState.Join:
                CardAnimation.Play("Join");
                break;
        }
    }

    void AnimationEventCall(string _event)
    {
        if (_event == "StartAttack")
        {
            CreateStartAttackEffect();
        }
        else if (_event == "Attacking")
        {
            if (mTargetObj != null)
            {
                int damage = CalculatorDamage(SelfData, mTargetObj.GetComponent<CardData>());
                mTargetObj.GetComponent<CardData>().HP -= damage;
                mTargetObj.GetComponent<CardLogic>().CreateHitEffect(damage);
            }
        }
        else if (_event == "StartSkill")
        {
        }
        else if (_event == "Joining")
        {
        }
        else if (_event == "Finish")
        {
            if (mCalculatorAI)
            {
                EndCalculator();
            }

            if (SelfData.Death)
            {
                gameObject.SetActiveRecursively(false);
            }
        }
    }

    CardData SelfData
    {
        get
        {
            return gameObject.GetComponent<CardData>();
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

        iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0), 0.25f);
    }

    public System.Action<CardLogic> ActionFinishEvent;

    GameObject mTargetObj = null;
    bool mCalculatorAI = false;
    public void CalculatorAI()
    {
        //print("Start calculatorAI: " + gameObject.name);

        mCalculatorAI = true;
        CardData data = SelfData;
        if (data.Phase == PhaseType.Charactor)
        {
            switch (data.CardAction)
            {
                case ActionType.NormalAttack:
                case ActionType.MagicAttack:
                case ActionType.ArrowAttack:
                case ActionType.Health:
                    {
                        CardData enemy = GetActionTarget(PhaseType.Enemy);
                        if (enemy != null)
                        {
                            mTargetObj = enemy.gameObject;
                            PlayAnimation(CharAnimationState.Attack);
                        }
                        else
                        {
                            EndCalculator();
                        }
                    }
                    break;
                default:
                    EndCalculator();
                    break;
            }
        }
        else
        {
            EndCalculator();
        }
    }

    void EndCalculator()
    {
        //print("End calculatorAI: " + gameObject.name);

        mTargetObj = null;
        mCalculatorAI = false;
        if (ActionFinishEvent != null)
        {
            ActionFinishEvent(this);
        }
    }

    CardData GetActionTarget(PhaseType _targetPhase)
    {
        return GameLogic.Instance.GetActionTarget(SelfData, _targetPhase);
    }

    int CalculatorDamage(CardData _from, CardData _target)
    {
        return (int)((_from.Atk - _target.Def * 0.5f) * 2);
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
