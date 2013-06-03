using UnityEngine;
using System.Collections;

public class CardUI : MonoBehaviour {
    public UISlider BloodBar;
    public UISlider ManaBar;
    public UISprite SelectSprite;

    public UISprite FrontSprite;
    public UISprite AvatarSprite;
    public UISprite BackSprite;

    public GameObject TalkRoot;
    public UILabel TalkLabel;
    public UISprite TalkBackground;

    public UISprite BuffSprite;
    public UISprite LeaderSprite;

    float mBloodTargetValue = 0;
    float mManaTargetValue = 0;

    void Awake()
    {
        BloodBar.sliderValue = mBloodTargetValue;
        ManaBar.sliderValue = mManaTargetValue;
        Buff = "";
        RealHideTalk();
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (BloodBar.sliderValue < mBloodTargetValue)
	    {
            float val = BloodBar.sliderValue + 0.5f * Time.deltaTime;
            if (val >= mBloodTargetValue)
            {
                val = mBloodTargetValue;
            }
            BloodBar.sliderValue = val;
	    }
        else if (BloodBar.sliderValue > mBloodTargetValue)
        {
            float val = BloodBar.sliderValue - 0.5f * Time.deltaTime;
            if (val <= mBloodTargetValue)
            {
                val = mBloodTargetValue;
            }
            BloodBar.sliderValue = val;
        }

        if (ManaBar.sliderValue < mManaTargetValue)
        {
            float val = ManaBar.sliderValue + 0.5f * Time.deltaTime;
            if (val >= mManaTargetValue)
            {
                val = mManaTargetValue;
            }
            ManaBar.sliderValue = val;
        }
        else if (ManaBar.sliderValue > mManaTargetValue)
        {
            float val = ManaBar.sliderValue - 0.5f * Time.deltaTime;
            if (val <= mManaTargetValue)
            {
                val = mManaTargetValue;
            }
            ManaBar.sliderValue = val;
        }
	}

    public float Blood
    {
        set
        {
            //BloodBar.sliderValue = value;
            mBloodTargetValue = value;
        }
    }

    public float Mana
    {
        set
        {
            //ManaBar.sliderValue = value;
            mManaTargetValue = value;
        }
    }

    public void ForceReset()
    {
        BloodBar.sliderValue = mBloodTargetValue;
        ManaBar.sliderValue = mManaTargetValue;
    }

    public PhaseType Phase
    {
        set
        {
            switch (value)
            {
                case PhaseType.Charactor:
                    BackSprite.color = new Color(0, 0, 0);
                    break;
                case PhaseType.Enemy:
                    BackSprite.color = new Color(1, 1, 1);
                    break;
            }
        }
    }

    public bool Select
    {
        set
        {
            SelectSprite.gameObject.SetActive(value);
        }
    }

    public ElementType Element
    {
        set
        {
            switch (value)
            {
                case ElementType.Fire:
                    FrontSprite.spriteName = "CardFrontground1";
                    break;
                case ElementType.Water:
                    FrontSprite.spriteName = "CardFrontground2";
                    break;
                case ElementType.Wind:
                    FrontSprite.spriteName = "CardFrontground3";
                    break;
                case ElementType.Earth:
                    FrontSprite.spriteName = "CardFrontground4";
                    break;
                case ElementType.Light:
                    FrontSprite.spriteName = "CardFrontground5";
                    break;
                case ElementType.Dark:
                    FrontSprite.spriteName = "CardFrontground6";
                    break;
            }
        }
    }

    public string SpriteName
    {
        set
        {
            AvatarSprite.spriteName = value;
            AvatarSprite.MakePixelPerfect();
        }
    }

    public void ShowTalk(string _talk)
    {
        TalkRoot.SetActive(true);

        TalkLabel.text = _talk;
        TweenColor.Begin(TalkLabel.gameObject, 0.15f, new Color(1, 0, 0, 1)).from = new Color(1, 0, 0, 0);
        TweenColor tc = TweenColor.Begin(TalkBackground.gameObject, 0.15f, new Color(1, 1, 1, 1));
        tc.from = new Color(1, 1, 1, 0);
        tc.eventReceiver = null;
        tc.callWhenFinished = "";

        Invoke("HideTalk", 1);
    }

    void HideTalk()
    {
        TweenColor.Begin(TalkLabel.gameObject, 0.15f, new Color(1, 0, 0, 0)).from = new Color(1, 0, 0, 1);
        TweenColor tc = TweenColor.Begin(TalkBackground.gameObject, 0.15f, new Color(1, 1, 1, 0));
        tc.from = new Color(1, 1, 1, 1);
        tc.eventReceiver = gameObject;
        tc.callWhenFinished = "RealHideTalk";
    }

    void RealHideTalk()
    {
        TalkRoot.SetActive(false);
    }

    public string Buff
    {
        set
        {
            string val = value;
            BuffSprite.alpha = string.IsNullOrEmpty(val) ? 0 : 1;
            BuffSprite.spriteName = val;
            BuffSprite.MakePixelPerfect();
        }
    }

    public bool IsLeader
    {
        set
        {
            LeaderSprite.gameObject.SetActive(value);
        }
    }
}
