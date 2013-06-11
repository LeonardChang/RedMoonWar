using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerFuction{
	
	public static string mName;
	public static string mPw;
	
	
	
	/// <summary>
	/// 获得搜索好友信息
	/// </summary>
	public delegate void SearchFriendListReceiveCallBack(SearchFriendIdsFeedBack ids,List<PlayerFeedBack> players,List<CardFeedBack> cards);
    public static event SearchFriendListReceiveCallBack OnSearchFriendListReceive;
	
	public delegate void GetRequestFriendListCallBack();
    public static event GetRequestFriendListCallBack OnGetRequestFriendList;
	
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
		mName = name;
		mPw = pw;
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
				ServerDatas.charId = acc.character;
				Player.Instance.UserName = mName;
				Player.Instance.Password = mPw;
				Debug.Log("MSG_ACC--->" + data);
				break;
			case MsgMap.MSGENUM.MSG_PLAYER:
				PlayerFeedBack playerFeedBack = JsonUtil.DeserializeObject<PlayerFeedBack>(data);
				Debug.Log("MSG_PLAYER--->" + data);
				Player.Instance.PlayerID = int.Parse(playerFeedBack.id); 
				Player.Instance.NickName = playerFeedBack.name;
				Player.Instance.Level = playerFeedBack.level;
				Player.Instance.EXP = playerFeedBack.exp;
				Player.Instance.EXPMax = Experience.Instance.GetPlayerEXP(Player.Instance.Level + 1);
				Player.Instance.Energy = (int)playerFeedBack.enegy;
				Player.Instance.EnergyMax = playerFeedBack.enegymax;
				Player.Instance.Coin = playerFeedBack.money;
				Player.Instance.FriendshipPoint = playerFeedBack.friendpt;
				break;
			case MsgMap.MSGENUM.MSG_CARD:
				CardFeedBack cardFeedBack = JsonUtil.DeserializeObject<CardFeedBack>(data);
				foreach(sCard card in cardFeedBack.cards)
				{
					CharacterData chara = new CharacterData();
					chara.ID = card.id;
					chara.CardID = card.card_id;
					chara.Level = card.level;
					chara.EXP = card.exp;
					chara.Atk = card.atk;
					chara.Def = card.def;
					chara.MaxHP = card.hp;
					chara.MaxMP = card.mp;
					chara.Spd = card.spd;
					if(card.use <=0)
					{
						Formation.Instance.Team.Add(chara.ID,chara);
					}
				}
				Debug.Log("MSG_CARD--->" + data);
				break;
			case MsgMap.MSGENUM.MSG_STATUS:
				StatusFeedBack statusFeedBack = JsonUtil.DeserializeObject<StatusFeedBack>(data);
				Debug.Log("MSG_STATUS--->" + data);
				break;
			case MsgMap.MSGENUM.MSG_COIN:
				CoinFeedBack coinFeedBack = JsonUtil.DeserializeObject<CoinFeedBack>(data);
				Player.Instance.Gem = coinFeedBack.coin;
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
        pParam.AddPair("char", ServerDatas.charId);
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
	
	/// <summary>
	/// 设置队伍
	/// </summary>
	public static void SetTeamCard()
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 6);
		pParam.AddPair("char", ServerDatas.charId);
        pParam.AddPair("pos1", 1);
		pParam.AddPair("pos1", 2);
		pParam.AddPair("pos1", 3);
		pParam.AddPair("pos1", 4);
		pParam.AddPair("pos1", 5);
        NetworkCtrl.Post(pParam, GetCardListHandler);
	}
	
	/// <summary>
	/// S设置队伍回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void SetTeamCardHandler(Response resp)
	{
		Debug.Log("SetTeamCardHandler");
	}
	
	/// <summary>
	/// 出售卡片
	/// </summary>
	/// <param name='ids'>
	/// Identifiers.
	/// </param>
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
		pParam.AddPair("char", ServerDatas.charId);
        pParam.AddPair("cards", cards);
        NetworkCtrl.Post(pParam, SellCardsHandler);	
	}
	
	/// <summary>
	/// 出售卡片回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void SellCardsHandler(Response resp)
	{
		Debug.Log("SellCardsHandler");
	}
	
	/// <summary>
	/// 获得好友列表
	/// </summary>
	public static void GetFriendList()
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 9);
		pParam.AddPair("char", ServerDatas.charId);
		NetworkCtrl.Post(pParam, GetFriendListHandler);	
	}
	
	/// <summary>
	/// 获得好友列表回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void GetFriendListHandler(Response resp)
	{	
		List<string> datas = ServerDatas.DataCheck(resp.value);
		Friends.Instance.MyFriends.Clear();
		foreach(string data in datas)
		{
			int msg = ServerDatas.GetMsg(data);
			switch((MsgMap.MSGENUM)msg)
			{
			case MsgMap.MSGENUM.MSG_PLAYER:
				PlayerFeedBack playerFeedBack = JsonUtil.DeserializeObject<PlayerFeedBack>(data);
				ServerDatas.playerDatas.Add(int.Parse(playerFeedBack.id),playerFeedBack);
				Friends.Instance.MyFriends.Add(int.Parse(playerFeedBack.id));
				Debug.Log("MSG_PLAYER--->" + data);						
				break;
			}
		}
		Debug.Log("GetFriendListHandler");
	}
	
	/// <summary>
	/// 获得好友申请列表
	/// </summary>
	public static void GetFriendRequestList()
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 10);
		pParam.AddPair("char", ServerDatas.charId);
		NetworkCtrl.Post(pParam, GetFriendRequestListHandler);	
	}
	
	/// <summary>
	/// 好友申请列表回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void GetFriendRequestListHandler(Response resp)
	{
		List<string> datas = ServerDatas.DataCheck(resp.value);
		Friends.Instance.Strangers.Clear();
		foreach(string data in datas)
		{
			int msg = ServerDatas.GetMsg(data);
			switch((MsgMap.MSGENUM)msg)
			{
			case MsgMap.MSGENUM.MSG_REQUESTFRIEND:	
				RequestFriendFeedBack re = JsonUtil.DeserializeObject<RequestFriendFeedBack>(data);
				foreach(RequestData ed in re.request)
				{
					ServerDatas.requestDatas.Add(ed.friend_id,ed);
					Friends.Instance.Strangers.Add(ed.friend_id);
				}
				Debug.Log("MSG_REQUESTFRIEND--->" + data);
				break;
			case MsgMap.MSGENUM.MSG_PLAYER:
				PlayerFeedBack playerFeedBack = JsonUtil.DeserializeObject<PlayerFeedBack>(data);
				ServerDatas.playerDatas.Add(int.Parse(playerFeedBack.id),playerFeedBack);
				Debug.Log("MSG_PLAYER--->" + data);				
				break;
			}
		}
		Debug.Log("GetFriendRequestListHandler");
		if(OnGetRequestFriendList != null)
		{
			OnGetRequestFriendList();
		}
	}
	
	/// <summary>
	/// 请求加好友
	/// </summary>
	/// <param name='id'>
	/// Identifier.
	/// </param>
	public static void RequestFriend(string id)
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 11);
		pParam.AddPair("char", ServerDatas.charId);
		pParam.AddPair("friend", id);
		pParam.AddPair("content", "fuck you");
		NetworkCtrl.Post(pParam, RequestFriendHandler);	
	}
	
	/// <summary>
	/// 请求加好友回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void RequestFriendHandler(Response resp)
	{
		Debug.Log("RequestFriendHandler");
	}
	
	/// <summary>
	/// 同意好友
	/// </summary>
	/// <param name='reqId'>
	/// Req identifier.
	/// </param>
	public static void AgreeFriend(string reqId)
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 12);
		pParam.AddPair("char", ServerDatas.charId);
		pParam.AddPair("req", reqId);
		NetworkCtrl.Post(pParam, AgreeFriendHandler);	
	}
	
	/// <summary>
	/// 同意好友回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void AgreeFriendHandler(Response resp)
	{
		Debug.Log("AgreeFriendHandler");
	}
	
	/// <summary>
	/// 拒绝好友
	/// </summary>
	/// <param name='reqId'>
	/// Req identifier.
	/// </param>
	public static void RefuseFriend(string reqId)
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 13);
		pParam.AddPair("char", ServerDatas.charId);
		pParam.AddPair("req", reqId);
		NetworkCtrl.Post(pParam, RefuseFriendHandler);	
	}
	
	/// <summary>
	/// 拒绝好友回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void RefuseFriendHandler(Response resp)
	{
		Debug.Log("RefuseFriendHandler");
	}
	
	/// <summary>
	/// 删除好友
	/// </summary>
	/// <param name='friendId'>
	/// Friend identifier.
	/// </param>
	public static void DeleteFriend(string friendId)
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 14);
		pParam.AddPair("char", ServerDatas.charId);
		pParam.AddPair("friend", friendId);
		NetworkCtrl.Post(pParam, DeleteFriendHandler);	
	}
	
	/// <summary>
	/// 删除好友回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void DeleteFriendHandler(Response resp)
	{
		Debug.Log("DeleteFriendHandler");
	}
	
	
	/// <summary>
	/// 关卡开始
	/// </summary>
	/// <param name='storyId'>
	/// Story identifier.
	/// </param>
	public static void StoryStart(string storyId)
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 15);
		pParam.AddPair("char", ServerDatas.charId);
		pParam.AddPair("story", storyId);
		NetworkCtrl.Post(pParam, StoryStartHandler);	
	}
	
	/// <summary>
	/// 关卡开始回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void StoryStartHandler(Response resp)
	{
		Debug.Log("StoryStartHandler");
	}
	
	/// <summary>
	/// 关卡结束
	/// </summary>
	public static void StoryFinish()
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 16);
		pParam.AddPair("char", ServerDatas.charId);
		NetworkCtrl.Post(pParam, StoryFinishHandler);	
	}
	
	/// <summary>
	/// 关卡结束回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void StoryFinishHandler(Response resp)
	{
		Debug.Log("StoryFinishHandler");
	}
	
	/// <summary>
	/// 搜索好友
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	public static void SearchFriend(string name)
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 17);
		pParam.AddPair("char", ServerDatas.charId);
		pParam.AddPair("name", name);
		NetworkCtrl.Post(pParam, SearchFriendHandler);	
	}
	
	/// <summary>
	/// 搜索好友回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void SearchFriendHandler(Response resp)
	{
		Debug.Log("GetRanFriendHandler");
		List<PlayerFeedBack> players = new List<PlayerFeedBack>();
		List<CardFeedBack> cards = new List<CardFeedBack>();
		SearchFriendIdsFeedBack searchFriendIdsFeedBack = new SearchFriendIdsFeedBack();
		
		List<string> datas = ServerDatas.DataCheck(resp.value);
		foreach(string data in datas)
		{
			int msg = ServerDatas.GetMsg(data);
			switch((MsgMap.MSGENUM)msg)
			{
			case MsgMap.MSGENUM.MSG_PLAYER:
				PlayerFeedBack playerFeedBack = JsonUtil.DeserializeObject<PlayerFeedBack>(data);
				Debug.Log("MSG_PLAYER--->" + data);
				players.Add(playerFeedBack);
				break;
			case MsgMap.MSGENUM.MSG_CARD:
				CardFeedBack cardFeedBack = JsonUtil.DeserializeObject<CardFeedBack>(data);
				cards.Add(cardFeedBack);
				Debug.Log("MSG_CARD--->" + data);
				break;
			case MsgMap.MSGENUM.MSG_FRIENDID:
				searchFriendIdsFeedBack = JsonUtil.DeserializeObject<SearchFriendIdsFeedBack>(data);
				break;
			}
		}
		if(OnSearchFriendListReceive != null)
		{
			OnSearchFriendListReceive(searchFriendIdsFeedBack,players,cards);
		}
		Debug.Log("SearchFriendHandler");
	}
	
	/// <summary>
	/// 获取好友推荐
	/// </summary>
	public static void GetRanFriend()
	{
		PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 18);
		pParam.AddPair("char", ServerDatas.charId);
		NetworkCtrl.Post(pParam, GetRanFriendHandler);	
	}
	
	/// <summary>
	/// 获得好友推荐回调
	/// </summary>
	/// <param name='resp'>
	/// Resp.
	/// </param>
	public static void GetRanFriendHandler(Response resp)
	{
		
	}
	
	
	
}
