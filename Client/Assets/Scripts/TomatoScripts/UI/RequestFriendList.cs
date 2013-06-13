using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RequestFriendList : GameUI {
	public GameObject FriendDet;
	public GameObject FriendTable;
	public List<GameObject> reList = new List<GameObject>();
	
	void Start()
	{
		
	}
	
	public override void Appear ()
	{
		base.Appear ();
	}
	
	public override void Init ()
	{
		base.Init ();
		int friendCount = Friends.Instance.StrangerCount;
		for(int i = 0; i< friendCount; i++)
		{
			GameObject friend = (GameObject)Instantiate(FriendDet);
			GameObject request = friend.transform.GetChild(0).gameObject;
			request.transform.parent = FriendTable.transform;
			request.transform.localPosition = Vector3.zero;
			request.transform.localScale = new Vector3(1,1,1);
			RequestFriendListCtrl ctrl = request.GetComponent<RequestFriendListCtrl>();
			int id = Friends.Instance.Strangers[i];
			PlayerFeedBack player = ServerDatas.playerDatas[id];
			ctrl.mName.text = player.name;
			ctrl.mLevel.text = player.level.ToString();
			ctrl.message.target = GameUIManager.g_gameUIManager.gameObject;
			RequestData reData = ServerDatas.requestDatas[id];
			
			request.name = "Request" + reData.id.ToString();
			Destroy(friend);
			reList.Add(request);
		}
		
		UITable table = FriendTable.GetComponent<UITable>();
		table.repositionNow = true;
	}
	
	public override void DisAppear ()
	{
		base.DisAppear();
		for(int i = 0; i< reList.Count;i++)
		{
			Destroy(reList[i]);
		}
		
		reList.Clear();
	}
}
