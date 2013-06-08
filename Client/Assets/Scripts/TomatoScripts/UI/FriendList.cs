using UnityEngine;
using System.Collections;

public class FriendList:GameUI{
	public GameObject FriendDet;
	public GameObject FriendTable;
	
	
	public override void Appear()
	{
		base.Appear();	
	}
	
	public override void Init ()
	{
		base.Init ();
		//int friendCount = Friends.Instance.MyFriends.Count;
		int friendCount = 5;
		for(int i = 0; i< friendCount; i++)
		{
			GameObject friend = (GameObject)Instantiate(FriendDet);
			friend.name = "friendDet" + (i+1).ToString();
			friend.transform.parent = FriendTable.transform;
			friend.transform.localPosition = Vector3.zero;
			friend.transform.localScale = new Vector3(1,1,1);
		}
		
		UITable table = FriendTable.GetComponent<UITable>();
		//table.repositionNow = true;
	}
	
	public override void DisAppear ()
	{
		base.DisAppear ();
	}
}
