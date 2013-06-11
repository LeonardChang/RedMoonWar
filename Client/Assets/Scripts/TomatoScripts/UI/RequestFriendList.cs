using UnityEngine;
using System.Collections;

public class RequestFriendList : GameUI {
	public GameObject FriendDet;
	public GameObject FriendTable;
	
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
			request.name = "Request" + id.ToString();
			Destroy(friend);
		}
		
		UITable table = FriendTable.GetComponent<UITable>();
		table.repositionNow = true;
	}
	
	public override void DisAppear ()
	{
		base.DisAppear();
	}
}
