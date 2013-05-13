using UnityEngine;
using System.Collections;

public class AutoScaleCenter : MonoBehaviour
{
    private const int DefaultWidth = 640;
    private const float DefaultRatio = 2.0f / 3.0f;
    private const float DefaultHeight = DefaultWidth / DefaultRatio;

    private ScreenOrientation mScreenOrientation;
    private int mWidth;
    private int mHeight;

    private bool mNeedReset = true;
    public bool NeedReset
    {
        get { return mNeedReset; }
        set { mNeedReset = value; }
    }

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        NeedReset = true;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mScreenOrientation != Screen.orientation
            || mWidth != Screen.width
            || mHeight != Screen.height)
        {
            NeedReset = true;
        }
    }

    void LateUpdate()
    {
        if (NeedReset)
        {
            Reset();
            NeedReset = false;
        }
    }

    /// <summary>
    /// Reset all object to current scale
    /// </summary>
    private void Reset()
    {
        mWidth = Screen.width;
        mHeight = Screen.height;
        mScreenOrientation = Screen.orientation;

        // Reset all UIRoot for NGUI.
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("GUIRoot"))
        {
            UIRoot root = obj.GetComponent<UIRoot>();
            if (root != null)
            {
                ResetNGUIRoot(root);
            }
        }
    }

    /// <summary>
    /// Reset all UIRoot for NGUI.
    /// </summary>
    /// <param name="_root"></param>
    private void ResetNGUIRoot(UIRoot _root)
    {
        if (_root == null)
        {
            return;
        }

        UIRoot root = _root;
        if ((float)Screen.width / (float)Screen.height < DefaultRatio)
        {
            root.manualHeight = DefaultWidth * Screen.height / Screen.width;
        }
        else
        {
            root.manualHeight = (int)DefaultHeight;
        }
    }

    /// <summary>
    /// Change degrees to radians
    /// </summary>
    /// <param name="_degrees"></param>
    /// <returns></returns>
    private float D2R(float _degrees)
    {
        return (Mathf.PI / 180) * _degrees;
    }

    /// <summary>
    /// Change radians to degrees
    /// </summary>
    /// <param name="_radians"></param>
    /// <returns></returns>
    private float R2D(float _radians)
    {
        return (180 / Mathf.PI) * _radians;
    }
}
