using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginCtrl 
{
    public static void GameLogin(string name,string password)
    {
        PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 1);
        pParam.AddPair("name", name);
        pParam.AddPair("password", password);
        NetworkCtrl.Post(pParam, LoginHandler);
    }

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
}
