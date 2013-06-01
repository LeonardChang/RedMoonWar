using UnityEngine;
using System.Collections;
using System.IO;

public class AttackSimulator : MonoBehaviour {
    public int Count = 100;

	// Use this for initialization
	void Start () {
        StreamWriter writer = new StreamWriter("H:/Works/Battle.txt", false, System.Text.Encoding.Unicode);
        writer.WriteLine("战役名\t胜利者\t使用回合\t等级");
        for (int i = 0; i < Count; i++)
        {
            string result = OneBattle(i);
            writer.WriteLine(result);
        }
        writer.Close();
        print("Finish!");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    string OneBattle(int _index)
    {
        int level = Random.Range(1, 99);
        CharacterData char1 = CharacterManager.Instance.CreateRandomCharactor(_index * 100 + 1, level);
        CharacterData char2 = CharacterManager.Instance.CreateRandomCharactor(_index * 100 + 2, level);

        int char1HP = char1.MaxHP;
        int char2HP = char2.MaxHP;

        CardBaseData char1Card = CardManager.Instance.GetCard(char1.CardID);
        CardBaseData char2Card = CardManager.Instance.GetCard(char2.CardID);

        int roundCount = 0;
        while (char1HP > 0 || char2HP > 0)
        {
            roundCount += 1;
            if (char1.Spd > char2.Spd)
            {
                char2HP -= CalculateDamage(char1.Atk, char2.Def, SkillManager.Instance.GetSkill(char1Card.NormalAttackSkillID), isFoElement(char1Card.Element, char2Card.Element));
                if (char2HP < 0)
                {
                    char2HP = 0;
                    break;
                }
                else
                {
                    char1HP -= CalculateDamage(char2.Atk, char1.Def, SkillManager.Instance.GetSkill(char2Card.NormalAttackSkillID), isFoElement(char2Card.Element, char1Card.Element));
                    if (char1HP < 0)
                    {
                        char1HP = 0;
                        break;
                    }
                }
            }
            else
            {
                char1HP -= CalculateDamage(char2.Atk, char1.Def, SkillManager.Instance.GetSkill(char2Card.NormalAttackSkillID), isFoElement(char2Card.Element, char1Card.Element));
                if (char1HP < 0)
                {
                    char1HP = 0;
                    break;
                }
                else
                {
                    char2HP -= CalculateDamage(char1.Atk, char2.Def, SkillManager.Instance.GetSkill(char1Card.NormalAttackSkillID), isFoElement(char1Card.Element, char2Card.Element));
                    if (char2HP < 0)
                    {
                        char2HP = 0;
                        break;
                    }
                }
            }
        }

        string char1Name = "1." + char1Card.Name + "[" + char1Card.StarCount.ToString() + "★]";
        string char2Name = "2." + char2Card.Name + "[" + char2Card.StarCount.ToString() + "★]";

        return string.Format("{0:S} VS {1:S}\t{2:S} win\t{3:D}\t{4:D}", char1Name, char2Name, char2HP <= 0 ? char1Name : char2Name, roundCount, level);
    }

    int CalculateDamage(int _fromATK, int _targetDef, SkillData _skill, bool _isFo)
    {
        // 基础伤害
        int damage = Mathf.CeilToInt((_fromATK * 4 - _targetDef * 2) * _skill.MultiplyDamage) + _skill.FixedDamage;

        //UnityEngine.Debug.Log(string.Format("(Atk[{0:D}] * 4 - Def[{1:D}] * 2) * Skill[{2:S}] = {3:D}", _from.Atk, _target.Def, _skill.MultiplyDamage.ToString("f2"), damage));

        // 属性克制, 2倍伤害
        if (_isFo)
        {
            damage = damage * 2;
        }

        // +-5%的离散值
        damage = Mathf.CeilToInt(damage * Random.Range(1.05f, 0.95f));

        // 未破防保护
        if (damage < 1)
        {
            damage = 1;
        }

        return damage;
    }

    bool isFoElement(ElementType _from, ElementType _to)
    {
        switch (_from)
        {
            case ElementType.Fire:
                return _to == ElementType.Wind;
            case ElementType.Water:
                return _to == ElementType.Fire;
            case ElementType.Wind:
                return _to == ElementType.Earth;
            case ElementType.Earth:
                return _to == ElementType.Water;
            case ElementType.Light:
                return _to == ElementType.Dark;
            case ElementType.Dark:
                return _to == ElementType.Light;
            default:
                return false;
        }
    }
}
