using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerFuction : MonoBehaviour {
	
	public static string charId;
	/// <summary>
	/// 登陆
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	/// <param name='pw'>
	/// Pw.
	/// </param>
	public static void GameLogin(string name,string pw)
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 1);
        pParam.AddPair("name", name);
        pParam.AddPair("password", pw);
        NetworkCtrl.Post(pParam, LoginHandler);
	}
	/// <summary>
	/// 登陆回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void LoginHandler(Response resp)
    {
        Debug.Log("LoginHandler");
		List<string> datas = ServerDatas.DataCheck(resp.value);
		foreach(string data in datas)
		{
			int msg = ServerDatas.GetMsg(data);
			switch((MsgMap.MSGENUM)msg)
			{
			case MsgMap.MSGENUM.MSG_ACC:		
				AccountFeedBack acc = JsonUtil.DeserializeObject<AccountFeedBack>(data);
				charId = acc.character;
				ServerFuction.GetFriendList();
				Debug.Log("MSG_ACC--->" + data);
				break;
			case MsgMap.MSGENUM.MSG_PLAYER:
				PlayerFeedBack playerFeedBack = JsonUtil.DeserializeObject<PlayerFeedBack>(data);
				Debug.Log("MSG_PLAYER--->" + data);
				break;
			case MsgMap.MSGENUM.MSG_CARD:
				CardFeedBack cardFeedBack = JsonUtil.DeserializeObject<CardFeedBack>(data);
				Debug.Log("MSG_CARD--->" + data);
				break;
			case MsgMap.MSGENUM.MSG_STATUS:
				StatusFeedBack statusFeedBack = JsonUtil.DeserializeObject<StatusFeedBack>(data);
				Debug.Log("MSG_STATUS--->" + data);
				break;
			case MsgMap.MSGENUM.MSG_COIN:
				CoinFeedBack coinFeedBack = JsonUtil.DeserializeObject<CoinFeedBack>(data);
				Debug.Log("MSG_COIN--->" + data);
				break;
			case MsgMap.MSGENUM.MSG_BAG:
				BagFeedBack bagFeedBack = JsonUtil.DeserializeObject<BagFeedBack>(data);
				Debug.Log("MSG_BAG--->" + data);
				break;
			case MsgMap.MSGENUM.MSG_TIME:
				TimeFeedBack timeFeedBack = JsonUtil.DeserializeObject<TimeFeedBack>(data);
				Debug.Log("MSG_TIME--->" + data);
				break;
			}
		}
    }
	
	/// <summary>
	/// 获得系统时间
	/// </summary>
	public static void GetSysTime()
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 4);
        NetworkCtrl.Post(pParam, LoginHandler);
	}
	
	/// <summary>
	/// 获得系统时间回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void GetSysTimeHandler(Response resp)
	{
		Debug.Log("GetSysTimeHandler");
		List<string> datas = ServerDatas.DataCheck(resp.value);
		foreach(string data in datas)
		{
			int msg = ServerDatas.GetMsg(data);
			switch((MsgMap.MSGENUM)msg)
			{
			case MsgMap.MSGENUM.MSG_TIME:
				TimeFeedBack timeFeedBack = JsonUtil.DeserializeObject<TimeFeedBack>(data);
				Debug.Log("MSG_TIME--->" + data);
				break;
			}
		}
	}
	
	/// <summary>
	/// 获得卡片列表
	/// </summary>
	public static void GetCardList()
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 5);
        pParam.AddPair("char", charId);
        NetworkCtrl.Post(pParam, GetCardListHandler);
	}
	
	/// <summary>
	/// 获得卡片列表回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void GetCardListHandler(Response resp)
	{
		Debug.Log("GetCardListHandler");
		List<string> datas = ServerDatas.DataCheck(resp.value);
		foreach(string data in datas)
		{
			int msg = ServerDatas.GetMsg(data);
			switch((MsgMap.MSGENUM)msg)
			{
			case MsgMap.MSGENUM.MSG_CARD:
				CardFeedBack cardFeedBack = JsonUtil.DeserializeObject<CardFeedBack>(data);
				Debug.Log("MSG_CARD--->" + data);
				break;
			}
		}
	}
	
	public static void SetTeamCard()
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 6);
		pParam.AddPair("char", charId);
        pParam.AddPair("pos1", 1);
		pParam.AddPair("pos1", 2);
		pParam.AddPair("pos1", 3);
		pParam.AddPair("pos1", 4);
		pParam.AddPair("pos1", 5);
        NetworkCtrl.Post(pParam, GetCardListHandler);
	}
	
	public static void SetTeamCardHandler(Response resp)
	{
		Debug.Log("SetTeamCardHandler");
	}
	
	public static void SellCards(List<string> ids)
	{
		string cards = string.Empty;
		foreach(string id in ids)
		{
			cards = cards + id;
			cards += ",";
		}
		
		cards.Substring(0,cards.Length-1);
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 7);
		pParam.AddPair("char", charId);
        pParam.AddPair("cards", cards);
        NetworkCtrl.Post(pParam, SellCardsHandler);	
	}
	
	public static void SellCardsHandler(Response resp)
	{
		Debug.Log("SellCardsHandler");
	}
	
	public static void GetFriendList()
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 9);
		pParam.AddPair("char", charId);
		NetworkCtrl.Post(pParam, GetFriendListHandler);	
	}
	
	public static void GetFriendListHandler(Response resp)
	{
		Debug.Log("GetFriendListHandler");
	}
	
	public static void GetFriendRequestList()
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 10);
		pParam.AddPair("char", charId);
		NetworkCtrl.Post(pParam, GetFriendRequestListHandler);	
	}
	
	public static void GetFriendRequestListHandler(Response resp)
	{
		Debug.Log("GetFriendRequestListHandler");
	}
	
	public static void RequestFriend(string id)
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 11);
		pParam.AddPair("char", charId);
		pParam.AddPair("friend", id);
		pParam.AddPair("content", "fuck you");
		NetworkCtrl.Post(pParam, RequestFriendHandler);	
	}
	
	public static void RequestFriendHandler(Response resp)
	{
		Debug.Log("RequestFriendHandler");
	}
	
	public static void AgreeFriend(string reqId)
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 12);
		pParam.AddPair("char", charId);
		pParam.AddPair("req", reqId);
		NetworkCtrl.Post(pParam, AgreeFriendHandler);	
	}
	
	public static void AgreeFriendHandler(Response resp)
	{
		Debug.Log("AgreeFriendHandler");
	}
	
	public static void RefuseFriend(string reqId)
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 13);
		pParam.AddPair("char", charId);
		pParam.AddPair("req", reqId);
		NetworkCtrl.Post(pParam, RefuseFriendHandler);	
	}
	
	public static void RefuseFriendHandler(Response resp)
	{
		Debug.Log("RefuseFriendHandler");
	}
	
	public static void DeleteFriend(string friendId)
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 14);
		pParam.AddPair("char", charId);
		pParam.AddPair("friend", friendId);
		NetworkCtrl.Post(pParam, DeleteFriendHandler);	
	}
	
	public static void DeleteFriendHandler(Response resp)
	{
		Debug.Log("DeleteFriendHandler");
	}
	
	public static void StoryStart(string storyId)
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 15);
		pParam.AddPair("char", charId);
		pParam.AddPair("story", storyId);
		NetworkCtrl.Post(pParam, StoryStartHandler);	
	}
	
	public static void StoryStartHandler(Response resp)
	{
		Debug.Log("StoryStartHandler");
	}
	
	public static void StoryFinish()
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 16);
		pParam.AddPair("char", charId);
		NetworkCtrl.Post(pParam, StoryFinishHandler);	
	}
	
	public static void StoryFinishHandler(Response resp)
	{
		Debug.Log("StoryFinishHandler");
	}
	
	
	
}
