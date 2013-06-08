using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;

public class EquationTool
{
    /// <summary>
    /// �˺�����
    /// </summary>
    /// <param name="_from"></param>
    /// <param name="_target"></param>
    /// <returns></returns>
    public static int CalculateDamage(CardData _from, CardData _target, SkillData _skill, ref bool _double)
    {
        // �����˺�
        int damage = Mathf.CeilToInt((_from.Atk * 4 - _target.Def * 2) * _skill.MultiplyDamage) + _skill.FixedDamage;

        //UnityEngine.Debug.Log(string.Format("(Atk[{0:D}] * 4 - Def[{1:D}] * 2) * Skill[{2:S}] = {3:D}", _from.Atk, _target.Def, _skill.MultiplyDamage.ToString("f2"), damage));

        // ���Կ���, 2���˺�
        if (_from.Element == _target.BeElement)
        {
            damage = damage * 2;
            _double = true;
        }
        else
        {
            _double = false;
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
    /// ����MD5
    /// </summary>
    /// <param name="sDataIn"></param>
    /// <returns></returns>
    public static string GetMD5(string sDataIn)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] bytValue, bytHash;
        bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
        bytHash = md5.ComputeHash(bytValue);
        md5.Clear();
        string sTemp = "";
        for (int i = 0; i < bytHash.Length; i++)
        {
            sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
        }
        return sTemp.ToLower();
    }

    /// <summary>
    /// ���ݼ���
    /// </summary>
    /// <param name="data">��Ҫ���ܵ��ַ���</param>
    /// <returns></returns>
    public static string Encode(string data)
    {
        byte[] byKey = System.Text.Encoding.Default.GetBytes(STATIC_KEY_64);
        byte[] byIV = System.Text.Encoding.Default.GetBytes(STATIC_IV_64);

        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        MemoryStream ms = new MemoryStream();
        CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

        StreamWriter sw = new StreamWriter(cst);
        sw.Write(data);
        sw.Flush();
        cst.FlushFinalBlock();
        sw.Flush();
        return System.Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
    }

    /// <summary>
    /// ���ݽ���
    /// </summary>
    /// <param name="data">��Ҫ���ܵ��ַ���</param>
    /// <returns></returns>
    public static string Decode(string data)
    {
        byte[] byKey = System.Text.Encoding.Default.GetBytes(STATIC_KEY_64);
        byte[] byIV = System.Text.Encoding.Default.GetBytes(STATIC_IV_64);

        byte[] byEnc;
        try
        {
            byEnc = System.Convert.FromBase64String(data);
        }
        catch
        {
            return "";
        }

        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        MemoryStream ms = new MemoryStream(byEnc);
        CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(cst);
        return sr.ReadToEnd();
    }

    /// <summary>
    /// ��Կkey
    /// </summary>
    private static string STATIC_KEY_64
    {
        get
        {
            return "su(7ff./";
        }
    }

    /// <summary>
    /// ��Կiv
    /// </summary>
    private static string STATIC_IV_64
    {
        get
        {
            return "95&#en8~";
        }
    }

    /// <summary>
    /// ��ʱ�ļ�����λ��
    /// </summary>
    public static string TemporaryFilePath
    {
        get
        {
            string path = "";
#if UNITY_IPHONE
            string fileNameBase = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
            path = fileNameBase.Substring(0, fileNameBase.LastIndexOf('/')) + "/Documents";
#else
            path = Application.persistentDataPath;
#endif

            return path;
        }
    }
}
