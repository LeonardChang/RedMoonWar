using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class ServerDatas{
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
	public string level;
	public string exp;
	public string enegy;
	public string enegyinc;
	public string enegymax;
	public int enegydate;
	public string money;
	public string country;
	public string chapter_id;
	public string route_id;
	public int gachadate;
	public string friendpt;	
	public int _msg;
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
	public string id;
	public string char_id;
	public string card_id;
	public string use;
	public string level;
	public string exp;
	public string hp;
	public string mp;
	public string spd;
	public string atk;
	public string def;	
}




