using UnityEngine;
using System.Collections;

public class GameUIManager : MonoBehaviour {
	public FriendList friend;
	public FriendMenu friendMenu;
	public SearchFriend searchFriend;
	public AddFriend addFriend;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void FriendListAppear()
	{
		friendMenu.DisAppear();
		friend.Appear();
		friend.Init();
	}
	
	public void FriendMainMenuAppear()
	{
		friendMenu.Appear();
		friendMenu.Init();
	}
	
	public void SearchFriendAppear()
	{
		friendMenu.DisAppear();
		searchFriend.Appear();
		searchFriend.Init();
	}
	
	public void SearchFriend(string name)
	{
		
	}
}
