using UnityEngine;
using System.Collections;

public class EquationTool
{
    /// <summary>
    /// 伤害计算公式
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_target"></param>
    /// <returns></returns>
    public static int CalculateDamage(CardData _from, CardData _target)
    {
        int damage = (int)((_from.Atk - _target.Def * 0.5f) * 2);

        if (damage < 1)
        {
            damage = 1;
        }

        if ((_from.Element == ElementType.Fire && _target.Element == ElementType.Wind)
            || (_from.Element == ElementType.Wind && _target.Element == ElementType.Earth)
            || (_from.Element == ElementType.Earth && _target.Element == ElementType.Water)
            || (_from.Element == ElementType.Water && _target.Element == ElementType.Fire)
            || (_from.Element == ElementType.Light && _target.Element == ElementType.Dark)
            || (_from.Element == ElementType.Dark && _target.Element == ElementType.Light))
        {
            damage *= 2;
        }

        return damage;
    }

    /// <summary>
    /// 计算升级所需经验
    /// </summary>
    /// <param name="_targetLevel"></param>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static int CalculateNextEXP(int _targetLevel, GrowType _type)
    {
        switch (_type)
        {
            case GrowType.TypeA:
                return 100;
            case GrowType.TypeB:
                return 1000;
            case GrowType.TypeC:
                return 10000;
        }

        return int.MaxValue;
    }
}
