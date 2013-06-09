using UnityEngine;
using System.Collections;

/// <summary>
/// ������������½ڵ�
/// </summary>
public class NetworkGameObject : MonoBehaviour 
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
		NetworkCtrl.Init();
		CacheLoader.Instance.CheckAllCache();
    }

	void Start () 
    {
        // ��ʼ������ϵͳ
        

        //// �����Զ���½����������Ѿ����˺ŵĻ�����true
        //if (!LoginCtrl.AutoLogin())
        //{
        //    // ����������˺ŵĻ�,����ע��
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
