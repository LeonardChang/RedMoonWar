using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour {
	public FriendList friend;
	public FriendMenu friendMenu;
	public SearchFriend searchFriend;
	public RequestFriendList requestFriendList;
	public MessageBox messageBox;
	public Anniuncement anniuncement;
	
	public static GameUIManager g_gameUIManager;
	
	// Use this for initialization
	void Start () {
		ServerFuction.OnSearchFriendListReceive += OnSearchInfo;
		ServerFuction.OnGetRequestFriendList += OnRequestFriendList;
		ServerFuction.OnGetFriendList += OnGetFriendList;
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
		ServerFuction.GetFriendList();
		friendMenu.DisAppear();
		friend.Appear();
		//friend.Init();
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
	
	public void RequestFriendReset()
	{
		requestFriendList.DisAppear();
		ServerFuction.GetFriendRequestList();
		requestFriendList.Appear();
	}
	
	public void FriendListReset()
	{
		friend.DisAppear();
		ServerFuction.GetFriendList();
		friend.Appear();
	}
	
	/// <summary>
	/// 搜索好友
	/// </summary>
	/// <param name='name'>
	/// Name.
	/// </param>
	public void SearchFriend()
	{
		searchFriend.SearchPlayer();
	}
	
	public void OnSearchInfo(SearchFriendIdsFeedBack ids,List<PlayerFeedBack> players,List<CardFeedBack> cards)
	{
		searchFriend.SearchInfo(ids,players,cards);
	}
	
	public void OnRequestFriendList()
	{
		requestFriendList.Init();
	}
	
	public void OnGetFriendList()
	{
		friend.Init();
	}
	
	
	public void ReplyAddFriend(GameObject gameObj)
	{
		string name = gameObj.transform.parent.parent.name;
		string id = name.Replace("Request","");
		MessageBoxAppear(eMessageType.eAddFriend,gameObject,id,"Reply Friend","What are your choose","Agree","Refuse","AgreeFriend","RefuseFriend");
	}
	
	public void DeleteFriend(GameObject gameObj)
	{
		string name = gameObj.transform.parent.parent.name;
		string id = name.Replace("Friend","");
		MessageBoxAppear(eMessageType.eDeleteFriend,gameObject,id,"Detele Friend","What are your choose","Delete","Cancel","DeleteFriend","CancelDeleteFriend");
	}
	
	public void MessageBoxAppear(eMessageType mType, GameObject target,object obj, string title,string message,string leftButton,string RightButton,string LeftFunc,string RightFunc)
	{
		messageBox.sTitle = title;
		messageBox.sMessage = message;
		messageBox.sLeftButton = leftButton;
		messageBox.sRightButton = RightButton;
		messageBox.sLeftFunc = LeftFunc;
		messageBox.sRightFunc = RightFunc;
		messageBox.target = target;
		messageBox.obj = obj;
		messageBox.mType = mType;
		messageBox.Appear();
		messageBox.Init();
	}
	
	public void AgreeFriend(object id)
	{
		Debug.Log("agree");
		Debug.Log(id);
		ServerFuction.AgreeFriend(id.ToString());
		//Debug.Log(obj.name);
	}
	
	public void RefuseFriend(object id)
	{
		Debug.Log("refuse");
		Debug.Log(id);
		ServerFuction.RefuseFriend(id.ToString());
	}
	
	public void DeleteFriend(object id)
	{
		Debug.Log("delete");
		ServerFuction.DeleteFriend(id.ToString());
	}
	
	public void CancelDeleteFriend(object id)
	{
		Debug.Log("cancel");
	}
	
	public void AddFriendToFriendMenu()
	{
		requestFriendList.DisAppear();
		friendMenu.Appear();
	}
	
	public void SearchFriendToFriendMunu()
	{
		searchFriend.DisAppear();
		friendMenu.Appear();
	}
	
	public void FriendListToFriendMenu()
	{
		friend.DisAppear();
		friendMenu.Appear();
	}
	
	public void AnniuncementAppear()
	{
		anniuncement.Appear();
		
	}
	
	
}
