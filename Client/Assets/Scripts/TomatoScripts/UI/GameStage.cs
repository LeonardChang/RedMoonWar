using UnityEngine;
using System.Collections;

public class GameStage : GameUI {
	public UITable table;
	public UITable smallTable;
	public GameObject bStage;
	
	public override void Appear ()
	{
		base.Appear ();
	}
	
	public override void DisAppear ()
	{
		base.DisAppear ();
	}
	
	public override void Init ()
	{
		base.Init ();
		for(int i = 1; i<= StageManager.bigStageId; i++)
		{
			GameObject obj = (GameObject)Instantiate(bStage);
			obj.name = "BigStage" + i.ToString();
			obj.transform.parent = table.gameObject.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = new Vector3(1,1,1);
			UIButtonMessage mess = obj.GetComponent<UIButtonMessage>();
			mess.target = GameUIManager.g_gameUIManager.gameObject;
			BigStage sta = obj.GetComponent<BigStage>();
			sta.beOpen = true;
			if(i == StageManager.bigStageId)
			{
				sta.beClear = false;
			}
			else
			{
				sta.beClear = true;
			}
			sta.bigStageID = i;
			sta.Init();
		}
		table.repositionNow = true;
	}
	
	public void IntoSmallStage()
	{
		table.gameObject.SetActive(false);
		if(StageManager.nowBigStageId == StageManager.bigStageId)
		{
			for(int i = 1; i<= StageManager.stageId; i++)
			{
				GameObject obj = (GameObject)Instantiate(bStage);
				obj.name = "BigStage" + i.ToString();
				obj.transform.parent = table.gameObject.transform;
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localScale = new Vector3(1,1,1);
				UIButtonMessage mess = obj.GetComponent<UIButtonMessage>();
				mess.target = GameUIManager.g_gameUIManager.gameObject;
				BigStage sta = obj.GetComponent<BigStage>();
				sta.beOpen = true;
				if(i == StageManager.bigStageId)
				{
					sta.beClear = false;
				}
				else
				{
					sta.beClear = true;
				}
				sta.bigStageID = i;
				sta.Init();
			}
			table.repositionNow = true;
		}
		else
		{
			
		}
	}
	
}
