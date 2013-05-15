using UnityEngine;
using System.Collections;

public class CardUI : MonoBehaviour {
    public UISlider BloodBar;
    public UISlider ManaBar;
    public UISprite SelectSprite;
    public UISprite PhaseStoneSprite;

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
}
