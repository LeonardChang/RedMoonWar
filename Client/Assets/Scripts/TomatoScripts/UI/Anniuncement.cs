using UnityEngine;
using System.Collections;

public class Anniuncement : GameUI {
	public UILabel label;
	
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
	}
	
	public void SetInfo(string info)
	{
		label.text = info;
	}
}
