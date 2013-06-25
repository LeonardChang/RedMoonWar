using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

public class ServerDatas{
	public static string charId;
	public static int enegydate;
	public static float enegyinc;
	public static DateTime TimeTick;
	public static long unixTime;
	
	
	public static Dictionary<System.Int64,PlayerFeedBack> playerDatas = new Dictionary<long, PlayerFeedBack>();
	public static Dictionary<System.Int64,CardFeedBack> cardDatas = new Dictionary<long, CardFeedBack>();
	public static Dictionary<System.Int64,RequestData> requestDatas = new Dictionary<long, RequestData>();
	public static Dictionary<System.Int64,FriendListData> friendDatas = new Dictionary<long, FriendListData>();
	
	
	public static List<string> DataCheck(string data)
	{
		List<string> dataList = new List<string>();
		data = data.Substring(1,data.Length-2);
		List<string> list = ServerDatas.WSplit(data);
		foreach(string s in list)
		{
			Debug.Log(s);
			dataList.Add(s);
		}
		
		return dataList;
	}
	
	private static List<ServerData> DataDetil(string data)
	{
		List<ServerData> dataList = new List<ServerData>();
		string[] datas = data.Split(',');
		for(int i = 0; i< datas.Length; i++)
		{
			string[] mainData = datas[i].Split(':');
			ServerData serData = new ServerData();
			serData.mName = mainData[0];
			serData.mValue = mainData[1];
			dataList.Add(serData);
		}
		return dataList;
	}
	
	private static List<string> WSplit(string data)
	{
		List<string> list = new List<string>();
		int index = 0;
		string s = "";
		
		for(int i = 0; i< data.Length; i++)
		{
			if(data[i] == '{')
			{
				index++;
				if(index == 1)
				{
					s = string.Empty;
				}
			}
			if(data[i] == '}')
			{
				index --;
				if(index == 0)
				{
					s+= "}";
					list.Add(s);
				}
			}
			if(index>0)
			{
				s += data[i];
			}
		}
		return list;
	}
	
	public static int GetMsg(string data)
	{
		int index = data.LastIndexOf("_msg");
		string inx = data.Substring(index+6);
		inx = inx.Substring(0,inx.Length-1);
		Debug.Log(inx);
		return int.Parse(inx);
	}
}

public struct ServerData
{
	public string mName;
	public string mValue;
}

public struct AccountFeedBack
{
	public string acc;
	public string character;
	public int _msg;
}

public struct StatusFeedBack
{
	public int status;
	public int _msg;
}

public struct CoinFeedBack
{
	public int coin;
	public int _msg;
}

public struct PlayerFeedBack
{
	public string id;
	public string name;
	public int level;
	public int exp;
	public float enegy;
	public float enegyinc;
	public int enegymax;
	public int enegydate;
	public int money;
	public string country;
	public string chapter_id;
	public string route_id;
	public int gachadate;
	public int friendpt;	
	public int _msg;
	public int logindate;
}

public struct TimeFeedBack
{
	public int time;
	public int _msg;
}

public struct BagFeedBack
{
	public Dictionary<string,int> book;
	public string cardcount;
	public int _msg;
}

public struct CardFeedBack
{
	public string id;
	public List<sCard> cards;
	public string cardcount;
	public int _msg;
}

public struct sCard
{
	public int id;
	public int char_id;
	public int card_id;
	public int use;
	public int level;
	public int exp;
	public int hp;
	public int mp;
	public int spd;
	public int atk;
	public int def;	
}

public struct SearchFriendIdsFeedBack
{
	public int[] ids;
	public int _msg;
}

public struct RequestFriendFeedBack
{
	public RequestData[] request;
	public int _msg;
}

public struct RequestData
{
	public int id;
	public int friend_id;
	public int create_date;
	public string content;
}

public struct FriendListFeedBack
{
	public FriendListData[] friends;
	public int _msg;
}

public struct FriendListData
{
	public int id;
	public int friend_id;
	public string op;
	public int opdate;
}

public struct StageInfoFeedBack
{
	public int stageId;
	public int taskId;
	public int energy;
	public int _msg;
}

public struct StageFeedBack
{
	public int stage_id;
	public int width;
    public int height;
    public int scene_id;
	public BattlePlayerDataFeedBack[] players;
	public BattleEnemyDataFeedBack[] enemys;
	public int _msg;
}

public struct BattlePlayerDataFeedBack
{
	public int x;
    public int y;
}

public struct BattleEnemyDataFeedBack
{
	public int x;
    public int y;
	public int monster_id;
	public int mDropCard;
    public int mDropCoin;
}

public struct TeamFormationFeedBack
{
	public int[] team;
}



