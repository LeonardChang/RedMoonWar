using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void ResponseHandler(Response feedback);
public delegate void NetworkHandler();

/// <summary>
/// 短连接控制类
/// </summary>
public class NetworkCtrl
{
    public static bool m_UseNetwork;                                        // 是否使用网络
    public static float m_TimeoutSecond = 10.0f;                            // 延迟超时时间（默认10秒）
    public static int m_CheckPerFrame = 3;                                  // 每帧最多检测的请求数。默认检测队列中的前3个请求的返回状态，检测到返回成功的请求时则调用处理函数。

    public static NetworkHandler TimeoutExHandler;                          // 超时的额外处理函数
    public static float m_WaitedTime;                                       // 已经等待了的时间（用于超时判定）

    private static List<PostEvent> m_EventList = new List<PostEvent>();     // 已经发送了但未返回结果的请求列表
    private static PostEvent m_CurrentEvent;                                // 当前的请求事件
    private static Response m_CurrentResponse;                              // 当前的返回数据（已分离出头以及数据的一个结构体）

    /// <summary>
    /// 初始化以及清理函数
    /// </summary>
    public static void Init()
    {
        m_EventList = new List<PostEvent>();
        m_CurrentEvent = null;
        m_CurrentResponse = null;
        m_TimeoutSecond = 10.0f;
        m_WaitedTime = 0;
        m_UseNetwork = true;
    }

    /// <summary>
    /// 更新处理器
    /// </summary>
    public static void UpdateHandler()
    {
        if (!m_UseNetwork) return;
        if (m_EventList == null || m_EventList.Count == 0)
            return;

        // 检测是否有反馈数据
        ResponseCheck();
        // 处理
        Handle();
    }

    /// <summary>
    /// 请求事件
    /// </summary>
    public static void Post(PostParam param , ResponseHandler handle)
    {
        PostEvent temp = new PostEvent(param, handle);
        m_EventList.Add(temp);
    }

    /// <summary>
    /// 请求事件
    /// </summary>
    public static void Post(PostParam param, ResponseHandler handle , bool isWithId)
    {
        PostEvent temp = new PostEvent(param, handle , isWithId);
        m_EventList.Add(temp);
    }


    /// <summary>
    /// 返回结果检测
    /// </summary>
    static void ResponseCheck()
    {
        m_CurrentEvent = null;
        int tempCount = m_EventList.Count;
        if (tempCount > 0)
        {
            m_WaitedTime += Time.deltaTime;
            for (int i = 0; i < tempCount; i++)
            {
                PostEvent temp = (PostEvent)m_EventList[i];
                if (temp.m_UrlPost.error != null)
                {
                    ClearNetwork();
                    return;
                }

                if (temp.m_UrlPost.isDone)
                {
                    Response resp = PackageModel.GetResponse(temp.m_UrlPost.text);
                    temp.m_UrlPost = null;
                    m_CurrentResponse = resp;
                    m_EventList.RemoveAt(i);
                    m_CurrentEvent = temp;
                    m_WaitedTime = 0; 
                    break;
                }
                if (i > m_CheckPerFrame)
                    break;
            }
            if (m_WaitedTime > m_TimeoutSecond)
            {
                TimeoutHandler();
            }
        }
        else 
        {
            m_WaitedTime = 0;
        }
    }

    /// <summary>
    /// 超时处理
    /// </summary>
    static void TimeoutHandler()
    {
        if (TimeoutExHandler != null)
            TimeoutExHandler();
        ClearNetwork();
    }

    /// <summary>
    /// 清理网络
    /// </summary>
    static void ClearNetwork()
    {
        m_UseNetwork = false;
        m_EventList.Clear();
        m_CurrentEvent = null;
        m_CurrentResponse = null;
        m_WaitedTime = 0;
    }

    /// <summary>
    /// 请求的分发以及处理
    /// </summary>
    /// <returns></returns>
    public static bool Handle()
    {
        if (m_CurrentEvent == null) return false;
        if (m_CurrentEvent.m_Handler == null) return false;
        m_CurrentEvent.m_Handler(m_CurrentResponse);
        return true;
    }
}
