using UnityEngine;
using System.Collections;

public class UIFontCenter : MonoBehaviour {

    static UIFontCenter mInstance;

    static public UIFontCenter instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = Object.FindObjectOfType(typeof(UIFontCenter)) as UIFontCenter;

                if (mInstance == null)
                {
                    GameObject go = new GameObject("_UIFontCenter");
                    DontDestroyOnLoad(go);
                    mInstance = go.AddComponent<UIFontCenter>();
                }
            }
            return mInstance;
        }
    }

    public UIFont startingFont;
    
    private UIFont mCurrentFont;
    public UIFont currentFont
    {
        get { return mCurrentFont; }
        set
        {
            mCurrentFont = value;

        }
    }

    void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this;
            DontDestroyOnLoad(gameObject);

            currentFont = startingFont;
            UIRoot.Broadcast("OnFont", currentFont);
        }
        else
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
