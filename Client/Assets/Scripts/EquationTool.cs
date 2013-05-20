using UnityEngine;
using System.Collections;

public class EquationTool
{
    /// <summary>
    /// 伤害计算
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_target"></param>
    /// <returns></returns>
    public static int CalculateDamage(CardData _from, CardData _target, SkillData _skill)
    {
        // 基础伤害
        int damage = Mathf.CeilToInt((_from.Atk * 4 - _target.Def * 2) * _skill.MultiplyDamage) + _skill.FixedDamage;

        // 属性克制, 2倍伤害
        if (_from.FoElement == _target.Element)
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

    /// <summary>
    /// 计算卡牌升级所需经验
    /// </summary>
    /// <param name="_targetLevel"></param>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static int CalculateCardNextEXP(int _targetLevel, GrowType _type)
    {
        switch (_type)
        {
            case GrowType.TypeA:
                return Mathf.FloorToInt(_targetLevel * _targetLevel * 4) + 400;
            case GrowType.TypeB:
                return Mathf.FloorToInt(_targetLevel * _targetLevel * 4.4f) + 560;
            case GrowType.TypeC:
                return Mathf.FloorToInt(_targetLevel * _targetLevel * 12) + 610;
        }

        return int.MaxValue;
    }

    /// <summary>
    /// 计算玩家升级所需经验
    /// </summary>
    /// <param name="_targetLevel"></param>
    /// <returns></returns>
    public static int CalculatePlayerNextEXP(int _targetLevel)
    {
        return Mathf.FloorToInt(_targetLevel * _targetLevel * 14.6f) + 2100;
    }
}
