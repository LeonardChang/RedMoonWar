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
    }

	void Start () 
    {
        // ��ʼ������ϵͳ
        NetworkCtrl.Init();

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
