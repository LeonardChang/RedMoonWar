using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class ServerStringTable
{

    private static volatile ServerStringTable instance;
    private static object syncRoot = new System.Object();

    public static ServerStringTable Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new ServerStringTable();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<string, string> mTable = new Dictionary<string, string>();

    public void Initialize(string _xml)
    {
        mTable.Clear();

        try
        {
            XmlDocument doc = new XmlDocument();

            byte[] encodedString = System.Text.Encoding.UTF8.GetBytes(_xml);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;
            doc.Load(ms);

            XmlNode root = doc.SelectSingleNode("localization");
            foreach (XmlElement node in root.SelectNodes("string"))
            {
                string key = node.GetAttribute("key");
                string value = node.GetAttribute("value");
                mTable[key] = value;
            }

            ms.Close();
        }
        catch (System.Xml.XmlException ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    /// <summary>
    /// 获取一个字符串
    /// 获取字符串前必须保证CacheLoader已检测过数据
    /// </summary>
    /// <param name="_key"></param>
    /// <returns></returns>
    public string GetString(string _key)
    {
        return mTable.ContainsKey(_key) ? mTable[_key] : "";
    }
}
