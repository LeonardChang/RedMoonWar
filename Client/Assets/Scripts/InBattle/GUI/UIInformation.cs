using UnityEngine;
using System.Collections;

public class UIInformation : MonoBehaviour {
    [HideInInspector]
    public CardData StoreData = null;

    public UILabel TitleLabel;

    public UILabel HPLabel;
    public UILabel MPLabel;
    public UILabel AtkLabel;
    public UILabel DefLabel;
    public UILabel SpdLabel;
    public UILabel EnemyLabel;
    public UILabel SkillLabel;
    public UILabel LeaderSkillLabel;
    public UILabel LevelLabel;

    public UISprite CardFrontground;
    public UISprite CardSprite;
    public UISprite CardBackground;

    public UISlider BloodBar;
    public UISlider ManaBar;

    public UIImageButton BuyEnemyBtn;
    public UILabel BuyEnemyLabel;

    public GameObject BackgroundObj;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        if (StoreData == null)
        {
            return;
        }

        TitleLabel.text = StoreData.BaseData.Name;

        HPLabel.text = StoreData.HP.ToString();
        MPLabel.text = StoreData.MP.ToString();
        AtkLabel.text = StoreData.Atk.ToString();
        DefLabel.text = StoreData.Def.ToString();
        SpdLabel.text = StoreData.Spd.ToString();
        SkillLabel.text = (StoreData.BaseData.SkillID <= 0 ? "No" : SkillManager.Instance.GetSkill(StoreData.BaseData.SkillID).Name);
        LeaderSkillLabel.text = (StoreData.BaseData.LeaderSkillID <= 0 ? "No" : LeaderSkillManager.Instance.GetSkill(StoreData.BaseData.LeaderSkillID).Name);
        LevelLabel.text = "Lv." + StoreData.Level.ToString();

        BloodBar.sliderValue = (float)StoreData.HP / StoreData.HPMax;
        ManaBar.sliderValue = (float)StoreData.MP / StoreData.MPMax;

        switch (StoreData.Element)
        {
            case ElementType.Fire:
                CardFrontground.spriteName = "CardFrontground1";
                break;
            case ElementType.Water:
                CardFrontground.spriteName = "CardFrontground2";
                break;
            case ElementType.Wind:
                CardFrontground.spriteName = "CardFrontground3";
                break;
            case ElementType.Earth:
                CardFrontground.spriteName = "CardFrontground4";
                break;
            case ElementType.Light:
                CardFrontground.spriteName = "CardFrontground5";
                break;
            case ElementType.Dark:
                CardFrontground.spriteName = "CardFrontground6";
                break;
        }
        CardSprite.spriteName = StoreData.BaseData.CardSprite;
        CardSprite.MakePixelPerfect();

        EnemyLabel.text = StoreData.Phase == PhaseType.Enemy ? "[Enemy]" : "";
        CardBackground.color = StoreData.Phase == PhaseType.Enemy ? new Color(1, 1, 1) : new Color(0, 0, 0);

        if (StoreData.Phase == PhaseType.Enemy && StoreData.BuyPrice > 0)
        {
            BuyEnemyBtn.gameObject.SetActive(true);
            BackgroundObj.transform.localScale = new Vector3(554, 515, 1);
        }
        else
        {
            BuyEnemyBtn.gameObject.SetActive(false);
            BackgroundObj.transform.localScale = new Vector3(554, 421.5f, 1);
        }

        BuyEnemyLabel.text = StoreData.BuyPrice.ToString();
        BuyEnemyBtn.isEnabled = GameLogic.Instance.Apple >= StoreData.BuyPrice;
    }

    public void BuyEnemy()
    {
        if (StoreData != null)
        {
            StoreData.Logic.BuyEnemy();
        }

        GameLogic.Instance.HideInfomation();
    }
}
