using UnityEngine;
using System.Collections;

/// <summary>
/// 短连接请求更新节点
/// </summary>
public class NetworkGameObject : MonoBehaviour 
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
		NetworkCtrl.Init();
    }

	void Start () 
    {
        // 初始化网络系统
        

        //// 尝试自动登陆，如果本地已经有账号的话返回true
        //if (!LoginCtrl.AutoLogin())
        //{
        //    // 如果本地无账号的话,尝试注册
        //    LoginCtrl.Register();
        //}
	}
	
	void Update ()
    {
        NetworkCtrl.UpdateHandler();

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoginCtrl.GameLogin("test", "test");
        }

	}

}
