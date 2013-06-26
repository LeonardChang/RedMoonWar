using UnityEngine;
using System.Collections;

public class BigStage : MonoBehaviour {
	public int bigStageID;
	public bool beClear;
	public bool beOpen;
	
	public UISprite tag;
	public UISprite bg;
	public UILabel label;
	
	public void Init()
	{
		if(beClear)
		{
			tag.spriteName = "clear";
		}
		else
		{
			tag.spriteName = "new";
		}
		
		if(beOpen)
		{
			bg.spriteName = "StageOpen";
		}
		else
		{
			bg.spriteName = "StageClose";
		}
		
		label.text = Battles.Instance.StoryList[bigStageID].Name;
	}
}
