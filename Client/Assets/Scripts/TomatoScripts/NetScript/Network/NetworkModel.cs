using UnityEngine;
using System.Collections;
using System;


#region 解析包的公共模型

/// <summary>
/// 包装模型类
/// </summary>
public class PackageModel
{
    /// <summary>
    /// 获取反馈包，分离头与数据结构
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static Response GetResponse(string text)
    {
        // 正常情况。返回头以及值数据
        try
        {
            string whiteString = WWW.UnEscapeURL(text);
            Debug.Log(whiteString);
            PkgResponse resp = JsonUtil.UnpackageHead(whiteString);

            Response result = new Response();
            result.head = resp.head;
            if (resp.value != null)
            {
                result.value = resp.value;
            }
            return result;
        }

        // 非正常数据，人工添加头部报错数据
        catch (Exception ex)
        {
            Debug.Log(ex);
            Response dummy = new Response();
            dummy.head = new Head();
            dummy.head.err_msg = "-100";
            dummy.value = WWW.UnEscapeURL(text);
            return dummy;
        }
    }
}

/// <summary>
/// 包头
/// </summary>
public class Head
{
    public string name;
    public string err_msg;
    public string version;
}

/// <summary>
/// 反馈包
/// </summary>
public struct PkgResponse
{
    public string tag;
    public string cmd;
    public string ret;
    public Head head;
    public string value;
}

/// <summary>
/// 反馈结果
/// </summary>
public class Response
{
    public Head head;
    public Texture texture;
    public string value;
}

#endregion

/// <summary>
/// 参数对拼装类
/// </summary>
public class PostParam
{
    string paramString = "";

    public PostParam()
    {
        paramString += NetBase.BaseUrl;
    }

    /// <summary>
    /// 添加参数对
    /// </summary>
    /// <param name="name">变量名</param>
    /// <param name="value">变量值</param>
    public void AddPair(string name, string value)
    {
        if (paramString.Length != NetBase.BaseUrl.Length)
        {
            paramString += "&";
        }      
        paramString += name;
        paramString += "=";
        paramString += WWW.EscapeURL(value);
    }

    public void AddPair(string name, int value)
    {
        if (paramString.Length != NetBase.BaseUrl.Length)
        {
            paramString += "&";
        }
        paramString += name;
        paramString += "=";
        paramString += WWW.EscapeURL(value.ToString());
    }

    /// <summary>
    /// 获取拼接好的字符串
    /// </summary>
    /// <returns></returns>
    public string GetParamString()
    {
        return paramString;
    }
}

/// <summary>
/// 短连接请求类
/// </summary>
public class PostEvent
{
    string m_UrlString;                                 // 请求地址
    public WWW m_UrlPost;                               // 短连接请求
    public ResponseHandler m_Handler;                   // 回调

    /// <summary>
    /// 构造函数，构造一个短连接请求
    /// </summary>
    /// <param name="url"></param>
    public PostEvent(PostParam param, ResponseHandler handler)
    {
        m_UrlString = param.GetParamString();
        Debug.Log(m_UrlString);
        NetworkUtil.Post(out m_UrlPost, m_UrlString);
        if (handler != null)
        {
            m_Handler += handler;
        }
    }

    /// <summary>
    /// 构造函数，构造一个短连接请求
    /// </summary>
    /// <param name="url"></param>
    public PostEvent(PostParam param, ResponseHandler handler, bool isWithId)
    {
        m_UrlString = param.GetParamString();
        NetworkUtil.Post(out m_UrlPost, m_UrlString);
        if (handler != null)
        {
            m_Handler += handler;
        }
    }
}


public class NetworkUtil
{
    /// <summary>
    /// 发送请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="text"></param>
    public static void Post(out WWW url, string text)
    {
        Debug.Log(text);
        text = Encryt(text);
        url = new WWW(text);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string Encryt(string content)
    {
        return content;
    }
}
