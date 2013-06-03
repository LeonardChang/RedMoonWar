using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AttackAnimation
/// </summary>
public class AttackAnimationData
{
    private AttackAnimType mID = 0; // 索引ID
    private string mName = ""; // 名称
    private string mDescription = ""; // 描述
    private string mTAnimationName = ""; // T动画名
    private float mDelay = 0;   // 延迟时间
    // 
    private string mFlyPerfab = ""; // 飞行特效
    private string mFlySound = ""; // 飞行声音
    private string mHitTAnimation = ""; // 击中T动画

    public AttackAnimationData(string _init)
    {
        Initialize(_init);
    }

    /// <summary>
    /// ID
    /// </summary>
    public AttackAnimType ID
    {
        get
        {
            return mID;
        }
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name
    {
        get
        {
            return mName;
        }
    }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description
    {
        get
        {
            return mDescription;
        }
    }

    /// <summary>
    /// T动画名
    /// </summary>
    public string TAnimationName
    {
        get
        {
            return mTAnimationName;
        }
    }

    /// <summary>
    /// 延迟
    /// </summary>
    public float Delay
    {
        get
        {
            return mDelay;
        }
    }

    public string FlyPerfab
    {
        get
        {
            return mFlyPerfab;
        }
    }

    public string FlySound
    {
        get
        {
            return mFlySound;
        }
    }

    public string HitTAnimation
    {
        get
        {
            return mHitTAnimation;
        }
    }

    void Initialize(string _str)
    {
        string[] list = _str.Split('\t');
        if (list.Length < 8)
        {
            Debug.LogError("Error AttackAnimation data: " + _str);
            return;
        }

        try
        {
            string trimstr = " \t\r\n\f";

            mID = (AttackAnimType)int.Parse(list[0]);
            mName = list[1].Trim(trimstr.ToCharArray());
            mDescription = list[2].Trim(trimstr.ToCharArray());
            mTAnimationName = list[3].Trim(trimstr.ToCharArray());
            mDelay = float.Parse(list[4]);
            mFlyPerfab = list[5].Trim(trimstr.ToCharArray());
            mFlySound = list[6].Trim(trimstr.ToCharArray());
            mHitTAnimation = list[7].Trim(trimstr.ToCharArray());
        }
        catch (System.FormatException ex)
        {
            Debug.LogError("Error AttackAnimation data: " + ex.Message);
        }
    }
}

/// <summary>
/// AttackAnimation管理器
/// </summary>
public class AttackAnimationManager
{
    private static volatile AttackAnimationManager instance;
    private static object syncRoot = new System.Object();

    public static AttackAnimationManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new AttackAnimationManager();
                    }
                }
            }
            return instance;
        }
    }

    private Dictionary<AttackAnimType, AttackAnimationData> mAttackAnimations = new Dictionary<AttackAnimType, AttackAnimationData>();

    public AttackAnimationManager()
    {
        mAttackAnimations.Clear();

        TextAsset text = Resources.Load("Datas/AttackAnimation", typeof(TextAsset)) as TextAsset;
        string[] line = text.text.Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            if (i == 0 || line[i] == "\n" || string.IsNullOrEmpty(line[i]))
            {
                continue;
            }

            AttackAnimationData data = new AttackAnimationData(line[i]);
            mAttackAnimations[data.ID] = data;
        }
    }

    public AttackAnimationData GetAttackAnimation(AttackAnimType _index)
    {
        if (!mAttackAnimations.ContainsKey(_index))
        {
            return null;
        }

        return mAttackAnimations[_index];
    }
}