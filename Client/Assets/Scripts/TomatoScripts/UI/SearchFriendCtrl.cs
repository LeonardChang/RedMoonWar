using UnityEngine;
using System.Collections;

public class SearchFriendCtrl : MonoBehaviour {
	public UILabel nameValue;
	public UILabel levelValue;
	public int charId;
	
	public void AddFriend()
	{
		ServerFuction.RequestFriend(charId.ToString());
	}
	
}
