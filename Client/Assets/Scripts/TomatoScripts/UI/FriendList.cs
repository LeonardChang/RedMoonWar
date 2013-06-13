using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FriendList:GameUI{
	public GameObject FriendDet;
	public GameObject FriendTable;
	public List<GameObject> friendList = new List<GameObject>();
	
	
	public override void Appear()
	{
		base.Appear();	
	}
	
	public override void Init ()
	{
		base.Init ();
		//int friendCount = Friends.Instance.MyFriends.Count;
		int friendCount = Friends.Instance.FriendCount;
		for(int i = 0; i< friendCount; i++)
		{
			GameObject friend = (GameObject)Instantiate(FriendDet);
			FriendListData friendListData = ServerDatas.friendDatas[Friends.Instance.MyFriends[i]];
			PlayerFeedBack player = ServerDatas.playerDatas[Friends.Instance.MyFriends[i]];
			int id = friendListData.id;
			friend.name = "Friend" + Friends.Instance.MyFriends[i].ToString();
			friend.transform.parent = FriendTable.transform;
			friend.transform.localPosition = Vector3.zero;
			friend.transform.localScale = new Vector3(1,1,1);
			FriendListCtrl ctrl = friend.GetComponent<FriendListCtrl>();
			ctrl.mName.text = player.name;
			ctrl.mLevel.text = player.level.ToString();
			ctrl.message.target = GameUIManager.g_gameUIManager.gameObject;
			friendList.Add(friend);
		}
		
		UITable table = FriendTable.GetComponent<UITable>();
		table.repositionNow = true;
	}
	
	public override void DisAppear ()
	{
		base.DisAppear ();
		for(int i = 0; i< friendList.Count; i++)
		{
			Destroy(friendList[i]);
		}
		
		friendList.Clear();
	}
}
