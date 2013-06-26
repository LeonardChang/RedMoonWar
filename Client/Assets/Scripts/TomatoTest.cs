using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class TomatoTest : MonoBehaviour {

    public TestData data;

	// Use this for initialization
	void Start () {
		ServerFuction.GameLogin("test2","test");
		//ServerFuction.GetSysTime();
		//ServerFuction.TestPost();
		//ServerFuction.GetSysTime();
		
        //data = new TestData();
        //this.GetType().InvokeMember("data", BindingFlags.SetField, null, this, new object[] { 3,"33" }, null, null, null);
        //System.Reflection.FieldInfo[] fi = data.GetType().GetFields();
        //for (int i = 0; i < fi.Length;i++ )
       // {
        //    fi[i].SetValue(fi.GetValue(i), "2");
       // }

        //Debug.Log(data.testInt);
        //Debug.Log(data.testSring);
        
	}
	
	// Update is called once per frame
	void Update () {
        //if(Input.GetKeyDown(KeyCode.T))
		//{
		//	GameUIManager.g_gameUIManager.FriendListAppear();
		//}
		if(Input.GetKeyDown(KeyCode.S))
		{
			GameUIManager.g_gameUIManager.SearchFriendAppear();
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			GameUIManager.g_gameUIManager.RequestFriendListAppear();
		}
		if(Input.GetKeyDown(KeyCode.D))
		{
			GameUIManager.g_gameUIManager.FriendListAppear();
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
			GameUIManager.g_gameUIManager.BigStageAppear();
		}
	}
}

public class TestData : MonoBehaviour 
{
    public string testInt = "1";
    public string testSring = "1";
}
