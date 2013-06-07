using UnityEngine;
using System.Collections;
using System;

public class XiaoZhiMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ServerDatas.TimeTick = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void CheckEnegy()
	{
		DateTime nowTime = DateTime.Now;
		ServerDatas.unixTime = (long)Math.Round((nowTime - ServerDatas.TimeTick).TotalMilliseconds, MidpointRounding.AwayFromZero);
	}
}
