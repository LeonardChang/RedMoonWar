using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour {
	public FriendList friend;
	public FriendMenu friendMenu;
	public SearchFriend searchFriend;
	public RequestFriendList requestFriendList;
	public MessageBox messageBox;
	
	public static GameUIManager g_gameUIManager;
	
	// Use this for initialization
	void Start () {
		ServerFuction.OnSearchFriendListReceive += OnSearchInfo;
		ServerFuction.OnGetRequestFriendList += OnRequestFriendList;
		g_gameUIManager = this;
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
		ServerFuction.GetFriendRequestList();
		friendMenu.DisAppear();
		requestFriendList.Appear();
		//requestFriendList.Init();
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
	
	public void OnRequestFriendList()
	{
		requestFriendList.Init();
	}
	
	public void AddFriend(GameObject obj)
	{
		string name = obj.transform.parent.parent.name;
		string id = name.Replace("request","");
		ServerFuction.AgreeFriend(id);
	}
	
	public void MessageBoxAppear(eMessageType mType, GameObject target,GameObject sWitch, string title,string message,string leftButton,string RightButton,string LeftFunc,string RightFunc)
	{
		messageBox.sTitle = title;
		messageBox.sMessage = message;
		messageBox.sLeftButton = leftButton;
		messageBox.sRightButton = RightButton;
		messageBox.sLeftFunc = LeftFunc;
		messageBox.sRightFunc = RightFunc;
		messageBox.target = target;
		messageBox.sWitch = sWitch;
		messageBox.mType = mType;
		messageBox.Appear();
		messageBox.Init();
	}
	
	public void AgreeFriend(GameObject obj)
	{
		Debug.Log("agree");
		Debug.Log(obj.name);
	}
	
	
}
