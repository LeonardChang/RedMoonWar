using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchFriend : GameUI {
	public GameObject searchFriendCtrl;
	public List<GameObject> searchList;
	public UITable table;
	
	
	void Start()
	{
		searchList = new List<GameObject>();
	}

	public override void Appear()
	{
		base.Appear();
		for(int i = 0; i< searchList.Count; i++)
		{
			GameObject obj = searchList[i];
			Destroy(obj);
		}		
		searchList.Clear();
	}
	
	public override void Init ()
	{
		base.Init ();
	}
	
	public override void DisAppear()
	{
		base.DisAppear ();
	}
	
	public void SearchInfo(SearchFriendIdsFeedBack ids,List<PlayerFeedBack> players,List<CardFeedBack> cards)
	{
		int count =  ids.ids.Length;
		for(int i = 0; i< count; i++)
		{
			GameObject obj = (GameObject)Instantiate(searchFriendCtrl);
			searchList.Add(obj);
			obj.transform.parent = table.gameObject.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = new Vector3(1,1,1);
			SearchFriendCtrl ctrl = obj.GetComponent<SearchFriendCtrl>();
			ctrl.nameValue.text = players[i].name;
			ctrl.levelValue.text = players[i].level.ToString();
			ctrl.charId = ids.ids[i];
		}	
		table.repositionNow = true;
	}
}
