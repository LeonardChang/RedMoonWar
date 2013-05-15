using UnityEngine;
using System.Collections;

public class LoginCtrl 
{
    public static void GameLogin(string name,string password)
    {
        PostParam pParam = new PostParam();
        pParam.AddPair("cmd", 1);
        pParam.AddPair("name", name);
        pParam.AddPair("password", password);
        NetworkCtrl.Post(pParam, LoginHandler);
    }

    public static void LoginHandler(Response resp)
    {
        Debug.Log("LoginHandler");  
    }
}