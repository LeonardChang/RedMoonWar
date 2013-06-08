using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJsonGT;

public class CacheLoader  : MonoBehaviour {
    private static volatile CacheLoader mInstance;
    private static object syncRoot = new System.Object();

    public static CacheLoader Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject perfab = Resources.Load("Perfabs/CacheLoader") as GameObject;
                GameObject obj = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
                mInstance = obj.GetComponent<CacheLoader>();
            }
            return mInstance;
        }
    }
    
    private static string IPAdress
    {
        get
        {
            return "http://103.6.220.151:8090";
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (mInstance != null && mInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            mInstance = this;
        }

        this.ChecnFinishEvent += CheckResult;
    }

    void Start()
    {

    }

    void Update()
    {
        if (mNeedCheckList.Count > 0)
        {
            int resule = 1;
            foreach (string key in mNeedCheckList.Keys)
            {
                if (mNeedCheckList[key] == 0)
                {
                    resule = 0;
                    break;
                }
                else if (mNeedCheckList[key] == -1)
                {
                    resule = -1;
                }
            }

            if (resule != 0)
            {
                mNeedCheckList.Clear();
                if (CheckAllCacheResult != null)
                {
                    CheckAllCacheResult(resule == 1);
                }
                print("Check all cache files finish: " + resule.ToString());
            }
        }
    }

    public System.Action<bool> CheckAllCacheResult = null;

    private Dictionary<string, int> mNeedCheckList = new Dictionary<string, int>();

    /// <summary>
    /// 检查所有字典并读取到内存中
    /// </summary>
    public void CheckAllCache()
    {
        CheckAllCache("", "", "", "", "", "");
    }

    /// <summary>
    /// 检查所有字典并读取到内存中(MD5)
    /// </summary>
    public void CheckAllCache(string strTableMD5, string cardMD5, string storyMD5, string levelMD5, string skillMD5, string gachaMD5)
    {
        print("Start check all cache files.");

        mNeedCheckList.Clear();

        string URL = IPAdress + "/?svrcmd=dbstr";
        string file = "DBStr.data";
        mNeedCheckList[file] = 0;
        CheckFile(file, strTableMD5, URL);

        URL = IPAdress + "/?cmd=3&name=card";
        file = "DBCard.data";
        mNeedCheckList[file] = 0;
        CheckFile(file, cardMD5, URL);

        URL = IPAdress + "/?cmd=3&name=story";
        file = "DBStory.data";
        mNeedCheckList[file] = 0;
        CheckFile(file, storyMD5, URL);

        URL = IPAdress + "/?cmd=3&name=level";
        file = "DBLevel.data";
        mNeedCheckList[file] = 0;
        CheckFile(file, levelMD5, URL);

        URL = IPAdress + "/?cmd=3&name=skill";
        file = "DBSkill.data";
        mNeedCheckList[file] = 0;
        CheckFile(file, skillMD5, URL);

        URL = IPAdress + "/?cmd=3&name=gacha";
        file = "DBGacha.data";
        mNeedCheckList[file] = 0;
        CheckFile(file, gachaMD5, URL);
    }

    private System.Action<string, bool, string> ChecnFinishEvent = null;
    private void CheckFile(string _file, string _md5, string _url)
    {
        string path = EquationTool.TemporaryFilePath + "/" + _file;
        print("Start check file: " + path);

        if (File.Exists(path))
        {
            StreamReader reader = new StreamReader(path, System.Text.Encoding.Default);
            string filestr = EquationTool.Decode(reader.ReadToEnd());
            reader.Close();
            string filemd5 = EquationTool.GetMD5(filestr);

            if (!string.IsNullOrEmpty(_md5) && filemd5 != _md5)
            {
                StartCoroutine(DownloadFile(_file, _url));
            }
            else
            {
                if (ChecnFinishEvent != null)
                {
                    ChecnFinishEvent(_file, true, filestr);
                }
            }
        }
        else
        {
            StartCoroutine(DownloadFile(_file, _url));
        }
    }

    private void CheckResult(string _file, bool _success, string _result)
    {
        mNeedCheckList[_file] = _success ? 1 : -1;

        if (_success)
        {
            if (_file == "DBStr.data")
            {
                ServerStringTable.Instance.Initialize(_result);
            }
            else if (_file == "DBCard.data")
            {
                PkgResponse response = JsonUtil.UnpackageHead(_result);
                sCardList data = JsonUtil.DeserializeObject<sCardList>(response.ret.Substring(1, response.ret.Length - 2));
                CardManager.Instance.ResetData(data);
            }
            else if (_file == "DBStory.data")
            {
                PkgResponse response = JsonUtil.UnpackageHead(_result);
                sStoryList data = JsonUtil.DeserializeObject<sStoryList>(response.ret.Substring(1, response.ret.Length - 2));
                Battles.Instance.ResetData(data);
            }
            else if (_file == "DBLevel.data")
            {
                PkgResponse response = JsonUtil.UnpackageHead(_result);
                sLevelList data = JsonUtil.DeserializeObject<sLevelList>(response.ret.Substring(1, response.ret.Length - 2));
                Experience.Instance.ResetPlayerData(data);
            }
            else if (_file == "DBSkill.data")
            {
                PkgResponse response = JsonUtil.UnpackageHead(_result);
                sSkillList data = JsonUtil.DeserializeObject<sSkillList>(response.ret.Substring(1, response.ret.Length - 2));
                SkillManager.Instance.ResetData(data);
            }
            else if (_file == "DBGacha.data")
            {
                PkgResponse response = JsonUtil.UnpackageHead(_result);
                sGachaList data = JsonUtil.DeserializeObject<sGachaList>(response.ret.Substring(1, response.ret.Length - 2));
                GachaManager.Instance.ResetData(data);
            }
        }
    }

    private IEnumerator DownloadFile(string _file, string _url)
    {
        WWW www = new WWW(_url);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            string str = www.text;
            string encode = EquationTool.Encode(str);

            string path = EquationTool.TemporaryFilePath + "/" + _file;
            StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.Default);
            writer.Write(encode);
            writer.Close();

            if (ChecnFinishEvent != null)
            {
                ChecnFinishEvent(_file, true, str);
            }
        }
        else
        {
            Debug.LogError(www.error);
            if (ChecnFinishEvent != null)
            {
                ChecnFinishEvent(_file, false, www.error);
            }
        }
    }
}

public class sCardList
{
    public List<sCardData> card;
    public int _msg;
}

public class sCardData
{
    public int id;
    public int rare;
    public string name;
    public string profile;
    public string img;
    public int level_growth;
    public int hp;
    public int mp;
    public int speed;
    public int attack;
    public int defence;
    public int normalskill;
    public int skill;
    public int leaderskill;
    public int kind;
    public int growth_id;
    public int collection_id;
    public int max_level;
    public int cost;
    public int sell;
    public int give_exp_base;
    public int give_exp_grow;
    public int give_cost_base;
    public int give_cost_grow;
    public int give_sell_base;
    public int give_sell_grow;
}

public class sStoryList
{
    public List<sStoryData> story;
    public int _msg;
}

public class sStoryData
{
    public int id;
    public int chapter_id;
    public int route_id;
    public int monster;
    public int cost;
    public string chapter_name;
    public string route_name;
    public string image;
    public int coin_bonus;
}

public class sLevelList
{
    public List<sLevelData> level;
    public int _msg;
}

public class sLevelData
{
    public int id;
    public int exp;
    public string enegy_inc;
    public int enegy_max;
    public int cost;
}

public class sSkillList
{
    public List<sSkillData> skill;
    public int _msg;
}

public class sSkillData
{
    public int id;
    public string name;
    public string description;
    public string atkrate;
    public int fix;
    public int range;
    public int buff;
    public int mana;
    public int hatred;
    public int maxlevel;
}

public class sGachaList
{
    public List<sGachaData> gacha;
    public int _msg;
}

public class sGachaData
{
    public int id;
    public int card_drop;
    public int cost;
}