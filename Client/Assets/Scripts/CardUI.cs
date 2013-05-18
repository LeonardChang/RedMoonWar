using UnityEngine;
using System.Collections;

public class CardUI : MonoBehaviour {
    public UISlider BloodBar;
    public UISlider ManaBar;
    public UISprite SelectSprite;
    public UISprite PhaseStoneSprite;

    public UISprite FrontSprite;
    public UISprite AvatarSprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float Blood
    {
        set
        {
            BloodBar.sliderValue = value;
        }
    }

    public float Mana
    {
        set
        {
            ManaBar.sliderValue = value;
        }
    }

    public PhaseType Phase
    {
        set
        {
            switch (value)
            {
                case PhaseType.Charactor:
                    PhaseStoneSprite.spriteName = "Bloodbar06";
                    break;
                case PhaseType.Enemy:
                    PhaseStoneSprite.spriteName = "Bloodbar05";
                    break;
            }
        }
    }

    public bool Select
    {
        set
        {
            SelectSprite.gameObject.active = value;
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
}
