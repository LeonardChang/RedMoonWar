using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eMessageType
{
	None,
	eAddFriend,
	eDeleteFriend,
}
public class MessageBox:GameUI{
	
	public UILabel title;
	public UILabel message;
	public UILabel leftButton;
	public UILabel rightButton;
	
	public string sTitle;
	public string sMessage;
	public string sLeftButton;
	public string sRightButton;
	public string sLeftFunc;
	public string sRightFunc;
	
	public GameObject target;
	//public GameObject sWitch;
	public object obj;
	
	
    public eMessageType mType;
	
	public override void Appear ()
	{
		base.Appear ();
	}
	
	public override void DisAppear ()
	{
		base.DisAppear ();
	}
	
	public override void Init()
	{
		title.text = sTitle;
		message.text = sMessage;
		leftButton.text = sLeftButton;
		rightButton.text = sRightButton;
		gameObject.SetActive(true);
	}
	
	public void Left()
	{
		if(target != null)
		{
			switch(mType)
			{
			case eMessageType.eAddFriend:
				break;
			case eMessageType.eDeleteFriend:
				break;
				
			}
			target.SendMessage(sLeftFunc,obj,SendMessageOptions.DontRequireReceiver);
		}
		
		gameObject.SetActive(false);
	}
	
	public void Right()
	{
		if(target != null)
		{
			target.SendMessage(sRightFunc,obj,SendMessageOptions.DontRequireReceiver);
		}
		
		gameObject.SetActive(false);
	}
}
