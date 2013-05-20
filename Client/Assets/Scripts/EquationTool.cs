using UnityEngine;
using System.Collections;

public class EquationTool
{
    /// <summary>
    /// �˺�����
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_target"></param>
    /// <returns></returns>
    public static int CalculateDamage(CardData _from, CardData _target, SkillData _skill)
    {
        // �����˺�
        int damage = Mathf.CeilToInt((_from.Atk * 4 - _target.Def * 2) * _skill.MultiplyDamage) + _skill.FixedDamage;

        // ���Կ���, 2���˺�
        if (_from.FoElement == _target.Element)
        {
            damage = damage * 2;
        }

        // +-5%����ɢֵ
        damage = Mathf.CeilToInt(damage * Random.Range(1.05f, 0.95f));

        // δ�Ʒ�����
        if (damage < 1)
        {
            damage = 1;
        }

        return damage;
    }

    /// <summary>
    /// ���㿨���������辭��
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
    /// ��������������辭��
    /// </summary>
    /// <param name="_targetLevel"></param>
    /// <returns></returns>
    public static int CalculatePlayerNextEXP(int _targetLevel)
    {
        return Mathf.FloorToInt(_targetLevel * _targetLevel * 14.6f) + 2100;
    }
}
