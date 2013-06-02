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

        //UnityEngine.Debug.Log(string.Format("(Atk[{0:D}] * 4 - Def[{1:D}] * 2) * Skill[{2:S}] = {3:D}", _from.Atk, _target.Def, _skill.MultiplyDamage.ToString("f2"), damage));

        // ���Կ���, 2���˺�
        //if (_from.FoElement == _target.Element)
        //{
        //    damage = damage * 2;
        //}

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
    public static int CalculateCardNextEXP(int _currentLevel, GrowType _type)
    {
        switch (_type)
        {
            case GrowType.TypeA:
                return Experience.Instance.GetATypeEXP(_currentLevel);
            case GrowType.TypeB:
                return Experience.Instance.GetBTypeEXP(_currentLevel);
            case GrowType.TypeC:
                return Experience.Instance.GetCTypeEXP(_currentLevel);
        }

        return 0;
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
