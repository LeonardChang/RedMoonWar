using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour {
	public FriendList friend;
	public FriendMenu friendMenu;
	public SearchFriend searchFriend;
	public RequestFriendList requestFriendList;
	
	// Use this for initialization
	void Start () {
		ServerFuction.OnSearchFriendListReceive += OnSearchInfo;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// 好友列表出现
	/// </summary>
	public void FriendListAppear()
	{
		friendMenu.DisAppear();
		friend.Appear();
		friend.Init();
	}
	
	/// <summary>
	/// 好友主界面出现
	/// </summary>
	public void FriendMainMenuAppear()
	{
		friendMenu.Appear();
		friendMenu.Init();
	}
	
	/// <summary>
	/// 搜索主界面出现
	/// </summary>
	public void SearchFriendAppear()
	{
		friendMenu.DisAppear();
		searchFriend.Appear();
		searchFriend.Init();
	}
	
	public void RequestFriendListAppear()
	{
		
	}
	
	/// <summary>
	/// 搜索好友
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	public void SearchFriend()
	{
		ServerFuction.SearchFriend("tester002");
	}
	
	public void OnSearchInfo(SearchFriendIdsFeedBack ids,List<PlayerFeedBack> players,List<CardFeedBack> cards)
	{
		searchFriend.SearchInfo(ids,players,cards);
	}
	
	
}
